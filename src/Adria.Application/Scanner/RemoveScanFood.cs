using Adria.Application.Contracts;
using Adria.Domain.Scanner;

namespace Adria.Application.Scanner;

public sealed record RemoveScanFoodInput(Guid ScanId);

public sealed class RemoveScanFood
    : IUseCase<RemoveScanFoodInput, Task>
{
    private readonly IScan _scanRepository;

    public RemoveScanFood(IScan scanRepository)
    {
        _scanRepository = scanRepository;
    }

    public async Task Execute(RemoveScanFoodInput input)
    {
        await _scanRepository.Remove(input.ScanId);
    }
}