using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Domain.Enums;
using FluentAssertions;
using Newtonsoft.Json;

namespace BikeWorkshop.API.Tests.Employee.Register;
public class RegisterByWorkerTests
    : WorkerBaseClass
{
    private const string _path = "api/employee/register";

    public RegisterByWorkerTests(WorkerTestWebApplicationFactory<Program> workerFactory) : base(workerFactory)
    {
    }

    [Fact]
    public async Task Register_WorkerTriesRegisterNewAccount_ReturnsForbiddenStatus()
    {
        var command = new RegisterEmployeeCommand("John", "Young",
            "john@johny.com", "12345678", "12345678", (int)Roles.Worker);

        var json = JsonConvert.SerializeObject(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }
}
