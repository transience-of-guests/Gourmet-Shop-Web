using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Table("ShoppingCart")]
public partial class ShoppingCart
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ShoppingCarts")]
    public virtual UserInfo UserInfo { get; set; } = null!;

    [InverseProperty("Cart")]
    public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; } = new List<ShoppingCartDetail>();
}
