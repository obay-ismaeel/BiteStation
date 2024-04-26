using AutoMapper;
using BiteStation.Domain.Abstractions;
using BiteStation.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BiteStation.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting("fixed")]
public class BaseController : ControllerBase
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;
    public readonly IImageService _imageService;
    public BaseController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageService = imageService;
    }
}
