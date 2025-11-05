namespace Adria.Domain.Order;

public interface ISupplement
{
    Task<Supplement?> BySupplementId(Guid supplementId);
    Task<Supplement?> ByName(string name);
    Task<Supplement?> ByType(string type);
    Task<IReadOnlyCollection<Supplement>> GetAll();
    Task Save(Supplement supplement);
    Task Remove(Supplement supplement);
}