using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository
{
    public Squad? GetSquad(Guid id) => _data.Squads.GetValueOrDefault(id);
    public List<Squad> GetAllSquads() => _data.Squads.Values.ToList();
    public void AddSquad(Squad squad) => _data.Squads.Add(squad.Id, squad);
    public void UpsertSquad(Squad squad) => _data.Squads[squad.Id] = squad;
    public void DeleteSquad(Guid id) => _data.Squads.Remove(id);
}