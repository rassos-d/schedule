using Scheduler.DataAccess;
using Scheduler.Services;
using GeneralRepository = Scheduler.DataAccess.General.GeneralRepository;
using PlanRepository = Scheduler.DataAccess.Plan.PlanRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<GeneralRepository>();
builder.Services.AddSingleton<ScheduleRepository>();
builder.Services.AddSingleton<PlanRepository>();

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