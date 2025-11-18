using Adria.Domain.BodyStatus;

namespace UnitTests.Adria.Domain;

public sealed class BodyStatTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesBodyStat()
    {
        // Arrange
        const string label = "Body Fat";
        const string unit = "%";
        const double goal = 15.5;

        // Act
        var bodyStat = new BodyStat(label, unit, goal);

        // Assert
        Assert.Equal(label, bodyStat.Label);
        Assert.Equal(unit, bodyStat.Unit);
        Assert.Equal(goal, bodyStat.Goal);
        Assert.NotEqual(Guid.Empty, bodyStat.Id);
    }

    [Fact]
    public void Constructor_WithValidParametersAndId_CreatesBodyStat()
    {
        // Arrange
        const string label = "Weight";
        const string unit = "kg";
        const double goal = 80;
        var id = Guid.NewGuid();

        // Act
        var bodyStat = new BodyStat(label, unit, goal, id);

        // Assert
        Assert.Equal(label, bodyStat.Label);
        Assert.Equal(id, bodyStat.Id);
    }

    [Fact]
    public void Constructor_WithNullUnitAndGoal_CreatesBodyStat()
    {
        // Arrange
        const string label = "Mood Score";

        // Act
        var bodyStat = new BodyStat(label, null, null);

        // Assert
        Assert.Equal(label, bodyStat.Label);
        Assert.Null(bodyStat.Unit);
        Assert.Null(bodyStat.Goal);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidLabel_ThrowsArgumentException(string invalidLabel)
    {
        // Arrange
        const string unit = "%";
        const double goal = 10;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new BodyStat(invalidLabel, unit, goal));
    }
}