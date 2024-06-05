using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BiteStation.Domain.Dtos;

public class IncomingItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int MenuId { get; set; }
}
