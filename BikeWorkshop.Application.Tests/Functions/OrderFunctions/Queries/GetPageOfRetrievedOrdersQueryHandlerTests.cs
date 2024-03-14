using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfRetrieved;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Pagination;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;
public class GetPageOfRetrievedOrdersQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
	private readonly GetPageOfRetrievedOrdersQuery _query;
	private readonly GetPageOfRetrievedOrdersQueryHandler _handler;
	private readonly List<Order> _orders = new()
	{
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(1)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(3)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(-1)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(-10)},
		new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Retrieved, AddedDate = DateTime.Now.AddSeconds(123)},
	};
	public GetPageOfRetrievedOrdersQueryHandlerTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
		_orderRepository.Setup(m => m.GetAllRetrieved())
			.ReturnsAsync(_orders);
        _query = new(1, 5);
        _handler = new(_orderRepository.Object);
    }

    [Fact]
	public async Task Handle_DescendingDirection_ShouldReturnOrdersInDescendingOrder()
	{
        var query  = _query with { SortingDirection = Application.Functions.DTO.Enums.SortingDirection.Descending};
		var paged = PagedList<Order>.Create(_orders, _query.Page, _query.PageSize);
		var response = await _handler.Handle(query, CancellationToken.None);

		response.Items.Should().NotBeNull();
		response.Items.Should().BeInDescendingOrder(x=>x.AddedDate);
		response.Items.Count().Should().BeLessOrEqualTo(query.PageSize);
		response.HasNextPage.Should().Be(paged.HasNextPage);
		response.HasPreviousPage.Should().Be(paged.HasPreviousPage);
	
	}

	[Fact]
	public async Task Handle_AscendingDirection_ShouldReturnOrdersInAscendingOrder()
	{
		var query = _query with { SortingDirection = Application.Functions.DTO.Enums.SortingDirection.Ascending };
		var paged = PagedList<Order>.Create(_orders, _query.Page, _query.PageSize);


		var response = await _handler.Handle(query, CancellationToken.None);

		response.Items.Should().NotBeNull();
		response.Items.Should().BeInAscendingOrder(x=>x.AddedDate);
		response.Items.Count().Should().BeLessOrEqualTo(query.PageSize);
		response.HasNextPage.Should().Be(paged.HasNextPage);
		response.HasPreviousPage.Should().Be(paged.HasPreviousPage);
	}
}
