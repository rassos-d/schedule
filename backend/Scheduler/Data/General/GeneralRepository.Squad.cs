using Scheduler.Models.General;

namespace Scheduler.Data.General;

public partial class GeneralRepository
{
    public Squad? GetSquad(Guid id) => _data.Squads.FirstOrDefault(s => s.Id == id);
    public List<Squad> GetAllSquads() => _data.Squads;
    public void AddSquad(Squad squad) => _data.Squads.Add(squad);
    public bool UpdateSquad(Squad squad)
    {
        var index = _data.Squads.FindIndex(s => s.Id == squad.Id);
        if (index == -1) return false;
        _data.Squads[index] = squad;
        return true;
    }
    public bool DeleteSquad(Guid id)
    {
        var squad = GetSquad(id);
        if (squad == null) return false;
        return _data.Squads.Remove(squad);
    }
}