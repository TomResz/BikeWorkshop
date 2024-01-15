using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Queries.GetByOrder;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Tests.Mocks;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Moq;
using System.Reflection.Metadata.Ecma335;

namespace BikeWorkshop.Application.Tests.Functions.ServiceToOrderFunctions.Queries;
public class GetByOrderQueryHandlerTests
{
	private readonly Mock<IServiceToOrderRepository> _serviceToOrderRepMock;
    private readonly Mock<IOrderRepository> _orderRepMock;
    private readonly GetServiceToOrderByOrderQuery _query;
    private readonly GetServiceToOrderByOrderQueryHandler _handler;
    private static Order _order;
    public GetByOrderQueryHandlerTests()
    {
        _serviceToOrderRepMock = ServiceToOrderRepositoryMock.GetMockedRepo();
        _query = new(Guid.NewGuid());
        _orderRepMock= GetOrderRepMock();
        _order = ServiceToOrderRepositoryMock.Order;
		_handler = new GetServiceToOrderByOrderQueryHandler(_serviceToOrderRepMock.Object,_orderRepMock.Object);
	}
    private static Mock<IOrderRepository> GetOrderRepMock()
    {
        var mock = new Mock<IOrderRepository>();
        mock.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => _order.Id == id ? _order : null);
        return mock;
    }

	[Fact]
    public async Task Handle_KnownOrderId_ReturnsListOfServiceOrders()
    {
        var orderId = _order.Id;
        var query = _query with { OrderId = orderId };
        var count = ServiceToOrderRepositoryMock.ServiceToOrders
            .Where(x=>x.OrderId == orderId)
            .ToList()
            .Count();

        var response = await _handler.Handle(query, default);

        Assert.True(count == response.Count);
    }

    [Fact]
    public async Task Handle_UnknownOrderId_ReturnsEmptyList()
    {
        var query = _query with { OrderId = Guid.NewGuid() };  
        var handleMethod =  _handler.Handle(query, default);
        await Assert.ThrowsAsync<NotFoundException>(() => handleMethod);
    }
}
