using Adria.Application.Contracts;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.BodyStats;

public sealed record UpdateBodyStatGoalInput(Guid BodyStatId, double? NewGoal);

public sealed class UpdateBodyStatGoal(IBodyStatRepository repository, ILogger<UpdateBodyStatGoal> logger)
    : IUseCase<UpdateBodyStatGoalInput, Task>
{
    public async Task Execute(UpdateBodyStatGoalInput input)
    {
        var bodyStat = await repository.ById(input.BodyStatId);

        if (bodyStat is null)
        {
            throw new ElementNotFoundException($"BodyStat with ID {input.BodyStatId} not found.");
        }

        bodyStat.UpdateGoal(input.NewGoal);

        await repository.Update(bodyStat);

        logger.LogInformation("Updated goal for BodyStat {BodyStatId} to {NewGoal}", input.BodyStatId, input.NewGoal);
    }
}