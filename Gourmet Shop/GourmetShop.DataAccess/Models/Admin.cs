using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Table("Admin")]
[Microsoft.EntityFrameworkCore.Index("UserId", Name = "IndexAdminUserId")]
public partial class Admin
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [StringLength(320)]
    public string? Email { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Admins")]
    public virtual User User { get; set; } = null!;
}
