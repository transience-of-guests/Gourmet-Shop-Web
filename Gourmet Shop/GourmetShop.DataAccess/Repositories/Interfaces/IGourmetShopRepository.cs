using GourmetShop.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Repositories
{
    // NOTE:
    // In the assignment instructions, you originally had a generic interface
    // However due to the rubric requiring two interfaces and typically the repositories would be in their own libraries
    // The interfaces for them implement CRUD on their own but still inherit from IGourmetShopRepository in case there are other functions that needs to be implemented but are shareable

    // Interfaces extending other interfaces have been used
    public interface IGourmetShopRepository<T>
    {
    }
}
