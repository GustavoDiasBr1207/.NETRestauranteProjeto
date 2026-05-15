namespace RestaurantOrders.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RestaurantOrders.API.Models;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Application.Orders.Commands.CreateOrder;
using RestaurantOrders.Application.Orders.Commands.AddItemToOrder;
using RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;
using RestaurantOrders.Application.Orders.Commands.SubmitOrder;
using RestaurantOrders.Application.Orders.Commands.UpdateOrderStatus;
using RestaurantOrders.Application.Orders.Commands.CancelOrder;
using RestaurantOrders.Application.Orders.Queries.GetOrderById;
using RestaurantOrders.Application.Orders.Queries.GetOrdersByTable;
using RestaurantOrders.Application.Orders.Queries.GetActiveOrdersByRestaurant;

/// <summary>
/// Criação e ciclo de vida dos pedidos — do carrinho à entrega na mesa.
/// </summary>
/// <remarks>
/// Fluxo principal:
/// <br/>1. <c>POST /api/orders</c> — cria pedido em rascunho (carrinho)
/// <br/>2. <c>POST /api/orders/{id}/items</c> — adiciona itens
/// <br/>3. <c>POST /api/orders/{id}/submit</c> — envia para cozinha
/// <br/>4. <c>PATCH /api/orders/{id}/status</c> — cozinha avança o status
/// </remarks>
[ApiController]
[Route("api/orders")]
[Produces("application/json")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    /// <summary>Cria um novo pedido em modo rascunho (carrinho).</summary>
    /// <remarks>
    /// Chamado logo após o cliente escanear o QR Code e o app resolver a mesa.
    /// Retorna o <c>orderId</c> que será usado nas demais operações do carrinho.
    /// </remarks>
    /// <param name="command">Dados do pedido: restaurante, mesa e cliente opcional.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="201">Pedido criado. Contém o <c>orderId</c> gerado.</response>
    /// <response code="400">Dados inválidos (restaurantId ou tableId ausentes).</response>
    /// <response code="422">Regra de negócio violada.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new CreateOrderResponse(id));
    }

    /// <summary>Retorna um pedido pelo seu identificador.</summary>
    /// <param name="id">ID do pedido (UUID).</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="200">Pedido encontrado com seus itens.</response>
    /// <response code="404">Pedido não existe.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await mediator.Send(new GetOrderByIdQuery { OrderId = id }, ct);
        return order is null ? NotFound() : Ok(order);
    }

    /// <summary>Lista pedidos, filtrando por mesa ou por restaurante (ativos).</summary>
    /// <remarks>
    /// Informe <b>apenas um</b> dos parâmetros:
    /// <br/>• <c>tableId</c> — pedidos da mesa (para o cliente acompanhar)
    /// <br/>• <c>restaurantId</c> — pedidos ativos do restaurante (para o painel da cozinha)
    /// </remarks>
    /// <param name="tableId">Filtra pedidos de uma mesa específica.</param>
    /// <param name="restaurantId">Filtra pedidos ativos de um restaurante.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="200">Lista de pedidos.</response>
    /// <response code="400">Nenhum filtro informado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] Guid? tableId,
        [FromQuery] Guid? restaurantId,
        CancellationToken ct)
    {
        if (tableId.HasValue)
        {
            var orders = await mediator.Send(new GetOrdersByTableQuery { TableId = tableId.Value }, ct);
            return Ok(orders);
        }

        if (restaurantId.HasValue)
        {
            var orders = await mediator.Send(
                new GetActiveOrdersByRestaurantQuery { RestaurantId = restaurantId.Value }, ct);
            return Ok(orders);
        }

        return BadRequest(new ErrorResponse { Status = 400, Message = "Informe tableId ou restaurantId." });
    }

    /// <summary>Adiciona um item ao carrinho.</summary>
    /// <remarks>
    /// Só é permitido enquanto o pedido estiver no status <c>Draft</c>.
    /// Se o item já existir no pedido, a quantidade é somada.
    /// </remarks>
    /// <param name="id">ID do pedido.</param>
    /// <param name="command">Item a adicionar: menuItemId, quantidade e observação opcional.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Item adicionado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="404">Pedido ou item do cardápio não encontrado.</response>
    /// <response code="422">Item indisponível ou pedido não está em rascunho.</response>
    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddItem(
        Guid id,
        [FromBody] AddItemToOrderCommand command,
        CancellationToken ct)
    {
        command.OrderId = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Remove um item do carrinho.</summary>
    /// <remarks>Só é permitido enquanto o pedido estiver no status <c>Draft</c>.</remarks>
    /// <param name="id">ID do pedido.</param>
    /// <param name="itemId">ID do item do pedido a remover.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Item removido.</response>
    /// <response code="404">Pedido ou item não encontrado.</response>
    /// <response code="422">Pedido não está em rascunho.</response>
    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RemoveItem(Guid id, Guid itemId, CancellationToken ct)
    {
        await mediator.Send(new RemoveItemFromOrderCommand { OrderId = id, OrderItemId = itemId }, ct);
        return NoContent();
    }

    /// <summary>Envia o pedido para a cozinha.</summary>
    /// <remarks>
    /// Transição de status: <c>Draft → Pending</c>.
    /// Após o envio, a cozinha é notificada via Supabase Realtime.
    /// O pedido precisa ter ao menos um item.
    /// </remarks>
    /// <param name="id">ID do pedido.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Pedido enviado para a cozinha.</response>
    /// <response code="404">Pedido não encontrado.</response>
    /// <response code="422">Pedido vazio ou status inválido para envio.</response>
    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Submit(Guid id, CancellationToken ct)
    {
        await mediator.Send(new SubmitOrderCommand { OrderId = id }, ct);
        return NoContent();
    }

    /// <summary>Avança o status do pedido (uso da cozinha / gerente).</summary>
    /// <remarks>
    /// Transições permitidas:
    /// <br/>• <c>Pending → Confirmed</c>
    /// <br/>• <c>Confirmed → Preparing</c>
    /// <br/>• <c>Preparing → Ready</c>
    /// <br/>• <c>Ready → Delivered</c>
    /// <br/>• Qualquer status (exceto Delivered) → <c>Cancelled</c>
    ///
    /// Requer token JWT de usuário autenticado (cozinheiro ou gerente).
    /// </remarks>
    /// <param name="id">ID do pedido.</param>
    /// <param name="command">Novo status desejado.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Status atualizado.</response>
    /// <response code="400">Novo status inválido.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Pedido não encontrado.</response>
    /// <response code="422">Transição de status não permitida.</response>
    [HttpPatch("{id:guid}/status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateOrderStatusCommand command,
        CancellationToken ct)
    {
        command.OrderId = id;
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Cancela um pedido.</summary>
    /// <remarks>
    /// Permitido em qualquer status exceto <c>Delivered</c> e <c>Cancelled</c>.
    /// </remarks>
    /// <param name="id">ID do pedido.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Pedido cancelado.</response>
    /// <response code="404">Pedido não encontrado.</response>
    /// <response code="422">Pedido já entregue ou cancelado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await mediator.Send(new CancelOrderCommand { OrderId = id }, ct);
        return NoContent();
    }
}
