using BiteStation.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BiteStation.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    public readonly IUnitOfWork _unitOfWork;
    public BaseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
