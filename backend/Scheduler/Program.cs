using Scheduler.Constants;
using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.Export;
using Scheduler.Services.Events;
using Scheduler.Services.General;
using Scheduler.Services.Migration;
using Scheduler.Services.Schedule;
using PlanRepository = Scheduler.DataAccess.Plan.PlanRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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
builder.Services.AddSingleton<EventGenerator>();
builder.Services.AddSingleton<ExcelExportService>();
builder.Services.AddHostedService<StartupService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var baseFolder = Directory.GetCurrentDirectory();
FilePaths.BaseFolder = Path.Combine(baseFolder, "data");

Console.WriteLine(baseFolder);

var app = builder.Build();

app.UseCors("AllowAll"); // Включаем CORS middleware

app.UseSwagger();
app.UseSwaggerUI(c => 
{ 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedule API v1");
});

app.MapControllers();


app.Run();