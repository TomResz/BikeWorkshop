using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetActual;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Tests.Mocks;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;

public class GetCurrentOrdersQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
	private readonly GetCurrentOrdersQuery _query;
	private readonly GetCurrentOrdersQueryHandler _handler;
	public GetCurrentOrdersQueryHandlerTests()
	{
		_orderRepository = OrderRepositoryMock.GetOrderRepositoryMock();
		_query = new(SortingDirection.Ascending);
		_handler = new GetCurrentOrdersQueryHandler(_orderRepository.Object);
	}

	[Fact]
	public async Task Handle_ShouldReturnAllActiveOrdersSortedAscending()
	{
		var query = _query with { Direction = SortingDirection.Ascending };
		var orders = await _orderRepository.Object.GetAllActive();
		var sortedOrders = orders.OrderBy(x=>x.AddedDate).ToList();

		var result = await _handler.Handle(query, CancellationToken.None);

		Assert.Equal(result.Count ,orders.Count);
		for(int i=0; i <result.Count; i++)
		{
			Assert.Equal(result[i].AddedDate, sortedOrders[i].AddedDate);
		}
	}

	[Fact]
	public async Task Handle_ShouldReturnAllActiveOrdersSortedDescending()
	{
		var query = _query with { Direction = SortingDirection.Descending };
		var orders = await _orderRepository.Object.GetAllActive();
		var sortedOrders = orders.OrderByDescending(x => x.AddedDate).ToList();

		var result = await _handler.Handle(query, CancellationToken.None);

		Assert.Equal(result.Count, orders.Count);
		for (int i = 0; i < result.Count; i++)
		{
			Assert.Equal(result[i].AddedDate, sortedOrders[i].AddedDate);
		}
	}
}
