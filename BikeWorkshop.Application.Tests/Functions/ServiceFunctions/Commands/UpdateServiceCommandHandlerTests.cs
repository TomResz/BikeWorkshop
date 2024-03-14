using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ServiceFunctions.Commands;
public class UpdateServiceCommandHandlerTests
{
	private readonly Mock<IServiceRepository> _repository;
	private readonly UpdateServiceCommand _command = new(Guid.NewGuid(),"name");

    private UpdateServiceCommandHandler _handler;
    public UpdateServiceCommandHandlerTests()
    {
        _repository = new Mock<IServiceRepository>();
    }

    [Fact]
    public async Task Handle_UnknownService_ThrowsNotFoundException()
    {
        _repository.Setup(x => x.GetById(_command.ServiceId))
            .ReturnsAsync(() => null);
        _handler = new(_repository.Object);

        var task = _handler.Handle(_command, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => task);
    }


	[Fact]
	public async Task Handle_NotUniqueServiceName_ThrowsBadRequestException()
	{
		_repository.Setup(x => x.GetById(_command.ServiceId))
			.ReturnsAsync(() => new());
        _repository.Setup(x => x.GetByName(_command.Name))
            .ReturnsAsync(() => new());

		_handler = new(_repository.Object);

		var task = _handler.Handle(_command, CancellationToken.None);

		await Assert.ThrowsAsync<BadRequestException>(() => task);
	}
	[Fact]
	public async Task Handle_ValidData_ThrowsBadRequestException()
	{
		var service = new Service() { Name = _command.Name };

		_repository.Setup(x => x.GetById(_command.ServiceId))
			.ReturnsAsync(() => service);
		_repository.Setup(x => x.GetByName(_command.Name))
			.ReturnsAsync(() => null);
		_handler = new(_repository.Object);

		await _handler.Handle(_command, CancellationToken.None);

		_repository.Verify(x => x.Update(service), Times.Once);
	}


}
