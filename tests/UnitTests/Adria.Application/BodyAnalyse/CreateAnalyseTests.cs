using Adria.Application.Analyses;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Analyses;

public sealed class CreateAnalyseTests
{
    private readonly MockAnalyseRepository _mockRepository;
    private readonly MockLogger<CreateAnalyse> _mockLogger;
    private readonly CreateAnalyse _useCase;
    private readonly Guid _validUserId;
    private readonly List<AnalyseDetailInput> _validDetails;

    public CreateAnalyseTests()
    {
        _mockRepository = new MockAnalyseRepository();
        _mockLogger = new MockLogger<CreateAnalyse>();
        _useCase = new CreateAnalyse(_mockRepository, _mockLogger);

        _validUserId = Guid.NewGuid();
        _validDetails = new List<AnalyseDetailInput>
        {
            new AnalyseDetailInput(Guid.NewGuid(), 75.5),
            new AnalyseDetailInput(Guid.NewGuid(), 18.2)
        };
    }

    [Fact]
    public async Task Execute_WithValidInput_CreatesAnalyseAndReturnsId()
    {
        // Arrange
        var input = new CreateAnalyseInput(
            _validUserId,
            DateTime.UtcNow,
            _validDetails
        );

        // Act
        var result = await _useCase.Execute(input);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        
        var savedAnalyse = await _mockRepository.ById(result);
        Assert.NotNull(savedAnalyse);
        Assert.Equal(_validUserId, savedAnalyse.AdrianId);
        Assert.Equal(2, savedAnalyse.Details.Count);
        
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains("Created new analyse", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithEmptyUserId_ThrowsArgumentException()
    {
        // Arrange
        var input = new CreateAnalyseInput(
            Guid.Empty, 
            DateTime.UtcNow,
            _validDetails
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WithEmptyDetails_ThrowsArgumentException()
    {
        // Arrange
        var input = new CreateAnalyseInput(
            _validUserId,
            DateTime.UtcNow,
            new List<AnalyseDetailInput>() 
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
    }
    
}