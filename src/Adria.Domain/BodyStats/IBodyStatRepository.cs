namespace Adria.Domain.BodyStatus;

public interface IBodyStatRepository
{
    Task<BodyStat?> ById(Guid bodyStatId);

    Task Save(BodyStat bodyStat);
}