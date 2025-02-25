using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Table("Order")]
[Microsoft.EntityFrameworkCore.Index("UserId", Name = "IndexOrderCustomerId")]
[Microsoft.EntityFrameworkCore.Index("OrderDate", Name = "IndexOrderOrderDate")]
public partial class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [StringLength(10)]
    public string? OrderNumber { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal? TotalAmount { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual UserInfo UserInfo { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
