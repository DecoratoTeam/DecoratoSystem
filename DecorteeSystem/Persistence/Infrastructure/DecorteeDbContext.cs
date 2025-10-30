using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DecorteeDbContext(DbContextOptions<DecorteeDbContext> options):DbContext(options)
    {
        public DbSet<User> Users { get; set; }


    }
}
