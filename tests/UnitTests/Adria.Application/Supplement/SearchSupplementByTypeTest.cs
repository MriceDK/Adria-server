using Adria.Application.Supplement;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using System;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.Supplement;

public sealed class SearchSupplementByTypeTest
{
    [Fact]
    public async Task Execute_WithExistingType_ReturnsSupplements()
    {
        var query = new MockSupplementsByTypeQuery();
        var logger = new MockLogger<SearchSupplementsByType>();

        var supplements = new[]
        {
            new SupplementData(Guid.NewGuid(), "Whey Protein", "Protein", 39.99, 100),
            new SupplementData(Guid.NewGuid(), "Casein Protein", "Protein", 44.99, 50)
        };

        query.SetReturnValue(supplements);

        var useCase = new SearchSupplementsByType(query, logger);

        var input = new SearchSupplementsByTypeInput("Protein");

        var result = await useCase.Execute(input);

        Assert.Equal(2, result.Count);
        Assert.Equal(supplements, result);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Fetching supplements with type", logger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithUnknownType_ThrowsElementNotFoundException()
    {
        var query = new MockSupplementsByTypeQuery();
        var logger = new MockLogger<SearchSupplementsByType>();

        query.SetReturnValue(null);

        var useCase = new SearchSupplementsByType(query, logger);

        var input = new SearchSupplementsByTypeInput("UnknownType");

        await Assert.ThrowsAsync<ElementNotFoundException>(
            () => useCase.Execute(input)
        );
    }
}