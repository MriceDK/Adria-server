using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adria.Application.Contracts;
using Adria.Domain.Shared.Exceptions;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class AllSupplementsQuery(
    ISupplementRepository supplementRepository) : IAllSupplementsQuery
{
    public async Task<IReadOnlyCollection<SupplementData?>> Fetch()
    {
        var supplements = await supplementRepository.GetAll();

        if (!supplements.Any())
        {
            throw new ElementNotFoundException("No supplements found.");
        }
        
        return supplements
            .Select(s => new SupplementData(s.SupplementId, s.Name, s.Type, s.Price, s.Stock))
            .ToList()
            .AsReadOnly();
    }
}