using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adria.Application.Scanner;

namespace UnitTests.Mocks;

public sealed class MockGetFoodNutrientsUseCase
    : IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>>
{
    private IReadOnlyCollection<NutrientInfo> _returnValue =
        Array.Empty<NutrientInfo>();

    public void SetReturnValue(IReadOnlyCollection<NutrientInfo> value)
    {
        _returnValue = value;
    }

    public Task<IReadOnlyCollection<NutrientInfo>> Execute(GetFoodNutrientsInput input)
    {
        return Task.FromResult(_returnValue);
    }
}