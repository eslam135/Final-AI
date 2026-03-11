// GoapAction.cs
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    public string actionName;
    public float cost = 1f;

    public Dictionary<string, bool> preconditions = new Dictionary<string, bool>();
    public Dictionary<string, bool> effects = new Dictionary<string, bool>();

    [Header("Audio")]
    public ActionAudioConfig audio;

    // Cached reference — set by EnemyAIContext on init
    [HideInInspector] public AudioSource audioSource;

    // ── Validity ──────────────────────────────────────────────────────────────
    public virtual bool IsValid(WorldState state) => state.MeetsConditions(preconditions);

    // ── Lifecycle (override in subclasses) ────────────────────────────────────
    public virtual void OnStart(EnemyAIContext ctx)
    {
        PlayWindup();
    }

    public abstract bool Execute(EnemyAIContext ctx);

    /// <summary>
    /// Called by ActionAnimationReceiver when the "OnActionHit" Animation Event fires.
    /// Override to apply damage / knockback / projectile spawn etc.
    /// </summary>
    public virtual void OnAnimationHit(EnemyAIContext ctx)
    {
        PlayHit();
    }

    /// <summary>
    /// Called by ActionAnimationReceiver when the "OnActionRecovery" Animation Event fires.
    /// Override for recovery-phase logic (re-enable colliders, reset flags, etc.)
    /// </summary>
    public virtual void OnAnimationRecovery(EnemyAIContext ctx)
    {
        PlayRecovery();
    }

    public virtual void OnEnd(EnemyAIContext ctx) { }

    // ── Audio helpers ─────────────────────────────────────────────────────────
    protected void PlayWindup() => PlayClip(audio.windupClip, audio.windupVolume);
    protected void PlayHit() => PlayClip(audio.hitClip, audio.hitVolume);
    protected void PlayRecovery() => PlayClip(audio.recoveryClip, audio.recoveryVolume);

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip, volume);
    }
}