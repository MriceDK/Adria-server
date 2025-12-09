namespace Adria.Domain.BodyStats;

public interface IBodyStatRepository
{
    Task<BodyStat?> ById(string bodyStatId);

    Task Save(BodyStat bodyStat);
}