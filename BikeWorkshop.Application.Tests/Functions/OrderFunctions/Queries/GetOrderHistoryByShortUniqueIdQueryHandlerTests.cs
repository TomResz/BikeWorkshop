using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetOrderHistoryByShortUniqueId;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Queries;
public class GetOrderHistoryByShortUniqueIdQueryHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
    private readonly GetOrderHistoryByShortUniqueIdQuery _query = new("short-id");
    private GetOrderHistoryByShortUniqueIdQueryHandler _handler;
	public GetOrderHistoryByShortUniqueIdQueryHandlerTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
    }


    [Fact]
    public async Task Handle_UnknownOrder_ThrowsNotFoundException()
    {
        _orderRepository.Setup(x => x.GetByShortId(_query.ShortUniqueId))
            .ReturnsAsync(() => null);
        _handler = new(_orderRepository.Object);

        var task = _handler.Handle(_query, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => task);
    }

    [Fact]
    public async Task Handle_CurrentOrder_ReturnsHistory()
    {
        var order = new Order()
        {
            AddedDate = DateTime.Now,
            OrderStatusId = (int)Status.During,
            ShortUniqueId = _query.ShortUniqueId
        };

        _orderRepository.Setup( x=> x.GetByShortId(_query.ShortUniqueId))
            .ReturnsAsync(order);

        _handler = new(_orderRepository.Object);

        var response = await _handler.Handle(_query, CancellationToken.None);

        response.Should().NotBeNull();
        response.StatusHistoryDtos.Count().Should().Be(1);

    }

	[Fact]
	public async Task Handle_CompletedOrder_ReturnsHistory()
    {
		var order = new Order()
		{
			AddedDate = DateTime.Now,
			OrderStatusId = (int)Status.Completed,
			ShortUniqueId = _query.ShortUniqueId,
            Summary = new()
            {
                EndedDate = DateTime.Now,
            }
		};

		_orderRepository.Setup(x => x.GetByShortId(_query.ShortUniqueId))
			.ReturnsAsync(order);

		_handler = new(_orderRepository.Object);

		var response = await _handler.Handle(_query, CancellationToken.None);

		response.Should().NotBeNull();
		response.StatusHistoryDtos.Count().Should().Be(2);
	}

	[Fact]
	public async Task Handle_RetrievedOrder_ReturnsHistory()
    {
		var order = new Order()
		{
			AddedDate = DateTime.Now,
			OrderStatusId = (int)Status.Retrieved,
			ShortUniqueId = _query.ShortUniqueId,
			Summary = new()
			{
				EndedDate = DateTime.Now,
				RetrievedDate = DateTime.Now,
			}
		};

		_orderRepository.Setup(x => x.GetByShortId(_query.ShortUniqueId))
			.ReturnsAsync(order);

		_handler = new(_orderRepository.Object);

		var response = await _handler.Handle(_query, CancellationToken.None);

		response.Should().NotBeNull();
		response.StatusHistoryDtos.Count().Should().Be(3);
	}
}
