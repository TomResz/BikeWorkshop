using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using Moq;

namespace BikeWorkshop.Application.Tests.Mocks;

internal class EmployeeRepositoryMock
{
	public static Mock<IEmployeeRepository> GetRepository()
	{
		var employeeList= GetEmployees();
		var mock = new Mock<IEmployeeRepository>();
		mock.Setup(x=>x.GetAll()).ReturnsAsync(GetEmployees);
		mock.Setup(f => f.GetByEmail(It.IsAny<string>()))
			.ReturnsAsync((string? email) =>
			{
				var employee = employeeList.FirstOrDefault(p => p.Email == email);
				return employee;
			});
		mock.Setup(f => f.GetById(It.IsAny<Guid>())).ReturnsAsync(
			(Guid id) =>
			{
				var employee = employeeList.FirstOrDefault(x => x.Id == id);
				return employee;
			});
		mock.Setup(x => x.Update(It.IsAny<Employee>())).Callback<Employee>(
			(entity) =>
			{
				employeeList.Remove(entity);
				employeeList.Add(entity);
			});
		mock.Setup(x => x.Register(It.IsAny<Employee>())).Callback<Employee>(
			(entity) =>
			{
				employeeList.Add(entity);
			});
		return mock;
	}

	private static List<Employee> GetEmployees()
	{
		var list =  new List<Employee>
			{
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "John",
					LastName = "Doe",
					Email = "john.doe@example.com",
					PasswordHash = "hashed_password_1",
					RoleId = (int) Roles.Manager

				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Jane",
					LastName = "Smith",
					Email = "jane.smith@example.com",
					PasswordHash = "hashed_password_2",
					RoleId = (int) Roles.Worker
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Alice",
					LastName = "Johnson",
					Email = "alice.johnson@example.com",
					PasswordHash = "hashed_password_3",
					RoleId = (int)Roles.Manager,

				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Bob",
					LastName = "Miller",
					Email = "bob.miller@example.com",
					PasswordHash = "hashed_password_4",
					RoleId = (int)Roles.Worker
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Eva",
					LastName = "Williams",
					Email = "eva.williams@example.com",
					PasswordHash = "hashed_password_5",
					RoleId = (int)Roles.Worker
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Alex",
					LastName = "Brown",
					Email = "alex.brown@example.com",
					PasswordHash = "hashed_password_6",
					RoleId = (int)Roles.Worker
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "David",
					LastName = "Jones",
					Email = "david.jones@example.com",
					PasswordHash = "hashed_password_7",
					RoleId = (int)Roles.Manager
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Sara",
					LastName = "White",
					Email = "sara.white@example.com",
					PasswordHash = "hashed_password_8",
					RoleId = (int)Roles.Manager
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Chris",
					LastName = "Taylor",
					Email = "chris.taylor@example.com",
					PasswordHash = "hashed_password_9",
					RoleId = (int)Roles.Manager
				},
				new Employee
				{
					Id = Guid.NewGuid(),
					FirstName = "Olivia",
					LastName = "Clark",
					Email = "olivia.clark@example.com",
					PasswordHash = "hashed_password_10",
					RoleId = (int)Roles.Manager
				}
			};
		foreach(var employee in list)
		{
			if(employee.RoleId == (int)Roles.Worker) {
				employee.Role = new Role
				{
					Id = (int)Roles.Worker,
					Name = "Worker"
				};
			}
			else
			{
				employee.Role = new Role
				{
					Id = (int)Roles.Manager,
					Name = "Manager"
				};
			}
		}
		return list;
	}
}
