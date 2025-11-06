using Adria.Domain.Subcriptions;

namespace Adria.Application.Contracts.Data;

public sealed record UserData(
 Guid AdrianId,
 string Name ,
 string Job ,
 Subscription Subscription 
);