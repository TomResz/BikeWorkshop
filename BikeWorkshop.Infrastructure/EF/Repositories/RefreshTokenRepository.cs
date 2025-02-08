using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.Infrastructure.EF.Repositories;
internal sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly BikeWorkshopDbContext _dbContext;

    public RefreshTokenRepository(BikeWorkshopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(default);
    }

    public async Task RevokeAsync(string refreshToken) 
        => await _dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken)
            .ExecuteDeleteAsync();
}
