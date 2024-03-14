using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetails;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.SummaryFunctions.Queries;
public class GetSummaryWithDetailsQueryHandlerTests
{
	private readonly Mock<ISummaryRepository> _summaryRepository;
	private readonly Mock<IServiceToOrderRepository> _serviceToOrderRepository;

	private readonly List<ServiceToOrder> serviceToOrders = new()
	{
		new(){Count = 0 ,Price = 12m,Service = new()},
		new(){Count = 0 ,Price = 1m ,Service = new()},
		new(){Count = 0 ,Price = 1m, Service = new()},
	};



	private readonly GetSummaryWithDetailsQuery _query = new(Guid.NewGuid());
    private GetSummaryWithDetailsQueryHandler _handler;
	public GetSummaryWithDetailsQueryHandlerTests()
    {
        _summaryRepository = new Mock<ISummaryRepository>();
        _serviceToOrderRepository = new Mock<IServiceToOrderRepository>();
    }

    [Fact]
    public async Task Handle_UnknownSummary_ThrowsNotFoundException()
    {
        _summaryRepository.Setup(x => x.GetByOrderId(_query.OrderId))
            .ReturnsAsync(() => null);
        _handler = new(_serviceToOrderRepository.Object, _summaryRepository.Object);
        var task = _handler.Handle(_query, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => task);
    }

    [Fact]
    public async Task Handle_ReturnsSummaryWitDetails()
    {
        var summary = new Summary
        {
            EndedDate = DateTime.Now,
            Conclusion = "conclusions",
        };
		_summaryRepository.Setup(x => x.GetByOrderId(_query.OrderId))
	        .ReturnsAsync(() => summary);
        _serviceToOrderRepository.Setup(x => x.GetServiceDetailsByOrderId(_query.OrderId))
            .ReturnsAsync(() => serviceToOrders);
        _handler = new(_serviceToOrderRepository.Object, _summaryRepository.Object);
        var response = await _handler.Handle(_query, CancellationToken.None);
      
        Assert.NotNull(response); 
        response.ServiceDetails.Count.Should().Be(serviceToOrders.Count);
        response.EndedDate.Should().Be(summary.EndedDate);
	}
}
