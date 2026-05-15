namespace RestaurantOrders.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using RestaurantOrders.API.Models;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Application.Tables.Queries.GetTableByQrCode;

/// <summary>
/// Resolução de mesa a partir do token gerado pelo QR Code.
/// </summary>
[ApiController]
[Route("api/tables")]
[Produces("application/json")]
public class TablesController(IMediator mediator) : ControllerBase
{
    /// <summary>Resolve a mesa pelo token do QR Code escaneado.</summary>
    /// <remarks>
    /// Primeiro endpoint chamado no fluxo do cliente.
    /// O app lê o QR Code da mesa (que contém a URL com o token), chama este endpoint
    /// e recebe o <c>tableId</c> e o <c>restaurantId</c> necessários para abrir o cardápio
    /// e criar o pedido.
    ///
    /// <br/><br/>Exemplo de URL embutida no QR Code:
    /// <br/><c>https://app.restaurante.com/mesa?token=abc123def456</c>
    /// <br/><br/>Chamada correspondente:
    /// <br/><c>GET /api/tables/by-qrcode/abc123def456</c>
    /// </remarks>
    /// <param name="token">Token único do QR Code da mesa (32 caracteres hex).</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="200">Dados da mesa: id, número, restaurantId e token.</response>
    /// <response code="404">QR Code inválido ou mesa inativa.</response>
    [HttpGet("by-qrcode/{token}")]
    [ProducesResponseType(typeof(TableDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByQrCode(string token, CancellationToken ct)
    {
        var table = await mediator.Send(new GetTableByQrCodeQuery { QrCodeToken = token }, ct);

        return table is null
            ? NotFound(new ErrorResponse { Status = 404, Message = "Mesa não encontrada ou QR Code inválido." })
            : Ok(table);
    }
}
