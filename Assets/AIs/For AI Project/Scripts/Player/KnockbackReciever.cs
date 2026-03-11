// KnockbackReceiver.cs
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    private Vector3 _knockbackVelocity;
    private CharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    public void ApplyKnockback(Vector3 impulse)
    {
        _knockbackVelocity += impulse;
    }

    private void Update()
    {
        if (_knockbackVelocity.sqrMagnitude < 0.01f)
        {
            _knockbackVelocity = Vector3.zero;
            return;
        }

        _cc.Move(_knockbackVelocity * Time.deltaTime);
        _knockbackVelocity = Vector3.Lerp(_knockbackVelocity, Vector3.zero, 10f * Time.deltaTime);
    }
}