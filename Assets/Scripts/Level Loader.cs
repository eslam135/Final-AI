using System.Collections;
using UnityEngine;

/// <summary>
/// LevelLoader — drag your Environment and Player GameObjects
/// (already in the hierarchy) into the Inspector fields.
/// On Play, the Environment activates first, then the Player
/// is activated after it is ready. Nothing is instantiated or destroyed.
/// </summary>
public class LevelLoader : MonoBehaviour
{
    [Header("Level Objects (assign from Hierarchy)")]
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform SpawnPoint;
    [Header("Timing")]
    [Tooltip("Delay in seconds between environment activating and player activating.")]
    [SerializeField] private float delayBeforePlayer = 0.5f;

    // ---------------------------------------------------------------
    // Unity lifecycle
    // ---------------------------------------------------------------
    private void Awake()
    {
        // Hide both immediately so nothing pops in before we're ready.
        if (environment != null) environment.SetActive(false);
        if (player != null) player.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    // ---------------------------------------------------------------
    // Loading sequence
    // ---------------------------------------------------------------
    private IEnumerator LoadLevel()
    {
        // --- Validate ---
        if (environment == null)
        {
            Debug.LogError("[LevelLoader] Environment is not assigned!", this);
            yield break;
        }

        if (player == null)
        {
            Debug.LogError("[LevelLoader] Player is not assigned!", this);
            yield break;
        }

        // --- Step 1: Activate Environment ---
        Debug.Log("[LevelLoader] Activating environment...");
        environment.SetActive(true);

        // Wait a frame so all Awake / OnEnable calls on the environment finish.
        yield return null;

        // Optional extra delay before spawning the player.
        if (delayBeforePlayer > 0f)
            yield return new WaitForSeconds(delayBeforePlayer);

        Debug.Log("[LevelLoader] Environment ready.");

        // --- Step 2: Activate Player ---
        Debug.Log("[LevelLoader] Activating player...");
        player.transform.position = SpawnPoint.position;
        player.SetActive(true);
        
        // Wait a frame so the player's Awake / Start methods finish.
        yield return null;

        Debug.Log("[LevelLoader] Player ready.");

        // --- Step 3: Done ---
        OnLevelLoaded();
    }

    // ---------------------------------------------------------------
    // Called when everything is loaded.
    // Override in a subclass to e.g. hide a loading screen.
    // ---------------------------------------------------------------
    protected virtual void OnLevelLoaded()
    {
        Debug.Log("[LevelLoader] Level fully loaded and ready.");
    }

    // ---------------------------------------------------------------
    // Reload: deactivates both and runs the sequence again.
    // ---------------------------------------------------------------
    public void ReloadLevel()
    {
        StopAllCoroutines();
        if (environment != null) environment.SetActive(false);
        if (player != null) player.SetActive(false);
        StartCoroutine(LoadLevel());
    }
}