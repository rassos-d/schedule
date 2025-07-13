using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.Services.Events;
using Scheduler.Services.General;
using Scheduler.Services.Schedule;
using PlanRepository = Scheduler.DataAccess.Plan.PlanRepository;

var builder = WebApplication.CreateBuilder(args);

// Repo
builder.Services.AddSingleton<ScheduleRepository>();
builder.Services.AddSingleton<PlanRepository>();

builder.Services.AddSingleton<AudienceRepository>();
builder.Services.AddSingleton<SquadRepository>();
builder.Services.AddSingleton<TeacherRepository>();
builder.Services.AddSingleton<PlanRepository>();

builder.Services.AddSingleton<SquadService>();
builder.Services.AddSingleton<TeacherService>();
builder.Services.AddSingleton<ScheduleService>();
builder.Services.AddSingleton<EventService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("*", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => 
{ 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedule API v1");
});

app.UseCors("*");               
app.MapControllers();


app.Run();