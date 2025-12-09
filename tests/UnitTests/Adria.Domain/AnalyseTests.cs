using Adria.Domain.BodyStats;

namespace UnitTests.Adria.Domain;

public sealed class AnalyseTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesAnalyse()
    {
        // Arrange
        var adrianId = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;
        var details = new List<AnalyseDetail>
        {
            new AnalyseDetail("bd2-calc-0001", 75.5),
            new AnalyseDetail("bd2-iron-0002", 18.2)
        };

        // Act
        var analyse = new Analyse(adrianId, dateTime, details);

        // Assert
        Assert.Equal(adrianId, analyse.AdrianId);
        Assert.Equal(dateTime, analyse.DateTime);
        Assert.Equal(details.Count, analyse.Details.Count);
        Assert.NotEqual(Guid.Empty, analyse.Id);
    }

    [Fact]
    public void Constructor_WithValidParametersAndId_CreatesAnalyse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var adrianId = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;
        var details = new List<AnalyseDetail> { new AnalyseDetail("bd2-magn-0003", 10) };

        // Act
        var analyse = new Analyse(adrianId, dateTime, details, id);

        // Assert
        Assert.Equal(id, analyse.Id);
        Assert.Equal(adrianId, analyse.AdrianId);
    }

    [Fact]
    public void Constructor_WithEmptyUserId_ThrowsArgumentException()
    {
        // Arrange
        var invalidUserId = Guid.Empty;
        var dateTime = DateTime.UtcNow;
        var details = new List<AnalyseDetail> { new AnalyseDetail("bd2-magn-0003", 10) };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Analyse(invalidUserId, dateTime, details));
    }

    [Fact]
    public void Constructor_WithNullDetails_ThrowsArgumentException()
    {
        // Arrange
        var adrianId = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Analyse(adrianId, dateTime, null!));
    }

    [Fact]
    public void Constructor_WithEmptyDetailsList_ThrowsArgumentException()
    {
        // Arrange
        var adrianId = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;
        var emptyDetails = new List<AnalyseDetail>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Analyse(adrianId, dateTime, emptyDetails));
    }
}