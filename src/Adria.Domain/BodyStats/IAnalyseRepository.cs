namespace Adria.Domain.BodyStats;

public interface IAnalyseRepository
{
    Task Save(Analyse analyse);
}