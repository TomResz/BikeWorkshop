using BikeWorkshop.Application.Functions.ServiceFunctions.Queries.GetAll;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ServiceFunctions.Queries;
public class GetAllServiceQueryHandlerTests
{
	private readonly Mock<IServiceRepository> _serviceRepository;
    private readonly GetAllServicesQuery _query;
    private readonly GetAllServiceQueryHandler _handler;
    private readonly List<Service> services = new()
    {
        new Service { Id = Guid.NewGuid(),Name = "Service 1",},
        new Service { Id = Guid.NewGuid(),Name = "Service 2",},
        new Service { Id = Guid.NewGuid(),Name = "Service 3",},
        new Service { Id = Guid.NewGuid(),Name = "Service 4",}
    };

    public GetAllServiceQueryHandlerTests()
    {
		_query = new();
		_serviceRepository = new Mock<IServiceRepository>();
        _serviceRepository.Setup(x=> x.GetAll())
            .ReturnsAsync(services);
        _handler = new(_serviceRepository.Object);
    }

    [Fact]
    public async Task Handle_DescendingOrder_ShouldReturnServicesInDescendingOrder()
    {
        var query = _query with { SortingDirection = Application.Functions.DTO.Enums.SortingDirection.Descending };

        var response = await _handler.Handle(query, CancellationToken.None);

        
        response.Should().BeInDescendingOrder(x=> x.Name);        
        Assert.True(services.Count() == response.Count());
    }

	[Fact]
	public async Task Handle_AscendingOrder_ShouldReturnServicesInAscendingOrder()
	{
		var query = _query with { SortingDirection = Application.Functions.DTO.Enums.SortingDirection.Ascending };

		var response = await _handler.Handle(query, CancellationToken.None);


		response.Should().BeInAscendingOrder(x => x.Name);
		Assert.True(services.Count() == response.Count());
	}
}
