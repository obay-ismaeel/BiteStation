using AutoMapper;
using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Models;
using BiteStation.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BiteStation.Presentation.Controllers;

public class ItemsController : BaseController
{
    public ItemsController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService) : base(unitOfWork, mapper, imageService)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _unitOfWork.Items.GetAllAsync();
        var result = items.Select(x => _mapper.Map<OutgoingItemDto>(x));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<OutgoingItemDto>(item);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(IncomingItemDto itemDto)
    {
        itemDto.Id = 0;
        var item = _mapper.Map<Item>(itemDto);

        var path = await _imageService.StoreAsync(itemDto.ImageFile);

        if (path is not null)
        {
            item.ImagePath = path;
        }

        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(Get), new { id = item.Id }, _mapper.Map<OutgoingItemDto>(item));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, IncomingItemDto itemDto)
    {
        if (id != itemDto.Id)
        {
            return BadRequest();
        }

        var item = await _unitOfWork.Items.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        var menu = await _unitOfWork.Menus.GetByIdAsync(itemDto.MenuId);

        if (menu is null)
        {
            return BadRequest("There is no such menu");
        }

        
        item.Name = itemDto.Name;
        item.Description = itemDto.Description;
        item.Price = itemDto.Price;
        item.MenuId = itemDto.MenuId;
        item.UpdatedAt = DateTime.UtcNow;

        if (itemDto.ImageFile is not null)
        {
            _imageService.Delete(item.ImagePath);
            var path = await _imageService.StoreAsync(itemDto.ImageFile);
            item.ImagePath = path;
        }

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        _unitOfWork.Items.Delete(item);
        await _unitOfWork.CompleteAsync();
        _imageService.Delete(item.ImagePath);

        return NoContent();
    }
}
