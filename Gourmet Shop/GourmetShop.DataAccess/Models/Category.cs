using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Microsoft.EntityFrameworkCore.Index("Name", Name = "UQ__Categori__737584F65623B0CE", IsUnique = true)]
public partial class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}
