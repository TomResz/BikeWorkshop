using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GePageOfCompleted;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;
public class GetPageOfCompletedOrdersQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
    private readonly List<Order> _orders = new()
    {
        new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Completed, AddedDate = DateTime.Now.AddSeconds(1)},
        new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Completed, AddedDate = DateTime.Now.AddSeconds(3)},
        new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Completed, AddedDate = DateTime.Now.AddSeconds(-1)},
        new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Completed, AddedDate = DateTime.Now.AddSeconds(-10)},
        new Order { Id = Guid.NewGuid(), OrderStatusId = (int)Status.Completed, AddedDate = DateTime.Now.AddSeconds(123)},
    };
    private readonly GetPageOfCompletedQueryHandler _handler;
    private readonly GetPageOfCompletedQuery _query = new(1,5);

    public GetPageOfCompletedOrdersQueryHandlerTests()
    { 
        _orderRepository = new();
        _orderRepository.Setup(x => x.GetAllCompleted())
            .ReturnsAsync(() => _orders);
        _handler = new(_orderRepository.Object);
    }
    
   
    [Fact]
    public async Task Handle_DescendingDirection_ShouldReturnOrdersInDescendingOrder()
    {
        var query = _query with { SortingDirection = Application.Functions.DTO.Enums.SortingDirection.Descending };

        var response = await _handler.Handle(query, CancellationToken.None);

        response.Items.Should().BeInDescendingOrder(x => x.AddedDate);
        response.Items.Should().NotBeInAscendingOrder(x => x.AddedDate);
        response.Items.Count.Should().Be(_query.PageSize);
    }

	[Fact]
	public async Task Handle_AscendingDirection_ShouldReturnOrdersInAscendingOrder()
	{
		var query = _query with { SortingDirection = Application.Functions.DTO.Enums.SortingDirection.Ascending };

		var response = await _handler.Handle(query, CancellationToken.None);

		response.Items.Should().BeInAscendingOrder(x => x.AddedDate);
		response.Items.Should().NotBeInDescendingOrder(x => x.AddedDate);
		response.Items.Count.Should().Be(_query.PageSize);
	}
}
