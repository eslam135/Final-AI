// ActionAnimationReceiver.cs
using UnityEngine;

/// <summary>
/// Attach this to the same GameObject as the Animator (or the enemy root).
///
/// In the Unity Animator window, add Animation Events to your clips:
///   • Function: "OnActionHit"      — at the impact frame
///   • Function: "OnActionRecovery" — at the recovery frame
///
/// This component routes those events to whichever GoapAction is active.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ActionAnimationReceiver : MonoBehaviour
{
    // Set by EnemyAIContext when it starts/ends an action
    [HideInInspector] public EnemyAIContext context;
    [HideInInspector] public GoapAction currentAction;

    /// <summary>
    /// Place this function name in the Animation Event at the hit/impact frame.
    /// </summary>
    public void OnActionHit()
    {
        if (currentAction == null || context == null) return;
        currentAction.OnAnimationHit(context);
    }

    /// <summary>
    /// Place this function name in the Animation Event at the recovery frame.
    /// </summary>
    public void OnActionRecovery()
    {
        if (currentAction == null || context == null) return;
        currentAction.OnAnimationRecovery(context);
    }
}