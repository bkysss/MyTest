using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Models;
namespace Test.Data
{
    public class UserInfoContext : DbContext
    {
        public UserInfoContext()
        {
        }

        public UserInfoContext(DbContextOptions<UserInfoContext> options)
            : base(options)
        {
        }

        public DbSet<Test.Models.UserInfo> UserInfo { get; set; } = default!;

        

        
       
        
    }
}
