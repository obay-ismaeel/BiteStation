using AutoMapper;
using BiteStation.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

namespace BiteStation.Presentation.Controllers;

public class CartController : BaseController
{
    public CartController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService) : base(unitOfWork, mapper, imageService)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Modify()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<IActionResult> Empty()
    {
        throw new NotImplementedException();
    }
}
