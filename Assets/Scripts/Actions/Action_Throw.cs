using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Banshee/Throw Attack")]
public class Action_Throw : GOAction
{
    [InParam("target")] public Transform target;
    [InParam("projectilePrefab")] public GameObject projectilePrefab;
    [InParam("spawnPoint")] public Transform spawnPoint;

    [InParam("attackDuration")] public float attackDuration = 2.0f;
    [InParam("throwReleaseTime")] public float throwReleaseTime = 0.8f;
    [InParam("cooldownTime")] public float cooldownTime = 5.0f;

    private NavMeshAgent agent;
    private Animator anim;
    private BansheeMemory memory;
    private float timer = 0f;
    private bool hasThrown = false;
    private bool isValidAttack = false;

    public override void OnStart()
    {
        memory = gameObject.GetComponent<BansheeMemory>();

        if (memory != null && Time.time >= memory.nextThrowTime)
        {
            isValidAttack = true;
            memory.nextThrowTime = Time.time + cooldownTime; 

            agent = gameObject.GetComponent<NavMeshAgent>();
            anim = gameObject.GetComponentInChildren<Animator>();
            timer = 0f;
            hasThrown = false;

            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            if (anim != null)
            {
                anim.SetBool("isThrowing", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isScreaming", false);
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
        if (gameObject.GetComponent<HealthView>().Health.currentHealth <= 0) return TaskStatus.FAILED;

        if (!isValidAttack) return TaskStatus.FAILED;

        if (target != null)
        {
            Vector3 direction = (target.position - gameObject.transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        timer += Time.deltaTime;

        if (timer >= throwReleaseTime && !hasThrown)
        {
            hasThrown = true;
            if (projectilePrefab != null && spawnPoint != null && target != null)
            {
                GameObject rock = GameObject.Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                rock.transform.LookAt(target.position + Vector3.up * 1.0f);
            }
        }

        if (timer >= attackDuration)
        {
            if (anim != null) anim.SetBool("isThrowing", false);
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        if (anim != null) anim.SetBool("isThrowing", false);
    }
}