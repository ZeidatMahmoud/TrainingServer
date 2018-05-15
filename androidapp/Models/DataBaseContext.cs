using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace androidapp.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Login> Login { get; set; }
      

       // string con = @"Data Source=DESKTOP-653TN87\LOCALSERVER;Initial Catalog=Training;Persist Security Info=True;User ID=TestUser;Password=***********";
        public DataBaseContext(DbContextOptions<DataBaseContext>options)
            :base(options)

        {
           
        }
        
    }
}
