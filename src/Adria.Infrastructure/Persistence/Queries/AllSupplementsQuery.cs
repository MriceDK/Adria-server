using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adria.Application.Contracts;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class AllSupplementsQuery(
    ISupplementRepository supplementRepository) : IAllSupplementsQuery
{
    public async Task<IReadOnlyCollection<SupplementData?>> Fetch()
    {
        var supplements = await supplementRepository.GetAll();

        if (supplements == null || !supplements.Any())
        {
            return null;
        }
        
        return supplements
            .Select(s => new SupplementData(s.SupplementId, s.Name, s.Type, s.Price, s.Stock))
            .ToList()
            .AsReadOnly();
    }
}