using BiteStation.Domain.Models;

namespace BiteStation.Domain.Abstractions;
public interface IUnitOfWork
{
    IBaseRepository<User> Users { get; }
    IBaseRepository<Restaurant> Restaurants { get; }
    IBaseRepository<Menu> Menus { get; }
    IBaseRepository<Item> Items { get; }
    IBaseRepository<Order> Orders { get; }
    IBaseRepository<OrderItem> OrderItems { get; }
    IBaseRepository<Cart> Carts { get; }
    Task<int> CompleteAsync();
}
