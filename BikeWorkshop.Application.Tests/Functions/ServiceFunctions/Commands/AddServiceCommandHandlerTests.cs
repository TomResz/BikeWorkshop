using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ServiceFunctions.Commands;
public class AddServiceCommandHandlerTests
{
	private readonly Mock<IServiceRepository> _serviceRepository;
    public AddServiceCommandHandlerTests()
    {
        _serviceRepository = new Mock<IServiceRepository>();
    }

    [Fact]
    public async Task Handle_UniqueServiceName_ShouldAddsService()
    {
		var serviceCommand = new AddServiceCommand("Unique");
		_serviceRepository.Setup(x => x.Add(It.IsAny<Service>()))
            .ReturnsAsync(() => true);
        var commandHandler = new AddServiceCommandHandler(_serviceRepository.Object);

        var response = await commandHandler.Handle(serviceCommand, CancellationToken.None);
        
        response.IsAdded.Should().BeTrue();
    }

	[Fact]
	public async Task Handle_NotUniqueName_ShouldNotAdd()
	{
		var service = new Service() { Name = "NotUnique", Id = Guid.NewGuid() };
		_serviceRepository.Setup(x => x.Add(service))
			.ReturnsAsync(() => false);
		var commandHandler = new AddServiceCommandHandler(_serviceRepository.Object);

		var serviceCommand = new AddServiceCommand(service.Name);
		var response = await commandHandler.Handle(serviceCommand, CancellationToken.None);

		response.Feedback.Should().NotBeNull();
		response.IsAdded.Should().BeFalse();
	}
}
