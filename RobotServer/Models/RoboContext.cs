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

        public DbSet<TeamEvent> TeamEvents { get; set; }

        public RoboContext(DbContextOptions<RoboContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamEvent>().HasOne<Team>(t => t.Team).WithMany(t => t.Events).HasForeignKey(t => t.TeamNumber);
            modelBuilder.Entity<TeamEvent>().HasOne<Event>(e => e.Event).WithMany(e => e.Teams).HasForeignKey(e => e.EventId);
            modelBuilder.Entity<TeamEvent>().HasKey(t => new {t.EventId, t.TeamNumber});
            Events.Include(e => e.Teams).ThenInclude(t => t.Team);

            modelBuilder.Entity<Match>().HasOne<Event>(e => e.Event).WithMany(t => t.Matchs).HasForeignKey(t => t.EventId);
            Events.Include(e => e.Matchs);

            modelBuilder.Entity<RobotEvent>().HasOne<Performance>(p => p.Performance).WithMany(w => w.Events).HasForeignKey(f => f.PerformanceId);
            Performances.Include(r => r.Events);

            modelBuilder.Entity<Performance>().HasOne<Match>(m => m.Match).WithMany(v => v.Performances).HasForeignKey(g => g.MatchId);
            Matches.Include(l => l.Performances);

            modelBuilder.Entity<Performance>().HasOne<Team>(k => k.Team).WithMany(d => d.Performances).HasForeignKey(u => u.TeamNumber);
            Teams.Include(x => x.Performances);

        }

        public static void Init(RoboContext context) {
            context.Database.EnsureCreated();
        }
    }

}
