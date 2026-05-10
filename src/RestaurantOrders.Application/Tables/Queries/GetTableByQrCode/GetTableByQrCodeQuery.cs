namespace RestaurantOrders.Application.Tables.Queries.GetTableByQrCode;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// Query to get table by QR code
/// </summary>
public class GetTableByQrCodeQuery : IRequest<TableDto?>
{
    public string QrCodeToken { get; set; } = string.Empty;
}
