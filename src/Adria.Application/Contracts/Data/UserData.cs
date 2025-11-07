using Adria.Domain.Subcriptions;

namespace Adria.Application.Contracts.Data;

public sealed record UserData(
 Guid AdriaId,
 string Name ,
 string Job ,
 string Subscription 
);