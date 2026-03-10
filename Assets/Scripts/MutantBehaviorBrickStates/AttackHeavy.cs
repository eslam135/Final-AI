using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Actions/Attack Heavy")]
public class AttackHeavy : GOAction
{
    public GameObject targetPosition;

    [InParam("Attack Range")]
    public float attackRange = 2.0f;

    [InParam("Attack Cooldown")]
    public float attackCooldown = 1.5f;

    [InParam("Rotation Speed")]
    public float rotationSpeed = 5.0f;

    private Animator animator;  
    private float nextAttackTime;

    public override void OnStart()
    {
        base.OnStart();
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Run", false);
    }

    public override TaskStatus OnUpdate()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        if (targetPosition == null) return TaskStatus.FAILED;

        float distance = Vector3.Distance(gameObject.transform.position, targetPosition.transform.position);
        if (distance > attackRange)
        {
            return TaskStatus.FAILED;
        }

        Vector3 direction = (targetPosition.transform.position - gameObject.transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("HeavyAttack");
            nextAttackTime = Time.time + attackCooldown;
        }

        return TaskStatus.RUNNING;
    }
}