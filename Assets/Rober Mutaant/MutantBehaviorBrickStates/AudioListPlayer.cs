using System.Collections.Generic;
using UnityEngine;

public class AudioListPlayer : MonoBehaviour
{
    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(int index)
    {
        if (index < 0 || index >= audioClips.Count) return;

        audioSource.PlayOneShot(audioClips[index]);
    }
}