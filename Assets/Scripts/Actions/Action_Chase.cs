using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Banshee/Chase")]
public class Action_Chase : GOAction
{
    [InParam("target")] public Transform target;
    [InParam("Speed")] private float speed = 2.0f;
    [InParam("Rotation Speed")] public float rotationSpeed;
    private NavMeshAgent agent;
    private Animator anim;
    private BansheeMemory memory;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        memory = gameObject.GetComponent<BansheeMemory>();

        if (agent != null)
        {
            agent.stoppingDistance = 1.5f;
            agent.isStopped = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null) return TaskStatus.FAILED;

        Vector3 myFlatPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        Vector3 targetFlatPos = new Vector3(target.position.x, 0, target.position.z);
        float dist = Vector3.Distance(myFlatPos, targetFlatPos);

        bool canScream = (memory != null && Time.time >= memory.nextScreamTime);
        bool canMelee = (memory != null && Time.time >= memory.nextMeleeTime);
        bool canThrow = (memory != null && Time.time >= memory.nextThrowTime);

        if (dist <= 2.2f && (canScream || canMelee))
        {
            return TaskStatus.FAILED;
        }

        if (dist >= 5f && dist <= 12f && canThrow)
        {
            return TaskStatus.FAILED;
        }

        if (dist <= agent.stoppingDistance)
        {
            if (agent != null) agent.isStopped = true;
            if (anim != null) anim.SetBool("isRunning", false);
        }
        else
        {
            if (agent != null)
            {
                agent.SetDestination(target.position);
                agent.isStopped = false;
            }
            if (anim != null)
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isThrowing", false);
                anim.SetBool("isScreaming", false);
                anim.SetBool("isMelee", false);
            }
        }

        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
        if (anim != null) anim.SetBool("isRunning", false);
    }
}