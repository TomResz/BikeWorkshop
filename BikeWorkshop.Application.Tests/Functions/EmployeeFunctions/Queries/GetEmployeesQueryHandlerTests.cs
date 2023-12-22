using BikeWorkshop.Application.Functions.EmployeeFunctions.Queries.GetEmployees;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Tests.Mocks;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.EmployeeFunctions.Queries;

public class GetEmployeesQueryHandlerTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    public GetEmployeesQueryHandlerTests()
    {
        _employeeRepositoryMock = EmployeeRepositoryMock.GetRepository();
    }
    [Fact]
    public async Task Handle_Should_GetAllEmployees()
    {
        var command = new GetEmployeesQuery();
        var handler = new GetEmployeesQueryHandler(_employeeRepositoryMock.Object);
        var exceptedCount = (await _employeeRepositoryMock.Object.GetAll()).Count();

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(response.Count, exceptedCount);
    }
}
