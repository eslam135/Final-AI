// PlayerKnockback.cs — Add to your Player
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private CharacterController _cc;
    private Vector3 _knockbackVelocity;
    private float _knockbackDecay = 5f;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        _knockbackVelocity = direction * force;
    }

    private void Update()
    {
        if (_knockbackVelocity.sqrMagnitude > 0.1f)
        {
            if (_cc != null)
            {
                _cc.Move(_knockbackVelocity * Time.deltaTime);
            }
            else
            {
                transform.position += _knockbackVelocity * Time.deltaTime;
            }

            // Decay
            _knockbackVelocity = Vector3.Lerp(
                _knockbackVelocity, Vector3.zero, _knockbackDecay * Time.deltaTime);
        }
    }
}