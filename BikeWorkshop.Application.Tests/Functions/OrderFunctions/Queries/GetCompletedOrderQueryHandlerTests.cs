using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetCompleted;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Tests.Mocks;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;

public class GetCompletedOrderQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
	private readonly GetCompletedOrderQuery _query;
	private readonly GetCompletedOrdersQueryHandler _handler;
    public GetCompletedOrderQueryHandlerTests()
    {
        _orderRepository = OrderRepositoryMock.GetOrderRepositoryMock();
        _query = new(SortingDirection.Ascending);
        _handler = new GetCompletedOrdersQueryHandler(_orderRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllActiveOrdersSortedAscending()
    {
        var query = _query with { Direction = SortingDirection.Ascending };
        var ordersSorted = (await _orderRepository.Object.GetAllCompleted())
            .OrderBy(x => x.AddedDate)
            .ToList();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(result.Count, ordersSorted.Count);

        for(int i = 0; i < result.Count; ++i)
        {
            Assert.Equal(result[i].AddedDate, ordersSorted[i].AddedDate);
        }
    }
	[Fact]
	public async Task Handle_ShouldReturnAllActiveOrdersSortedDescending()
	{
		var query = _query with { Direction = SortingDirection.Descending };
		var ordersSorted = (await _orderRepository.Object.GetAllCompleted())
			.OrderByDescending(x => x.AddedDate)
			.ToList();

		var result = await _handler.Handle(query, CancellationToken.None);

		Assert.Equal(result.Count, ordersSorted.Count);

		for (int i = 0; i < result.Count; ++i)
		{
			Assert.Equal(result[i].AddedDate, ordersSorted[i].AddedDate);
		}
	}
}
