using BikeWorkshop.Application.Functions.ClientDataFunctions.Commands.CreateClientData;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ClientDataFunctions.Commands;

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

        await handler.Handle(command, CancellationToken.None);

        _clientDataRepositoryMock.Verify(x => x.Add(It.IsAny<ClientData>()), Times.Once);
    }

}
