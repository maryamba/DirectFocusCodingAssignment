using DirectFocusCodingAssignment.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectFocusCodingAssignment.Data
{
    public class DFCContext:DbContext
    {
        public DFCContext(DbContextOptions<DFCContext> options)
            :base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

    }
}
