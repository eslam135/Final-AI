using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Actions/Die")]
public class Die : GOAction
{
    private Animator animator;
    private bool isDead = false;

    public override void OnStart()
    {
        base.OnStart();
        animator = gameObject.GetComponent<Animator>();

        if (!isDead)
        {
            animator.SetBool("Run", false);
            isDead = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.RUNNING;
    }
}