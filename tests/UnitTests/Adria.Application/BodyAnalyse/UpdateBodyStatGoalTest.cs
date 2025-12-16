using Adria.Application.BodyStats;
using Adria.Domain.BodyStats;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.Analyses;

public sealed class UpdateBodyStatGoalTest
{
    [Fact]
    public async Task Execute_WithExistingBodyStat_UpdatesGoal()
    {
        var repository = new MockBodyStatRepository();
        var logger = new MockLogger<UpdateBodyStatGoal>();
        var useCase = new UpdateBodyStatGoal(repository, logger);

        var bodyStatId = "bd2-calc-0001";
        var bodyStat = new BodyStat(
            "Calcium",
            "Calories",
            2000,
            bodyStatId
            );

        repository.Seed(bodyStat);

        var input = new UpdateBodyStatGoalInput(bodyStatId, 1800);

        var exception = await Record.ExceptionAsync(() => useCase.Execute(input));

        Assert.Null(exception);
        Assert.Equal(1800, bodyStat.Goal);
        Assert.Single(repository.UpdatedEntities);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Updated goal for BodyStat", logger.LoggedMessages[0]);
    }
}