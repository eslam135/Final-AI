// ActionAudioConfig.cs
using UnityEngine;

/// <summary>
/// Serializable audio config for a single GOAP action.
/// Assign clips directly in the Inspector on each Action component.
/// </summary>
[System.Serializable]
public class ActionAudioConfig
{
    [Header("Wind-up (plays on OnStart)")]
    public AudioClip windupClip;
    [Range(0f, 1f)] public float windupVolume = 1f;

    [Header("Hit / Impact (plays via Animation Event)")]
    public AudioClip hitClip;
    [Range(0f, 1f)] public float hitVolume = 1f;

    [Header("Recovery / End (plays via Animation Event or OnEnd)")]
    public AudioClip recoveryClip;
    [Range(0f, 1f)] public float recoveryVolume = 1f;
}