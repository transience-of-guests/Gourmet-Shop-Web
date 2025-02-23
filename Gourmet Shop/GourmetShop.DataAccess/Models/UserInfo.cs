using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Table("UserInfo")]
[Microsoft.EntityFrameworkCore.Index("LastName", "FirstName", Name = "IndexUserName")]
public partial class UserInfo
{
    [Key]
    public int Id { get; set; }

    // public int RoleId { get; set; }

    /*
     private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // TODO: Authorize so that only admins can see this
        // GET: UsersController
        [HttpGet("customer/get/{userId}")]
        public async Task<IActionResult> Index()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Customer");

            // Get the IDs of users in the role as they are all strings
            List<string?> authIds = usersInRole.Select(u => u.Id).ToList();

            var users = await _userRepository.GetAllByRoleAsync(authIds);
            return View(users);
        }

        [HttpGet("admin/get/{userId}")]
        public async Task<IActionResult> Index()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Admin");

            // Get the IDs of users in the role as they are all strings
            List<string?> authIds = usersInRole.Select(u => u.Id).ToList();

            var users = await _userRepository.GetAllByRoleAsync(authIds);
            return View(users);
        }
     
     */

    [Required]
    public required string AuthenticationId { get; set; }
    [InverseProperty("UserInfo")]
    [ForeignKey("AuthenticationId")]
    public virtual required Authentication Authentication { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(40)]
    public string? City { get; set; }

    [StringLength(40)]
    public string? Country { get; set; }

    [InverseProperty("UserInfo")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("UserInfo")]
    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    /*[StringLength(20)]
    public string? Phone { get; set; }

    [InverseProperty("UserInfo")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("UserInfo")]
    public virtual ICollection<UserInfo> Users { get; set; } = new List<UserInfo>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;*/
}
