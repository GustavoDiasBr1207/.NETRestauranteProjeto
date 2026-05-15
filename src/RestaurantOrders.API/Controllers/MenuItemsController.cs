namespace RestaurantOrders.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RestaurantOrders.API.Models;
using RestaurantOrders.Application.Menu.Commands.CreateMenuItem;
using RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

/// <summary>
/// Gestão de itens do cardápio — criação e controle de disponibilidade.
/// </summary>
/// <remarks>
/// Todos os endpoints requerem autenticação JWT (usuário gerente ou administrador).
/// </remarks>
[ApiController]
[Route("api/menu-items")]
[Authorize]
[Produces("application/json")]
public class MenuItemsController(IMediator mediator) : ControllerBase
{
    /// <summary>Cria um novo item no cardápio.</summary>
    /// <remarks>
    /// O item é criado como disponível por padrão (<c>IsAvailable = true</c>).
    /// Use <c>PATCH /{id}/availability</c> para desabilitá-lo temporariamente
    /// (ex: item esgotado).
    /// </remarks>
    /// <param name="command">Dados do item: nome, preço, categoria, descrição e URL da imagem.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="201">Item criado. Contém o <c>menuItemId</c> gerado.</response>
    /// <response code="400">Dados inválidos (nome ou preço ausentes/inválidos).</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="422">Categoria ou restaurante não encontrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateMenuItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(null, new { id }, new CreateMenuItemResponse(id));
    }

    /// <summary>Habilita ou desabilita a disponibilidade de um item do cardápio.</summary>
    /// <remarks>
    /// Itens desabilitados (<c>IsAvailable = false</c>) não aparecem no cardápio do cliente
    /// e não podem ser adicionados a novos pedidos.
    /// Útil para marcar um item como esgotado temporariamente sem removê-lo.
    /// </remarks>
    /// <param name="id">ID do item do cardápio.</param>
    /// <param name="request">Novo valor de disponibilidade.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Disponibilidade atualizada.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Item não encontrado.</response>
    [HttpPatch("{id:guid}/availability")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAvailability(
        Guid id,
        [FromBody] UpdateAvailabilityRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateMenuItemAvailabilityCommand(id, request.IsAvailable), ct);
        return NoContent();
    }
}
