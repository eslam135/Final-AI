// GoapPlanner.cs
using System.Collections.Generic;
using System.Linq;

public class GoapPlanner
{
    public GoapAction Plan(List<GoapAction> availableActions, WorldState world)
    {
        return availableActions
            .Where(a => a.IsValid(world))
            .OrderBy(a => a.cost)
            .FirstOrDefault();
    }
}