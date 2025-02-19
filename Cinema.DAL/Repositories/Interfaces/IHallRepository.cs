using Cinema.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.DAL.Repositories.Interfaces
{
    public interface IHallRepository : IRepository<Hall>
    {
        Task<Hall?> GetByNameAsync(string name);
    }

}
