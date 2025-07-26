using Scheduler.DataAccess.Plan;

namespace Scheduler.Services.Migration;

public class StartupService : IHostedService
{
    private readonly PlanRepository _planRepository;
    private IHostedService _hostedServiceImplementation;

    public StartupService(PlanRepository planRepository)
    {
        _planRepository = planRepository;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _planRepository.UpdateSubjectColors();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
}