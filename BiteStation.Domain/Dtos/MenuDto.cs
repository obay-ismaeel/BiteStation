﻿namespace BiteStation.Domain.Dtos;

public class MenuDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int RestaurantId { get; set; }
}
