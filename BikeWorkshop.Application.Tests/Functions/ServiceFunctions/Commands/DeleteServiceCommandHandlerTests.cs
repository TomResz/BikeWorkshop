using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Delete;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ServiceFunctions.Commands;
public class DeleteServiceCommandHandlerTests
{
	private readonly Mock<IServiceRepository> _serviceRepository;
    private readonly DeleteServiceCommand _command = new(Guid.NewGuid());

    public DeleteServiceCommandHandlerTests()
    {
        _serviceRepository = new Mock<IServiceRepository>();
    }

    [Fact]
    public async Task Handle_UnknownService_ThrowsNotFoundException()
    {
        _serviceRepository.Setup(x=>x.GetById(_command.ServiceId))
            .ReturnsAsync(() => null);
        var handler = new DeleteServiceCommandHandler(_serviceRepository.Object);

        var task = handler.Handle(_command, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => task);
    }

	[Fact]
	public async Task Handle_RelatedServices_ThrowsBadRequestException()
	{
		_serviceRepository.Setup(x => x.GetById(_command.ServiceId))
	        .ReturnsAsync(() => new()
            {
                ServiceToOrders = new List<ServiceToOrder>()
                {
                    new ServiceToOrder() { Id = Guid.NewGuid() },
                }
            });
		var handler = new DeleteServiceCommandHandler(_serviceRepository.Object);

		var task = handler.Handle(_command, CancellationToken.None);
		await Assert.ThrowsAsync<BadRequestException>(() => task);
	}

	[Fact]
	public async Task Handle_ValidData_ShouldDeleteService()
	{
        var service = new Service() { ServiceToOrders = new List<ServiceToOrder>()};
		_serviceRepository.Setup(x => x.GetById(_command.ServiceId))
			.ReturnsAsync(() => service);
		var handler = new DeleteServiceCommandHandler(_serviceRepository.Object);

		await handler.Handle(_command, CancellationToken.None);

        _serviceRepository.Verify(x=>x.Delete(service),Times.Once);
	}
}
