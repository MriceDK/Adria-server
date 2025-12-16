using Adria.Application.Supplement;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTests.Adria.Application.Supplement;

public sealed class CreateSupplementTests
{
    private readonly MockSupplementRepository _mockSupplementRepository;
    private readonly MockLogger<CreateSupplement> _mockLogger;
    private readonly CreateSupplement _useCase;

    private readonly Guid _validSupplementId = Guid.NewGuid();
    private const string _validName = "Protein Powder";
    private const string _validType = "Protein";
    private const double _validPrice = 39.99;
    private const int _validStock = 100;

    public CreateSupplementTests()
    {
        _mockSupplementRepository = new MockSupplementRepository();
        _mockLogger = new MockLogger<CreateSupplement>();

        _useCase = new CreateSupplement(_mockSupplementRepository, _mockLogger);
    }

    private CreateSupplementInput GetValidInput()
    {
        return new CreateSupplementInput(
            _validSupplementId,
            _validName,
            _validType,
            _validPrice,
            _validStock
        );
    }

    [Fact]
    public async Task Execute_WithValidInput_CreatesSupplementAndReturnsId()
    {
        var input = GetValidInput();

        var resultId = await _useCase.Execute(input);

        Assert.Equal(_validSupplementId, resultId);

        var savedSupplement = await _mockSupplementRepository.ById(resultId);

        Assert.NotNull(savedSupplement);
        Assert.Equal(_validName, savedSupplement!.Name);
        Assert.Equal(_validType, savedSupplement.Type);
        Assert.Equal(_validPrice, savedSupplement.Price);
        Assert.Equal(_validStock, savedSupplement.Stock);

        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains("New supplement created with ID", _mockLogger.LoggedMessages[0]);
    }

    [Theory]
    [InlineData(-10.00, 100)]
    [InlineData(39.99, -5)]
    public async Task Execute_WithInvalidValues_ThrowsArgumentExceptionAndDoesNotSave(
        double price,
        int stock)
    {
        var supplementId = Guid.NewGuid();
        var input = new CreateSupplementInput(
            supplementId,
            _validName,
            _validType,
            price,
            stock
        );
        _mockLogger.Clear();

        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));

        var savedSupplement = await _mockSupplementRepository.ById(supplementId);
        Assert.Null(savedSupplement);

        Assert.Empty(_mockLogger.LoggedMessages);
    }

    [Fact]
    public async Task Execute_WithEmptyName_ThrowsArgumentExceptionAndDoesNotSave()
    {
        var supplementId = Guid.NewGuid();
        var input = new CreateSupplementInput(
            supplementId,
            string.Empty,
            _validType,
            _validPrice,
            _validStock
        );

        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
        Assert.Null(await _mockSupplementRepository.ById(supplementId));
    }
}
