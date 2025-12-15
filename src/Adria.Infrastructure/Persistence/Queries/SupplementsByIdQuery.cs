using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using System;
using System.Threading.Tasks;
using Adria.Application.Contracts;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class SupplementsByIdQuery(
    ISupplementRepository supplementRepository) : ISupplementsByIdQuery
{
    public async Task<SupplementData?> Fetch(Guid id)
    {
        var supplement = await supplementRepository.ById(id);

        if (supplement == null)
        {
            return null;
        }

        // Map Domain Entity to DTO
        return new SupplementData(
            supplement.SupplementId, 
            supplement.Name, 
            supplement.Type, 
            supplement.Price, 
            supplement.Stock
        );
    }
}