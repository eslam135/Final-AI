using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Banshee/Scream Attack")]
public class Banshee_Action_Scream : GOAction
{
    [InParam("target")] public Transform target;
    [InParam("screamVFX")] public ParticleSystem screamVFX;

    [InParam("attackDuration")] public float attackDuration = 2.5f;
    [InParam("screamHitTime")] public float screamHitTime = 0.5f;
    [InParam("screamDamage")] public int screamDamage = 30;
    [InParam("damageRadius")] public float damageRadius = 4.0f;
    [InParam("cooldownTime")] public float cooldownTime = 8.0f;

    private NavMeshAgent agent;
    private Animator anim;
    private AudioSource audioSource;
    private BansheeMemory memory;
    private float timer = 0f;
    private bool hasScreamed = false;
    private bool isValidAttack = false;

    public override void OnStart()
    {
        memory = gameObject.GetComponent<BansheeMemory>();

        if (memory != null && Time.time >= memory.nextScreamTime)
        {
            isValidAttack = true;
            memory.nextScreamTime = Time.time + cooldownTime; 

            agent = gameObject.GetComponent<NavMeshAgent>();
            anim = gameObject.GetComponentInChildren<Animator>();
            audioSource = gameObject.GetComponent<AudioSource>();
            timer = 0f;
            hasScreamed = false;

            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            if (anim != null)
            {
                anim.SetBool("isScreaming", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isThrowing", false);
                anim.SetBool("isMelee", false);
            }
        }
        else
        {
            isValidAttack = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!isValidAttack) return TaskStatus.FAILED;

        if (target != null)
        {
            Vector3 direction = (target.position - gameObject.transform.position).normalized;
            direction.y = 0;
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }

        timer += Time.deltaTime;

        if (timer >= screamHitTime && !hasScreamed)
        {
            hasScreamed = true;
            if (screamVFX != null) screamVFX.Play();
            if (audioSource != null && audioSource.clip != null) audioSource.Play();

            if (target != null && Vector3.Distance(gameObject.transform.position, target.position) <= damageRadius)
            {
                PlayerHealthDummy playerHealth = target.GetComponent<PlayerHealthDummy>();
                if (playerHealth != null) playerHealth.TakeDamage(screamDamage);
            }
        }

        if (timer >= attackDuration)
        {
            if (anim != null) anim.SetBool("isScreaming", false);
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        if (anim != null) anim.SetBool("isScreaming", false);
    }
}