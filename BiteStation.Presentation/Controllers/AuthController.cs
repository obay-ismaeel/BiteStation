using AutoMapper;
using BiteStation.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BiteStation.Presentation.Controllers;

public class AuthController : BaseController
{
    public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService) : base(unitOfWork, mapper, imageService)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        throw new NotImplementedException();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        throw new NotImplementedException();
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword()
    {
        throw new NotImplementedException();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail()
    {
        throw new NotImplementedException();
    }
}
