using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityApp.Models;
using IdentityApp.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    /// <summary>
    /// Retrieves all registered users
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")] // Optional: restrict to admin users
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    /// <summary>
    /// Retrieves the current user's shopping cart (PanierParUser)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserCart()
    {
        var userId = _userManager.GetUserId(User);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var cartItems = await _context.PanierParUsers
            .Where(p => p.UserId == userId)
            .Include(p => p.Produit)
            .ToListAsync();

        return View(cartItems);
    }

    /// <summary>
    /// Retrieves a user's profile information
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = _userManager.GetUserId(User);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    /// <summary>
    /// Updates the current user's profile
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ApplicationUser model)
    {
        var userId = _userManager.GetUserId(User);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        user.City = model.City;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Profile));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(nameof(Profile), user);
    }

    /// <summary>
    /// Adds a product to the current user's cart
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int produitId, int quantity = 1)
    {
        var userId = _userManager.GetUserId(User);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var produit = await _context.Produits.FindAsync(produitId);
        
        if (produit == null)
        {
            return NotFound("Product not found");
        }

        var cartItem = await _context.PanierParUsers
            .FirstOrDefaultAsync(p => p.UserId == userId && p.ProduitId == produitId);

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new PanierParUser
            {
                UserId = userId,
                ProduitId = produitId,
                Quantity = quantity
            };
            _context.PanierParUsers.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(GetUserCart));
    }

    /// <summary>
    /// Removes a product from the current user's cart
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        var cartItem = await _context.PanierParUsers.FindAsync(cartItemId);
        
        if (cartItem == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        
        if (cartItem.UserId != userId)
        {
            return Unauthorized();
        }

        _context.PanierParUsers.Remove(cartItem);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(GetUserCart));
    }
}
