using AutoMapper;
using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Models;
using BiteStation.Infrastructure;
using BiteStation.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BiteStation.Presentation.Controllers;

public class RestaurantsController : BaseController
{

    public RestaurantsController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService) : base(unitOfWork, mapper, imageService)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
        var result = restaurants.Select(x => _mapper.Map<OutgoingRestaurantDto>(x));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);

        if (restaurant is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<OutgoingRestaurantDto>(restaurant);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(IncomingRestaurantDto restaurantDto)
    {
        restaurantDto.Id = 0;
        var restaurant = _mapper.Map<Restaurant>(restaurantDto);

        var path = await _imageService.StoreAsync(restaurantDto.ImageFile);

        if (path is not null)
        {
            restaurant.ImagePath = path;
        }

        await _unitOfWork.Restaurants.AddAsync(restaurant);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(Get), new { id = restaurant.Id }, _mapper.Map<OutgoingRestaurantDto>(restaurant));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, IncomingRestaurantDto restaurantDto)
    {
        if (id != restaurantDto.Id)
        {
            return BadRequest();
        }

        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);

        if (restaurant is null)
        {
            return NotFound();
        }

        restaurant.Name = restaurantDto.Name;
        restaurant.Description = restaurantDto.Description;
        restaurant.Location = restaurantDto.Location;
        restaurant.UpdatedAt = DateTime.UtcNow;

        if (restaurantDto.ImageFile is not null)
        {
            _imageService.Delete(restaurant.ImagePath);
            var path = await _imageService.StoreAsync(restaurantDto.ImageFile);
            restaurant.ImagePath = path;
        }

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);

        if (restaurant is null)
        {
            return NotFound();
        }

        _unitOfWork.Restaurants.Delete(restaurant);
        await _unitOfWork.CompleteAsync();
        _imageService.Delete(restaurant.ImagePath);

        return NoContent();
    }

    [HttpGet("{id}/menus")]
    public async Task<IActionResult> GetMenus(int id)
    {
        var restaurant = await _unitOfWork.Restaurants.FindAsync(x => x.Id == id, ["Menus"]);

        if (restaurant is null)
            return NotFound();

        var menus = restaurant.Menus;

        return Ok(menus.Select(x => _mapper.Map<MenuDto>(x)));
    }
}
