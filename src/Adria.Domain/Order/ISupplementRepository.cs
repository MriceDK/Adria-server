namespace Adria.Domain.Order;

public interface ISupplementRepository
{
    Task<Supplement?> ById(Guid supplementId);
    Task<IReadOnlyCollection<Supplement>> ByName(string name);
    Task<IReadOnlyCollection<Supplement>> ByType(string type);
    Task<IReadOnlyCollection<Supplement>> GetAll();
    Task Save(Supplement supplement);
}