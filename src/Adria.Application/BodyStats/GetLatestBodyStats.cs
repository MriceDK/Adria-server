using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.BodyStats;

public sealed record GetLatestBodyStatsInput(Guid UserId);

public class GetLatestBodyStats : IUseCase<GetLatestBodyStatsInput, Task<IReadOnlyCollection<BodyStatData>>>
{
    private readonly IBodyStatsQuery _bodyStatsQuery;
    private readonly ILogger<GetLatestBodyStats> _logger;

    public GetLatestBodyStats(
        IBodyStatsQuery bodyStatsQuery,
        ILogger<GetLatestBodyStats> logger)
    {
        _bodyStatsQuery = bodyStatsQuery;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<BodyStatData>> Execute(GetLatestBodyStatsInput input)
    {
        _logger.LogInformation("Fetching latest body stats for user {UserId}", input.UserId);
        return await _bodyStatsQuery.Fetch(input.UserId) ?? throw new InvalidOperationException();
    }
}