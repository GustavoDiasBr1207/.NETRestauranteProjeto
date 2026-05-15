namespace RestaurantOrders.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using RestaurantOrders.API.Models;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Application.Menu.Queries.GetMenuByRestaurant;

/// <summary>
/// Consulta do cardápio por restaurante.
/// </summary>
/// <remarks>
/// Endpoint público usado pelo app do cliente logo após escanear o QR Code da mesa.
/// Retorna todas as categorias com seus itens disponíveis.
/// </remarks>
[ApiController]
[Route("api/menu")]
[Produces("application/json")]
public class MenuController(IMediator mediator) : ControllerBase
{
    /// <summary>Retorna o cardápio completo de um restaurante.</summary>
    /// <remarks>
    /// Chamado pelo app após resolver a mesa via QR Code.
    /// Retorna categorias ordenadas por <c>DisplayOrder</c>, cada uma com seus
    /// itens disponíveis também ordenados.
    /// <br/><br/>
    /// Exemplo de uso após o scan:
    /// <br/><c>GET /api/menu?restaurantId=3fa85f64-5717-4562-b3fc-2c963f66afa6</c>
    /// </remarks>
    /// <param name="restaurantId">ID do restaurante obtido em <c>GET /api/tables/by-qrcode/{token}</c>.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="200">Lista de categorias com itens disponíveis.</response>
    /// <response code="400">restaurantId não informado ou inválido.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MenuCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMenu(
        [FromQuery] Guid restaurantId,
        CancellationToken ct)
    {
        var categories = await mediator.Send(
            new GetMenuByRestaurantQuery { RestaurantId = restaurantId }, ct);

        return Ok(categories);
    }
}
