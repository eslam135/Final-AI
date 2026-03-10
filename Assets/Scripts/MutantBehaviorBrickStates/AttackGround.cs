using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;[Action("Actions/Attack Ground")]
public class AttackGround : GOAction
{
    public GameObject targetPosition;

    [InParam("Spike Prefab")]
    public GameObject spikePrefab;

    [InParam("Spike Range")]
    public float spikeRange = 8.0f;

    [InParam("Spike Cooldown")]
    public float spikeCooldown = 5.0f;

    [InParam("Attack Duration")]
    public float attackDuration = 1.5f;

    [InParam("Rotation Speed")]
    public float rotationSpeed = 5.0f;

    private Animator animator;
    private float nextSpikeTime = 0f; 
    private float finishAnimTime = 0f;
    private bool isAttacking = false;

    public override void OnStart()
    {
        base.OnStart();
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        isAttacking = false; 
    }

    public override TaskStatus OnUpdate()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        if (targetPosition == null || spikePrefab == null) return TaskStatus.FAILED;
        if (isAttacking)
        {
            if (Time.time >= finishAnimTime)
            {
                isAttacking = false;
                return TaskStatus.COMPLETED;
            }
            return TaskStatus.RUNNING;
        }

        if (Time.time < nextSpikeTime)
        {
            return TaskStatus.FAILED;
        }

        float distance = Vector3.Distance(gameObject.transform.position, targetPosition.transform.position);
        if (distance > spikeRange)
        {
            return TaskStatus.FAILED; 
        }

        animator.SetBool("Run", false);
        
        Vector3 direction = (targetPosition.transform.position - gameObject.transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(direction);
        }

        animator.SetTrigger("GroundAttack");
        GameObject spikes = GameObject.Instantiate(spikePrefab, targetPosition.transform.position, Quaternion.identity);
        spikes.transform.forward = Vector3.up;
        spikes.transform.transform.position = 
        new Vector3(spikes.transform.transform.position.x , spikes.transform.transform.position.y - 1.0f
        ,spikes.transform.transform.position.z );
        
        nextSpikeTime = Time.time + spikeCooldown;
        finishAnimTime = Time.time + attackDuration;
        isAttacking = true;

        return TaskStatus.RUNNING;
    }
}