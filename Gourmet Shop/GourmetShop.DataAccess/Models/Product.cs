using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Table("Product")]
[Microsoft.EntityFrameworkCore.Index("ProductName", Name = "IndexProductName")]
[Microsoft.EntityFrameworkCore.Index("SupplierId", Name = "IndexProductSupplierId")]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    public int? SubcategoryId { get; set; }

    public int SupplierId { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal? UnitPrice { get; set; }

    [StringLength(30)]
    public string? Package { get; set; }

    public bool IsDiscontinued { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Product")]
    public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; } = new List<ShoppingCartDetail>();

    [ForeignKey("SubcategoryId")]
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Subcategory? Subcategory { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Supplier Supplier { get; set; } = null!;
}
