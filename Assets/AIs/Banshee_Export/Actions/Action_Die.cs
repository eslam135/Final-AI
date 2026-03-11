using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Banshee/Die")]
public class Action_Die : GOAction
{
    private NavMeshAgent agent;
    private Animator anim;
    private CapsuleCollider col;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        col = gameObject.GetComponent<CapsuleCollider>();

        if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
        if (col != null) col.enabled = false;

        if (anim != null)
        {
            anim.SetBool("isDead", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isThrowing", false);
            anim.SetBool("isScreaming", false);
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.RUNNING; 
    }
}