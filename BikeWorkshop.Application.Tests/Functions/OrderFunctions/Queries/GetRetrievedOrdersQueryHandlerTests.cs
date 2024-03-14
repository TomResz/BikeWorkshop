using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetRetrieved;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;
public class GetRetrievedOrdersQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
	private readonly GetRetrievedOrdersQuery _query;
	private readonly GetRetrievedOrdersQueryHandler _handler;
	private readonly List<Order> _orders = new()
	{
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(1)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(3)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(-1)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(-10)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(123)},
	};

    public GetRetrievedOrdersQueryHandlerTests()
    {
		_orderRepository = new Mock<IOrderRepository>();
		_orderRepository.Setup(x => x.GetAllRetrieved())
			.ReturnsAsync(_orders);
		_query = new(Application.Functions.DTO.Enums.SortingDirection.Ascending);
		_handler = new GetRetrievedOrdersQueryHandler(_orderRepository.Object);
    }

	[Fact]
	public async Task Handle_DescendingOrder_ReturnsOrdersInDescendingOrder()
	{
		var query = _query with { Direction = SortingDirection.Descending};

		var response = await _handler.Handle(query, CancellationToken.None);

		response.Should().NotBeNull();
		response.Should().BeInDescendingOrder(x=> x.AddedDate);
		response.Count.Should().Be(_orders.Count());
	}

	[Fact]
	public async Task Handle_AscendingOrder_ReturnsOrdersInAscendingOrder()
	{
		var query = _query with { Direction = SortingDirection.Ascending };

		var response = await _handler.Handle(query, CancellationToken.None);

		response.Should().NotBeNull();
		response.Should().BeInAscendingOrder(x => x.AddedDate);
		response.Count.Should().Be(_orders.Count());
	}
}
