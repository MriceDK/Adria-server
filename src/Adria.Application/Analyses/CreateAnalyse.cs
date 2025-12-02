using Adria.Application.Contracts;
using Adria.Domain.BodyStatus;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Analyses;

public sealed record AnalyseDetailInput(
    string BodyStatId,
    double Value
);

public sealed record CreateAnalyseInput(
    Guid AdrianId,
    DateTime DateTime,
    List<AnalyseDetailInput> Details
);

public sealed class CreateAnalyse : IUseCase<CreateAnalyseInput, Task<Guid>>
{
    private readonly IAnalyseRepository _repository;
    private readonly ILogger<CreateAnalyse> _logger;

    public CreateAnalyse(IAnalyseRepository repository, ILogger<CreateAnalyse> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Execute(CreateAnalyseInput input)
    {
        var details = input.Details
            .Select(d => new AnalyseDetail(d.BodyStatId, d.Value))
            .ToList();

        var analyse = new Analyse(input.AdrianId, input.DateTime, details);

        await _repository.Save(analyse);

        _logger.LogInformation("Created new analyse {AnalyseId} for user {UserId}", analyse.Id, input.AdrianId);

        return analyse.Id;
    }
}