﻿namespace BiteStation.Presentation.Dtos;

public class OutgoingItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath{ get; set; }
    public int MenuId { get; set; }
}
