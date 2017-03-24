using ScoutingServer.SQLDataObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RobotServer.SQLDataObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using ScoutingServer.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using BlueAllianceClient;
using Microsoft.Extensions.Logging;
using ScoutingServer.Controllers;

namespace RobotServer.Models
{
    public class RoboContext : IdentityDbContext<Account> {        

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<RobotEvent> RobotEvents { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Performance> Performances { get; set; }

        public DbSet<TeamEvent> TeamEvents { get; set; }
        public DbSet<DataSheet> DataSheets { get; set; }
        public DbSet<Note> Notes { get; set; }

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

            //modelBuilder.Entity<RobotEvent>().HasOne<Account>(i => i.Poster).WithMany(q => q.RobotEvents).HasForeignKey(b => b.PosterId);
            //RobotEvents.Include(u => u.Poster);
            //Accounts.Include(o => o.RobotEvents);

            modelBuilder.Entity<Note>().HasOne<DataSheet>(g => g.DataSheet).WithMany(t => t.Notes).HasForeignKey(s => s.DataSheetId);

            base.OnModelCreating(modelBuilder);
        }

        public static async void Init(UserManager<Account> um, RoboContext context, ILogger logger) {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            /*um.CreateAsync(new Account()
            {
                UserName = "DevBo"
            }, "devaregod").Wait();*/
            BlueAllianceContext refresher = new BlueAllianceContext();
            try
            {
                var events = await refresher.GetEvents(2017);

                var dbEvents = context.Events.ToList();

                foreach (var e in dbEvents)
                {
                    var ev = events.FirstOrDefault(x => x.Key == e.EventId);
                    if (ev != null)
                    {
                        e.Location = ev.Location;
                        e.EventId = ev.Key;
                        events.Remove(ev);
                    }
                    else
                    {
                        context.Events.Remove(e);
                    }
                }

                foreach (var e in events)
                {
                    context.Events.Add(new Event(e));
                }

                context.SaveChanges();
                foreach (var e in events)
                {
                    try
                    {
                        await EventController.GetEvent(logger, context, e.Key, 2017);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }
    }

}
