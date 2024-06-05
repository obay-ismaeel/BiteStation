using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Dtos;
using BiteStation.Domain.Models;
using BiteStation.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Infrastructure.Services;
public class CartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public CartService(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Modify(IncomingCartDto cartItems, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var user = _unitOfWork.Users.GetById(userId);
    }

    public async Task GetAll(string userId)
    {
        var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId, ["Order", "OrderItems"]);

    }

    public async Task Purchase(string userId)
    {
        var user = _userManager.FindByIdAsync(userId);

    }

    public async Task Empty()
    {
        throw new NotImplementedException();
    }
}
