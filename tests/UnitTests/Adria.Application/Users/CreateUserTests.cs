using Adria.Application.Users;
using Adria.Domain.Subcriptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class CreateUserTests
{
private readonly MockUserRepository _mockUserRepository;
    private readonly MockLogger<CreateUser> _mockLogger;
    private readonly CreateUser _useCase;
    private readonly Subscription _defaultSubscription;

    public CreateUserTests()
    {
        _mockUserRepository = new MockUserRepository();
        _mockLogger = new MockLogger<CreateUser>();
        
        _defaultSubscription = new Subscription(
            SubscriptionType.Basic, 
            9.99, 
            "Basic user advantages"
        );
        
        _useCase = new CreateUser(_mockUserRepository, _mockLogger);
    }

    [Fact]
    public async Task Execute_WithValidInput_CreatesUserAndReturnsId()
    {
        // Arrange
        var input = new CreateUserInput(
            "John Doe", 
            "Software Developer", 
            _defaultSubscription
        );

        // Act
        var result = await _useCase.Execute(input);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        var savedUser = await _mockUserRepository.ById(result);
        Assert.NotNull(savedUser);
        Assert.Equal("John Doe", savedUser.Name);
        Assert.Equal("Software Developer", savedUser.Job);
        Assert.Equal(_defaultSubscription, savedUser.Subscription);
        Assert.Equal(result, savedUser.AdriaId); 
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains("New user created", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var input = new CreateUserInput(
            "", 
            "Software Developer", 
            _defaultSubscription
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
    }
    
    [Fact]
    public async Task Execute_WithNullName_ThrowsArgumentException()
    {
        // Arrange
        var input = new CreateUserInput(
            null!, 
            "Software Developer", 
            _defaultSubscription
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WithEmptyJob_ThrowsArgumentException()
    {
        // Arrange
        var input = new CreateUserInput(
            "John Doe", 
            "", 
            _defaultSubscription
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
    }
    
    
}