using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Models;
using BiteStation.Infrastructure.Repositories;

namespace BiteStation.Infrastructure.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IBaseRepository<User> Users { get; private set; }

    public IBaseRepository<Restaurant> Restaurants { get; private set; }

    public IBaseRepository<Menu> Menus { get; private set; }

    public IBaseRepository<Item> Items { get; private set; }

    public IBaseRepository<Order> Orders { get; private set; }

    public IBaseRepository<OrderItem> OrderItems { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new BaseRepository<User>(context);
        Restaurants = new BaseRepository<Restaurant>(context);
        Menus = new BaseRepository<Menu>(context);
        Items = new BaseRepository<Item>(context);
        Orders = new BaseRepository<Order>(context);
        OrderItems = new BaseRepository<OrderItem>(context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose() 
    {
        _context.Dispose();
    }   
}
