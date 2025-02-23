using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GourmetShop.DataAccess.Models;

[Table("Authentication")]
public partial class Authentication : IdentityUser
{
    [InverseProperty("Authentication")]
    public virtual required UserInfo UserInfo { get; set; }
}
