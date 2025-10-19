using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solomon_Lorena_Lab2.Models;

namespace Solomon_Lorena_Lab2.Data
{
    public class Solomon_Lorena_Lab2Context : DbContext
    {
        public Solomon_Lorena_Lab2Context (DbContextOptions<Solomon_Lorena_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Solomon_Lorena_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Solomon_Lorena_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    }
}
