using Scheduler.DataAccess;
using GeneralRepository = Scheduler.DataAccess.GeneralRepository;
using PlanRepository = Scheduler.DataAccess.Plan.PlanRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<GeneralRepository>();
builder.Services.AddSingleton<ScheduleRepository>();
builder.Services.AddSingleton<PlanRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c => 
{ 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedule API v1");
});
                            
app.MapControllers();
app.UseCors(builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
    builder.AllowCredentials();
});

app.Run();