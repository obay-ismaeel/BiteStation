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
    public async Task<IActionResult> GetAllAsync()
    {
        var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
        var result = restaurants.Select(x => _mapper.Map<OutgoingRestaurantDto>(x));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);

        if(restaurant is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<OutgoingRestaurantDto>(restaurant);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(IncomingRestaurantDto restaurantDto)
    {
        restaurantDto.Id = 0;
        var restaurantToPresist = _mapper.Map<Restaurant>(restaurantDto);
        
        var path = await _imageService.StoreAsync(restaurantDto.ImageFile);

        if(path is not null)
        {
            restaurantToPresist.ImagePath = path;
        }

        var restaurant = await _unitOfWork.Restaurants.AddAsync(restaurantToPresist);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetAsync), new { id = restaurant.Id }, _mapper.Map<OutgoingRestaurantDto>(restaurant) );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, IncomingRestaurantDto restaurantDto)
    {
        if(id != restaurantDto.Id)
        {
            return BadRequest();
        }

        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);

        if(restaurant is null)
        {
            return NotFound();
        }

        restaurant.Name = restaurantDto.Name;
        restaurant.Description = restaurantDto.Description;
        restaurant.Location = restaurantDto.Location;
        restaurant.UpdatedAt = DateTime.UtcNow;

        if(restaurantDto.ImageFile is not null)
        {
            _imageService.Delete(restaurant.ImagePath);
            var path = await _imageService.StoreAsync(restaurantDto.ImageFile);
            restaurant.ImagePath = path;
        }

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);

        if(restaurant is null)
        {
            return NotFound();
        }

        _unitOfWork.Restaurants.Delete(restaurant);
        await _unitOfWork.CompleteAsync();
        _imageService.Delete(restaurant.ImagePath);

        return NoContent();
    }
}
