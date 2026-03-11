// EnemyAIContext.cs
using UnityEngine;
using System.Collections.Generic;

public class EnemyAIContext : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    public Rigidbody playerRigidbody;

    [Header("Detection Ranges")]
    public float detectionRadius = 25f;
    public float screamRange = 5f;
    public float healRadius = 10f;
    public LayerMask wallLayer;
    public LayerMask enemyLayer;

    [Header("Health")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float healAmount = 20f;
    HealthView healthView;

    [Header("Mana")]
    public float mana = 20f;
    public float maxMana = 20f;
    public float manaRegenRate = 2f;

    [Header("Mana Costs")]
    public float screamManaCost = 4f;
    public float laserManaCost = 7f;
    public float portalBeamManaCost = 10f;

    [Header("Scream Attack")]
    public float screamKnockbackForce = 18f;
    public float screamDamage = 10f;
    public GameObject screamVFXPrefab;

    [Header("Laser Beam Attack")]
    public float laserDamage = 20f;
    public float laserSpeed = 40f;
    public GameObject laserProjectilePrefab;
    public Transform laserFirePoint;

    [Header("Portal Beam Attack")]
    public float portalBeamDamage = 35f;
    public float portalSpawnHeight = 4f;
    public float portalWarningDelay = 1.5f;
    public float portalBeamDuration = 1.0f;
    public float portalBeamRadius = 1.5f;
    public GameObject portalEntryVFXPrefab;
    public GameObject portalExitVFXPrefab;
    public GameObject portalBeamVFXPrefab;
    public Transform portalFirePoint;

    [Header("Cooldowns")]
    public float screamCooldown = 3f;
    public float laserCooldown = 2f;
    public float portalBeamCooldown = 5f;
    public float healCooldown = 4f;

    [HideInInspector] public float lastScreamTime = -99f;
    [HideInInspector] public float lastLaserTime = -99f;
    [HideInInspector] public float lastPortalBeamTime = -99f;
    [HideInInspector] public float lastHealTime = -99f;

    [HideInInspector] public List<EnemyAIContext> nearbyAllies = new List<EnemyAIContext>();

    // ── Runtime action tracking ───────────────────────────────────────────────
    [HideInInspector] public GoapAction currentAction;

    // ── Cached components ─────────────────────────────────────────────────────
    [HideInInspector] public ActionAnimationReceiver animReceiver;
    private AudioSource _audioSource;

    // ─────────────────────────────────────────────────────────────────────────
    
    private void Awake()
    {
        // Animation event receiver (must be on same GO as Animator)
        animReceiver = animator != null
            ? animator.GetComponent<ActionAnimationReceiver>()
            : GetComponentInChildren<ActionAnimationReceiver>();

        if (animReceiver != null)
            animReceiver.context = this;
        else
            Debug.LogWarning($"[{name}] No ActionAnimationReceiver found. Animation events won't fire.");

        // AudioSource — must live on this GameObject
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogWarning($"[{name}] No AudioSource found. Attack audio won't play.");

        // Wire AudioSource into every GoapAction on this GameObject
        foreach (var action in GetComponents<GoapAction>())
            action.audioSource = _audioSource;
        healthView = GetComponentInChildren<HealthView>();

    }

    private void Update()
    {
        if (healthView != null && healthView.GetHealth() <= 0)
        {
            return;
        }
        // Passive mana regen
        if (mana < maxMana)
            mana = Mathf.Min(mana + manaRegenRate * Time.deltaTime, maxMana);
    }

    // ── Action management (call these from your GOAP executor) ─────────────────

    /// <summary>
    /// Call this instead of action.OnStart() directly so the receiver stays in sync.
    /// </summary>
    public void StartAction(GoapAction action)
    {
        currentAction = action;
        if (animReceiver != null)
            animReceiver.currentAction = action;

        action.OnStart(this);
    }

    /// <summary>
    /// Call this instead of action.OnEnd() directly.
    /// </summary>
    public void EndAction(GoapAction action)
    {
        action.OnEnd(this);

        if (animReceiver != null)
            animReceiver.currentAction = null;

        currentAction = null;
    }

    // ── Mana ──────────────────────────────────────────────────────────────────
    public bool TrySpendMana(float amount)
    {
        if (mana >= amount) { mana -= amount; return true; }
        return false;
    }
}