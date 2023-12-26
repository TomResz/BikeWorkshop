using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateClientData;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Commands;

public class CreateClientDataCommandHandlerTests
{
	private readonly Mock<IClientDataRepository> _clientDataRepositoryMock;

    public CreateClientDataCommandHandlerTests()
    {
        _clientDataRepositoryMock = new Mock<IClientDataRepository>();
    }

    [Fact]
    public async Task Handle_ValidCredentials_Should_AddOrder()
    {
        var command = new CreateClientDataCommand("tom@gmail.com", "+48777666555");
        var handler = new CreateClientDataCommandHandler(_clientDataRepositoryMock.Object);

        await handler.Handle(command,CancellationToken.None);

        _clientDataRepositoryMock.Verify(x => x.Add(It.IsAny<ClientData>()), Times.Once);
    }
    [Fact]
    public async Task Handle_InvalidData_ThrowsBadRequestException()
    {
		var command = new CreateClientDataCommand("", "+48777666555");
		var handler = new CreateClientDataCommandHandler(_clientDataRepositoryMock.Object);

		var task = handler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<BadRequestException>(()=>  task);
	}

}
