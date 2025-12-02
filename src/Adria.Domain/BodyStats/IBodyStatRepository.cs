namespace Adria.Domain.BodyStats;

public interface IBodyStatRepository
{
    Task<BodyStat?> ById(Guid bodyStatId);

    Task Save(BodyStat bodyStat);
}