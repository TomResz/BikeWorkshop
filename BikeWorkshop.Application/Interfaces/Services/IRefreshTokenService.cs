using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Services;
public interface IRefreshTokenService
{
    RefreshToken GenerateRefreshToken(Guid EmployeeId,DateTime date);
    Guid? GetUserIdFromExpiredToken(string oldAccessToken);
}
