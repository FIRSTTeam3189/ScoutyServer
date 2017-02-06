using ScoutingServer.SQLDataObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RobotServer.SQLDataObjects;

namespace RobotServer.Models
{
    public class RoboContext : DbContext {        

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSecurity> AccountSecurities { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<RobotEvent> RobotEvents { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Role> Roles { get; set; }

        public RoboContext(DbContextOptions<RoboContext> options) : base(options) {

        }

        public static void Init(RoboContext context) {
            context.Database.EnsureCreated();
        }
    }

}
