// Action_Idle.cs
using UnityEngine;

public class Action_Idle : GoapAction
{
    private void Awake()
    {
        actionName = "Idle";
        cost = 100f;
    }

    public override bool IsValid(WorldState state) => true;

    public override bool Execute(EnemyAIContext ctx)
    {
        ctx.animator?.SetBool("IsIdle", true);
        // Mana regenerates passively in EnemyAIContext.Update()
        return false; // never finishes — planner will swap when something else is valid
    }

    public override void OnEnd(EnemyAIContext ctx)
    {
        ctx.animator?.SetBool("IsIdle", false);
    }
}