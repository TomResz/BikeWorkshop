using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetailsByShortId;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.SummaryFunctions.Queries;
public class GetSummaryWithDetailsByShortIdQueryHandlerTests
{
	private readonly Mock<ISummaryRepository> _summaryRepository;
	private readonly Mock<IServiceToOrderRepository> _serviceToOrderRepository;

	private readonly List<ServiceToOrder> serviceToOrders = new()
	{
		new(){Count = 0 ,Price = 12m,Service = new()},
		new(){Count = 0 ,Price = 1m ,Service = new()},
		new(){Count = 0 ,Price = 1m, Service = new()},
	};

	private readonly GetSummaryWithDetailsByShortIdQuery _query = new("short-id");
	private GetSummaryWithDetailsByShortIdQueryHandler _handler;


	public GetSummaryWithDetailsByShortIdQueryHandlerTests()
    {
        _summaryRepository = new Mock<ISummaryRepository>();
        _serviceToOrderRepository = new Mock<IServiceToOrderRepository>();
    }

    [Fact]
    public async Task Handle_UnknownSummary_ThrowsNotFoundException()
	{
		_summaryRepository.Setup(x => x.GetByShortUniquerId(_query.ShortId))
			.ReturnsAsync(() => null);
		_handler = new(_summaryRepository.Object, _serviceToOrderRepository.Object);
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
		_summaryRepository.Setup(x => x.GetByShortUniquerId(_query.ShortId))
			.ReturnsAsync(() => summary);
		_serviceToOrderRepository.Setup(x => x.GetServiceDetailsByShortId(_query.ShortId))
			.ReturnsAsync(() => serviceToOrders);
		_handler = new(_summaryRepository.Object, _serviceToOrderRepository.Object);
		var response = await _handler.Handle(_query, CancellationToken.None);

		Assert.NotNull(response);
		response.ServiceDetails.Count.Should().Be(serviceToOrders.Count);
		response.EndedDate.Should().Be(summary.EndedDate);
	}
}
