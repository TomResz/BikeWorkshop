using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;
public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task RevokeAsync(string refreshToken);
}
