namespace RestaurantOrders.Application.Tables.Queries.GetTableByQrCode;

using MediatR;
using AutoMapper;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for GetTableByQrCodeQuery
/// </summary>
public class GetTableByQrCodeQueryHandler : IRequestHandler<GetTableByQrCodeQuery, TableDto?>
{
    private readonly ITableRepository _tableRepository;
    private readonly IMapper _mapper;
    
    public GetTableByQrCodeQueryHandler(ITableRepository tableRepository, IMapper mapper)
    {
        _tableRepository = tableRepository;
        _mapper = mapper;
    }
    
    public async Task<TableDto?> Handle(GetTableByQrCodeQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get table by QR code
        var table = await _tableRepository.GetByQrCodeTokenAsync(request.QrCodeToken, cancellationToken);
        
        if (table == null)
            return null;
        
        return _mapper.Map<TableDto>(table);
    }
}
