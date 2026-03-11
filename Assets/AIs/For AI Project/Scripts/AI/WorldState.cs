// WorldState.cs
using System.Collections.Generic;

public class WorldState
{
    private Dictionary<string, bool> _states = new Dictionary<string, bool>();

    public void Set(string key, bool value) => _states[key] = value;

    public bool Get(string key) => _states.ContainsKey(key) && _states[key];

    public bool MeetsConditions(Dictionary<string, bool> conditions)
    {
        foreach (var kvp in conditions)
        {
            if (Get(kvp.Key) != kvp.Value)
                return false;
        }
        return true;
    }
}