// using Adria.Application.Contracts.Data;
// using Adria.Application.Order;
// using Adria.Domain.Order;
// using Adria.Domain.Shared.Exceptions;
// using UnitTests.Mocks;
//
// namespace Adria.Application.Tests.Order;
//
// public sealed class SearchOrderByUserIdTests
// {
//     
//     private readonly MockOrderByUserIdQuery _orderByUserIdQuery;
//     private readonly MockOrderSupplementDetailsRepository _supplementDetailsRepository;
//     private readonly MockSupplementRepository _supplementRepository;
//     private readonly MockLogger<SearchOrderByUserId> _logger;
//     private readonly SearchOrderByUserId _sut;
//
//     public SearchOrderByUserIdTests()
//     {
//         _orderByUserIdQuery = new MockOrderByUserIdQuery();
//         _supplementDetailsRepository = new MockOrderSupplementDetailsRepository();
//         _supplementRepository = new MockSupplementRepository();
//         _logger = new MockLogger<SearchOrderByUserId>();
//
//         _sut = new SearchOrderByUserId(
//             _orderByUserIdQuery,
//             _supplementDetailsRepository,
//             _supplementRepository,
//             _logger
//         );
//     }
//
//     [Fact]
//     public async Task Execute_WhenUserHasNoOrders_ReturnsEmptyCollection()
//     {
//         // Arrange
//         var adrianId = Guid.NewGuid();
//         var input = new SearchOrderByUserIdInput(adrianId);
//
//         _orderByUserIdQuery.SetOrders(adrianId, new List<OrderData>());
//
//         // Act
//         var result = await _sut.Execute(input);
//
//         // Assert
//         Assert.NotNull(result);
//         Assert.Empty(result);
//     }
//
//     [Fact]
//     public async Task Execute_WhenUserHasOrdersWithoutSupplements_ReturnsOrdersWithEmptySupplements()
//     {
//         // Arrange
//         var adrianId = Guid.NewGuid();
//         var orderId = Guid.NewGuid();
//         var input = new SearchOrderByUserIdInput(adrianId);
//
//         var orders = new List<OrderData>
//         {
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 100.0),
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 150.0),
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 200.0),
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 450.0)
//
//         };
//
//         _orderByUserIdQuery.SetOrders(adrianId, orders);
//         _supplementDetailsRepository.Seed(null);
//
//         // Act
//         var result = await _sut.Execute(input);
//
//         // Assert
//         Assert.NotNull(result);
//         Assert.Single(result);
//         var order = result.First();
//         Assert.Equal(orderId, order.OrderId);
//         Assert.Equal(adrianId, order.AdrianId);
//         Assert.Empty(order.Supplements);
//     }
//
//     [Fact]
//     public async Task Execute_WhenUserHasOrdersWithSupplements_ReturnsOrdersWithSupplementData()
//     {
//         // Arrange
//         var adrianId = Guid.NewGuid();
//         var orderId = Guid.NewGuid();
//         var supplementId1 = Guid.NewGuid();
//         var supplementId2 = Guid.NewGuid();
//         var input = new SearchOrderByUserIdInput(adrianId);
//
//         var orders = new List<OrderData>
//         {
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 150.0)
//         };
//
//         var supplementDetails = new List<OrderSupplementDetailsData>
//         {
//             new OrderSupplementDetailsData(orderId, supplementId1, 2),
//             new OrderSupplementDetailsData(orderId, supplementId2, 1)
//         };
//
//         var supplement1 = new OrderSupplementDetails(orderId, supplementId1, 5);
//         var supplement2 = new OrderSupplementDetails(orderId, supplementId2, 10);
//
//         _orderByUserIdQuery.SetOrders(adrianId, orders);
//
//         _supplementDetailsRepository.Seed(supplement1);
//         
//         _supplementDetailsRepository.Seed(supplement2);
//
//         // Act
//         var result = await _sut.Execute(input);
//
//         // Assert
//         Assert.NotNull(result);
//         Assert.Single(result);
//         var order = result.First();
//         Assert.Equal(orderId, order.OrderId);
//         Assert.Equal(adrianId, order.AdrianId);
//         Assert.Equal(2, order.Supplements.Count);
//         
//         var supp1 = order.Supplements.First(s => s.SupplementId == supplementId1);
//         Assert.Equal("Protein Powder", supp1.Name);
//         Assert.Equal("Protein", supp1.Type);
//         Assert.Equal(50.0, supp1.Price);
//
//         var supp2 = order.Supplements.First(s => s.SupplementId == supplementId2);
//         Assert.Equal("Creatine", supp2.Name);
//     }
//
//     [Fact]
//     public async Task Execute_WhenSupplementNotFound_ThrowsElementNotFoundException()
//     {
//         // Arrange
//         var adrianId = Guid.NewGuid();
//         var orderId = Guid.NewGuid();
//         var supplementId = Guid.NewGuid();
//         var input = new SearchOrderByUserIdInput(adrianId);
//
//         var orders = new List<OrderData>
//         {
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 100.0)
//         };
//
//         var supplementDetails = new List<OrderSupplementDetailsData>
//         {
//             new OrderSupplementDetailsData(orderId, supplementId, 1)
//         };
//
//         _orderByUserIdQuery
//             .Setup(x => x.Fetch(adrianId))
//             .ReturnsAsync(orders);
//
//         _supplementDetailsRepository
//             .Setup(x => x.ByOrderId(orderId))
//             .ReturnsAsync(supplementDetails);
//
//         _supplementRepository
//             .Setup(x => x.ById(supplementId))
//             .ReturnsAsync((SupplementData?)null);
//
//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
//             () => _sut.Execute(input)
//         );
//
//         Assert.Contains(supplementId.ToString(), exception.Message);
//     }
//
//     [Fact]
//     public async Task Execute_WhenUserHasMultipleOrders_ReturnsAllOrdersWithSupplements()
//     {
//         // Arrange
//         var adrianId = Guid.NewGuid();
//         var orderId1 = Guid.NewGuid();
//         var orderId2 = Guid.NewGuid();
//         var supplementId1 = Guid.NewGuid();
//         var supplementId2 = Guid.NewGuid();
//         var input = new SearchOrderByUserIdInput(adrianId);
//
//         var orders = new List<OrderData>
//         {
//             new OrderData(orderId1, adrianId, DateTime.UtcNow.AddDays(-5), 100.0),
//             new OrderData(orderId2, adrianId, DateTime.UtcNow, 200.0)
//         };
//
//         var supplementDetails1 = new List<OrderSupplementDetailsData>
//         {
//             new OrderSupplementDetailsData(orderId1, supplementId1, 1)
//         };
//
//         var supplementDetails2 = new List<OrderSupplementDetailsData>
//         {
//             new OrderSupplementDetailsData(orderId2, supplementId2, 2)
//         };
//
//         var supplement1 = new SupplementData(supplementId1, "Vitamin C", "Vitamin", 20.0, 200);
//         var supplement2 = new SupplementData(supplementId2, "Omega-3", "Essential Fatty Acid", 40.0, 150);
//
//         _orderByUserIdQuery(x => x.Fetch(adrianId))
//             .ReturnsAsync(orders);
//
//         _supplementDetailsRepository
//             .Setup(x => x.ByOrderId(orderId1))
//             .ReturnsAsync(supplementDetails1);
//
//         _supplementDetailsRepository
//             .Setup(x => x.ByOrderId(orderId2))
//             .ReturnsAsync(supplementDetails2);
//
//         _supplementRepository
//             .Setup(x => x.ById(supplementId1))
//             .ReturnsAsync(supplement1);
//
//         _supplementRepository
//             .Setup(x => x.ById(supplementId2))
//             .ReturnsAsync(supplement2);
//
//         // Act
//         var result = await _sut.Execute(input);
//
//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count);
//         
//         var order1 = result.First(o => o.OrderId == orderId1);
//         Assert.Single(order1.Supplements);
//         Assert.Equal("Vitamin C", order1.Supplements.First().Name);
//
//         var order2 = result.First(o => o.OrderId == orderId2);
//         Assert.Single(order2.Supplements);
//         Assert.Equal("Omega-3", order2.Supplements.First().Name);
//     }
//
//     [Fact]
//     public async Task Execute_LogsCorrectInformation()
//     {
//         // Arrange
//         var adrianId = Guid.NewGuid();
//         var orderId = Guid.NewGuid();
//         var input = new SearchOrderByUserIdInput(adrianId);
//
//         var orders = new List<OrderData>
//         {
//             new OrderData(orderId, adrianId, DateTime.UtcNow, 100.0)
//         };
//
//         _orderByUserIdQuery
//             .Setup(x => x.Fetch(adrianId))
//             .ReturnsAsync(orders);
//
//         _supplementDetailsRepository
//             .Setup(x => x.ByOrderId(orderId))
//             .ReturnsAsync(new List<OrderSupplementDetailsData>());
//
//         // Act
//         await _sut.Execute(input);
//
//         // Assert
//         _logger.Verify(
//             x => x.Log(
//                 LogLevel.Information,
//                 It.IsAny<EventId>(),
//                 It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Found 1 order(s)")),
//                 It.IsAny<Exception>(),
//                 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
//             Times.Once);
//     }
// }
