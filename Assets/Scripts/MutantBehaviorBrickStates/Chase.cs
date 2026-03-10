using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Actions/Chase")]
public class Chase : GOAction
{
    public GameObject targetPosition;

    [InParam("Chase Range")]
    public float chaseRange;

    [InParam("Speed")]
    public float speed;

    [InParam("Rotation Speed")]
    public float rotationSpeed;

    private Animator animator;

    public override void OnStart()
    {
        base.OnStart();
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Run", true);
    }

    public override TaskStatus OnUpdate()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        if (targetPosition == null) return TaskStatus.FAILED;
        float distance = Vector3.Distance(gameObject.transform.position, targetPosition.transform.position);

        if (distance <= chaseRange)
            return TaskStatus.FAILED;

        Vector3 direction = (targetPosition.transform.position - gameObject.transform.position).normalized;
        
        Vector3 lookDirection = direction;
        lookDirection.y = 0; 

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, speed * Time.deltaTime);

        return TaskStatus.RUNNING;
    }
}