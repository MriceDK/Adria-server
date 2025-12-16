using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using System;
using System.Threading.Tasks;
using Adria.Application.Contracts;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class SupplementsByIdQuery(
    ISupplementRepository supplementRepository) : ISupplementsByIdQuery
{
    public async Task<SupplementData?> Fetch(Guid supplementId)
    {
        var supplement = await supplementRepository.ById(supplementId);

        if (supplement == null)
        {
            return null;
        }

        return new SupplementData(
            supplement.SupplementId, 
            supplement.Name, 
            supplement.Type, 
            supplement.Price, 
            supplement.Stock
        );
    }
}