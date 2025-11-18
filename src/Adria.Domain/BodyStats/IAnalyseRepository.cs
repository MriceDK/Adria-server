namespace Adria.Domain.BodyStatus;

public interface IAnalyseRepository
{
    Task Save(Analyse analyse);
}