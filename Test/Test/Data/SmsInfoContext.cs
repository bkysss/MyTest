using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Models;
namespace Test.Data
{
    public class SmsInfoContext:DbContext
    {
        public SmsInfoContext(DbContextOptions<SmsInfoContext> options)
            : base(options)
        {
        }

        public SmsInfoContext()
        {

        }

        public DbSet<Test.Models.SmsInfo> SmsInfo { get; set; } = default!;
    }
}
