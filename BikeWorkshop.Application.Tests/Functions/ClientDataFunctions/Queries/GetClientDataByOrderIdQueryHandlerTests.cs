using BikeWorkshop.Application.Functions.ClientDataFunctions.Queries.Get;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ClientDataFunctions.Queries;
public class GetClientDataByOrderIdQueryHandlerTests
{
	private readonly Mock<IClientDataRepository> _clientDataRepository;
    private readonly GetClientDataByOrderIdQuery _query;
    private  GetClientDataByOrderIdQueryHandler _handler;
	public GetClientDataByOrderIdQueryHandlerTests()
    {
        _clientDataRepository = new Mock<IClientDataRepository>();
        _query = new(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_UnknownOrderId_ThrowsNotFoundException()
    {
        _clientDataRepository.Setup(x => x.GeByOrderId(_query.OrderId))
            .ReturnsAsync(() => null);
        _handler = new(_clientDataRepository.Object);

        var task = _handler.Handle(_query, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => task);
    }

    [Fact]
    public async Task Handle_ValidData_ReturnsClientData()
    {
        var clientData = new ClientData
        {
            PhoneNumber = "777 888 999",
            Email = "email@email.com"
        };
		_clientDataRepository.Setup(x => x.GeByOrderId(_query.OrderId))
			.ReturnsAsync(clientData);
        _handler = new(_clientDataRepository.Object);


        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().NotBeNull();
        result.PhoneNumber.Should().Be(clientData.PhoneNumber);
        result.Email.Should().Be(clientData.Email);

	}

}
