using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

public partial class ShoppingCartDetail
{
    [Key]
    public int Id { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Price { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("ShoppingCartDetails")]
    public virtual ShoppingCart Cart { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ShoppingCartDetails")]
    public virtual Product Product { get; set; } = null!;
}
