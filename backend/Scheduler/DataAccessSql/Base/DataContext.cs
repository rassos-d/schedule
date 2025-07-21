using Microsoft.EntityFrameworkCore;
using Scheduler.SqlEntities.General;
using Scheduler.SqlEntities.Plan;
using Scheduler.SqlEntities.Schedule;

namespace Scheduler.DataAccessSql.Base;

public class DataContext : DbContext
{
    // general
    public DbSet<Audience> Audiences { get; set; }
    public DbSet<Squad> Squads { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    
    // plan
    public DbSet<Direction> Directions { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    
    // schedules
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<SchedulePage> SchedulePages { get; set; }
    public DbSet<Event> Events { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("DataSource=:memory:");
    }
}