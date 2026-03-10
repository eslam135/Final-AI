using UnityEngine;

public class DebrisProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 15;
    public float lifetime = 3f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthDummy playerHealth = other.GetComponent<PlayerHealthDummy>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}