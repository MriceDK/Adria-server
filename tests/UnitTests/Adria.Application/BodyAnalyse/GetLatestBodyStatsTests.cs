using Adria.Application.BodyStats;
using Adria.Application.Contracts.Data;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Analyses;

public sealed class GetLatestBodyStatsTests
{
    [Fact]
    public async Task Execute_WithExistingStats_ReturnsList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var statsData = new List<BodyStatData>
        {
            new BodyStatData("bd4-weig-0002","Weight", 75, 70, "kg"),
            new BodyStatData("bd4-fatp-0001","Body Fat", 15, 12, "%")
        };

        var mockQuery = new MockBodyStatsQuery();
        mockQuery.AddStats(userId, statsData);

        var mockLogger = new MockLogger<GetLatestBodyStats>();
        var useCase = new GetLatestBodyStats(mockQuery, mockLogger);
        var input = new GetLatestBodyStatsInput(userId);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Label == "Weight" && Math.Abs(r.Current - 75) < 1);
        
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching latest body stats for user {userId}", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNoStats_ReturnsEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockQuery = new MockBodyStatsQuery(); // Veri eklemiyoruz
        var mockLogger = new MockLogger<GetLatestBodyStats>();
        var useCase = new GetLatestBodyStats(mockQuery, mockLogger);
        var input = new GetLatestBodyStatsInput(userId);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        
        Assert.Single(mockLogger.LoggedMessages);
    }
}