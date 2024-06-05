using AutoMapper;
using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Dtos;
using BiteStation.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BiteStation.Presentation.Controllers;

public class MenusController : BaseController
{
    public MenusController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService) : base(unitOfWork, mapper, imageService)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var menus = await _unitOfWork.Menus.GetAllAsync();
        var result = menus.Select(x => _mapper.Map<MenuDto>(x));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var menu = await _unitOfWork.Menus.GetByIdAsync(id);

        if (menu is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<MenuDto>(menu);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(MenuDto menuDto)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(menuDto.RestaurantId);

        if(restaurant is null)
        {
            return BadRequest("No such restaurant!");
        }

        menuDto.Id = 0;
        var menu = await _unitOfWork.Menus.AddAsync( _mapper.Map<Menu>(menuDto) );
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(Get), new { id = menu.Id }, _mapper.Map<MenuDto>(menu));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, MenuDto menuDto)
    {
        if (id != menuDto.Id )
        {
            return BadRequest();
        }

        var menu = await _unitOfWork.Menus.GetByIdAsync(id);

        if (menu is null)
        {
            return NotFound();
        }

        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(menuDto.RestaurantId);

        if (restaurant is null)
        {
            return BadRequest("No such restaurant!");
        }

        menu.Name = menuDto.Name;
        menu.Description = menuDto.Description;
        menu.RestaurantId = menuDto.RestaurantId;
        menu.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var menu = await _unitOfWork.Menus.GetByIdAsync(id);

        if (menu is null)
        {
            return NotFound();
        }

        _unitOfWork.Menus.Delete(menu);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetMenus(int id)
    {
        var menu = await _unitOfWork.Menus.FindAsync(x => x.Id == id, ["Items"]);

        if (menu is null)
            return NotFound();

        var menus = menu.Items;

        return Ok(menus.Select(x => _mapper.Map<OutgoingItemDto>(x)));
    }
}
