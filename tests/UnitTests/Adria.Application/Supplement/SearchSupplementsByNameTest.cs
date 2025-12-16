using Adria.Application.Supplement;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Supplement;

public sealed class SearchSupplementsByNameTest
{
    [Fact]
    public async Task Execute_WithExistingName_ReturnsSupplements()
    {
        var query = new MockSupplementsByNameQuery();
        var logger = new MockLogger<SearchSupplementsByName>();

        var supplements = new[]
        {
            new SupplementData(Guid.NewGuid(), "Protein Powder", "Protein", 39.99, 100),
            new SupplementData(Guid.NewGuid(), "Protein Bar", "Protein", 2.99, 250)
        };

        query.SetReturnValue(supplements);

        var useCase = new SearchSupplementsByName(query, logger);

        var input = new SearchSupplementsByNameInput("Protein");

        var result = await useCase.Execute(input);

        Assert.Equal(2, result.Count);
        Assert.Equal(supplements, result);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Fetching supplements with name", logger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithUnknownName_ThrowsElementNotFoundException()
    {
        var query = new MockSupplementsByNameQuery();
        var logger = new MockLogger<SearchSupplementsByName>();

        query.SetReturnValue(null);

        var useCase = new SearchSupplementsByName(query, logger);

        var input = new SearchSupplementsByNameInput("Unknown");

        await Assert.ThrowsAsync<ElementNotFoundException>(
            () => useCase.Execute(input)
        );
    }
}