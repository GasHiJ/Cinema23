using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.DAL.Repositories
{
    public class HallRepository : Repository<Hall>, IHallRepository
    {
        public HallRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Hall?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(h => h.Name == name);
        }
    }

}
