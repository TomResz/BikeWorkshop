using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfCurrents;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Pagination;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;
public class GetPageOfCurrentsOrdersQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
	private readonly List<Order> _orders = new()
	{
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.During, AddedDate = DateTime.Now.AddSeconds(1)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.During, AddedDate = DateTime.Now.AddSeconds(3)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.During, AddedDate = DateTime.Now.AddSeconds(-1)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.During, AddedDate = DateTime.Now.AddSeconds(-10)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.During, AddedDate = DateTime.Now.AddSeconds(123)},
	};
	private readonly GetPageOfCurrentsOrdersQuery _query = new(1, 5);
	private readonly GetPageOfCurrentsOrdersQueryHandler _handler;
	public GetPageOfCurrentsOrdersQueryHandlerTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
		_orderRepository.Setup(x => x.GetAllActive())
			.ReturnsAsync(() => _orders);
		_handler = new(_orderRepository.Object);
    }
	[Fact]
	public async Task Handle_ReturnsPageOfCurrentOrders()
	{
		var paged = PagedList<Order>.Create(_orders,_query.Page, _query.PageSize);
		var response = await _handler.Handle(_query, CancellationToken.None);

		response.Items.Count.Should().Be(_query.PageSize);
		response.HasNextPage.Should().Equals(paged.HasNextPage);
		response.HasPreviousPage.Should().Equals(paged.HasPreviousPage);
	}
}
