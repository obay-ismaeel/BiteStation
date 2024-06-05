namespace BiteStation.Domain.Dtos;

public class OutgoingRestaurantDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ImagePath { get; set; }
}
