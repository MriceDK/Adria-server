namespace Adria.Domain.BodyStatus;

public interface IAnalyseRepository
{
    Task<Analyse?> ById(Guid analyseId);

    Task Save(Analyse analyse);
}