using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, JwtDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(
        IEmployeeRepository employeeRepository,
        IRefreshTokenService refreshTokenService,
        IJwtService jwtService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _employeeRepository = employeeRepository;
        _refreshTokenService = refreshTokenService;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<JwtDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var id = _refreshTokenService.GetUserIdFromExpiredToken(request.AccessToken);
        
        if (id is null)
        {
            throw new NotFoundException("Invalid access token");    
        }

        var employee = await _employeeRepository.GetByIdAndRefreshTokenAsync(
            (Guid)id,request.RefreshToken);

        if (employee is null)
        {
            throw new NotFoundException("User not found.");
        }

        var accessToken = _jwtService.GetToken(employee);
        var refreshToken = _refreshTokenService.GenerateRefreshToken(employee.Id,DateTime.UtcNow);

        await _refreshTokenRepository.RevokeAsync(request.RefreshToken);
        await _refreshTokenRepository.AddAsync(refreshToken);
        return new JwtDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
        };

    }
}
