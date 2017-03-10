using ScoutingServer.SQLDataObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RobotServer.SQLDataObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RobotServer.Models
{
    public class RoboContext : IdentityDbContext {        

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<RobotEvent> RobotEvents { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Performance> Performances { get; set; }

        public DbSet<TeamEvent> TeamEvents { get; set; }

        public RoboContext(DbContextOptions<RoboContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<TeamEvent>().HasOne<Team>(t => t.Team).WithMany(t => t.Events).HasForeignKey(t => t.TeamNumber);
            modelBuilder.Entity<TeamEvent>().HasOne<Event>(e => e.Event).WithMany(e => e.TeamEvents).HasForeignKey(e => e.EventId);
            modelBuilder.Entity<TeamEvent>().HasKey(t => new {t.EventId, t.TeamNumber});
            //Events.Include(e => e.TeamEvents).ThenInclude(t => t.Team);

            modelBuilder.Entity<Match>().HasOne<Event>(e => e.Event).WithMany(t => t.Matchs).HasForeignKey(t => t.EventId);
            //Events.Include(e => e.Matchs);
            //Matches.Include(t => t.Event);

            modelBuilder.Entity<RobotEvent>().HasOne<Match>(p => p.Match).WithMany(w => w.RobotEvents).HasForeignKey(f => f.MatchId);
            modelBuilder.Entity<RobotEvent>().HasOne<Team>(h => h.Team).WithMany(f => f.RobotEvents).HasForeignKey(l => l.TeamNumber);
            //RobotEvents.Include(r => r.Match);
            //RobotEvents.Include(j => j.Team);

            modelBuilder.Entity<Performance>().HasOne<Match>(m => m.Match).WithMany(v => v.Performances).HasForeignKey(g => g.MatchId);
            modelBuilder.Entity<Performance>().HasOne<Team>(k => k.Team).WithMany(d => d.Performances).HasForeignKey(u => u.TeamNumber);
            modelBuilder.Entity<Performance>().HasKey(y => new {y.MatchId, y.TeamNumber});
            //Matches.Include(l => l.Performances);
            //Teams.Include(x => x.Performances);

            modelBuilder.Entity<RobotEvent>().HasOne<Account>(i => i.Poster).WithMany(q => q.RobotEvents).HasForeignKey(b => b.PosterId);
            //RobotEvents.Include(u => u.Poster);
            //Accounts.Include(o => o.RobotEvents);

            base.OnModelCreating(modelBuilder);
        }

        public static void Init(RoboContext context) {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }

}
