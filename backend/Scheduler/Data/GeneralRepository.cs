public class GeneralRepository
{
    private readonly string _filePath;
    private GeneralData _data;
    private readonly JsonSerializerOptions _jsonOptions;

    public GeneralRepository(string basePath = "data")
    {
        _filePath = Path.Combine(basePath, "general.json");
        Directory.CreateDirectory(basePath);
        
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        LoadData();
    }

    private void LoadData()
    {
        _data = File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(_filePath), _jsonOptions) ?? new GeneralData()
            : new GeneralData();
    }

    public void SaveChanges() => File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, _jsonOptions));

    #region Audience CRUD
    public Audience? GetAudience(Guid id) => _data.Audiences.FirstOrDefault(a => a.Id == id);
    public List<Audience> GetAllAudiences() => _data.Audiences;
    public void AddAudience(Audience audience) => _data.Audiences.Add(audience);
    public bool UpdateAudience(Audience audience)
    {
        var index = _data.Audiences.FindIndex(a => a.Id == audience.Id);
        if (index == -1) return false;
        _data.Audiences[index] = audience;
        return true;
    }
    public bool DeleteAudience(Guid id)
    {
        var audience = GetAudience(id);
        if (audience == null) return false;
        return _data.Audiences.Remove(audience);
    }
    #endregion

    #region Squad CRUD
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
    #endregion

    #region Teacher CRUD
    public Teacher? GetTeacher(Guid id) => _data.Teachers.FirstOrDefault(t => t.Id == id);
    public List<Teacher> GetAllTeachers() => _data.Teachers;
    public void AddTeacher(Teacher teacher) => _data.Teachers.Add(teacher);
    public bool UpdateTeacher(Teacher teacher)
    {
        var index = _data.Teachers.FindIndex(t => t.Id == teacher.Id);
        if (index == -1) return false;
        _data.Teachers[index] = teacher;
        return true;
    }
    public bool DeleteTeacher(Guid id)
    {
        var teacher = GetTeacher(id);
        if (teacher == null) return false;
        return _data.Teachers.Remove(teacher);
    }
    #endregion

    public void SaveChangesToDisk() => SaveChanges();
}