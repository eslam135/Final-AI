// PortalBeamController.cs
using UnityEngine;
using System.Collections;

public class PortalBeamController : MonoBehaviour
{
    private GameObject _beamPrefab;
    private Vector3 _spawnPosition;
    private Vector3 _targetGround;
    private float _warningDelay;
    private float _beamDuration;
    private float _beamRadius;
    private float _damage;
    private LayerMask _wallLayer;

    private GameObject _beamInstance;
    private bool _hasDamaged = false;

    public void Init(
        GameObject beamPrefab,
        Vector3 spawnPosition,
        Vector3 targetGround,
        float warningDelay,
        float beamDuration,
        float beamRadius,
        float damage,
        LayerMask wallLayer)
    {
        _beamPrefab = beamPrefab;
        _spawnPosition = spawnPosition;
        _targetGround = targetGround;
        _warningDelay = warningDelay;
        _beamDuration = beamDuration;
        _beamRadius = beamRadius;
        _damage = damage;
        _wallLayer = wallLayer;

        StartCoroutine(BeamSequence());
    }

    private IEnumerator BeamSequence()
    {
        // ===== WARNING PHASE =====
        // Spawn a ground indicator so the player knows where the beam will land
        GameObject warning = CreateWarningIndicator();
        warning.transform.position = _targetGround + Vector3.up * 0.05f;

        yield return new WaitForSeconds(_warningDelay);

        // ===== BEAM PHASE =====
        Destroy(warning);

        // Spawn the beam VFX (particle system shooting downward)
        if (_beamPrefab != null)
        {
            _beamInstance = Instantiate(_beamPrefab, _spawnPosition,Quaternion.identity);
            _beamInstance.transform.LookAt(_targetGround);
            // The beam VFX should have its own ParticleSystem with start delay = 0
            // since we already waited for the warning
        }

        // Damage tick loop
        float elapsed = 0f;
        float tickRate = 0.2f;
        float tickTimer = 0f;

        while (elapsed < _beamDuration)
        {
            elapsed += Time.deltaTime;
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickRate)
            {
                tickTimer = 0f;
                DamageInRadius();
            }

            yield return null;
        }

        // ===== CLEANUP =====
        if (_beamInstance != null)
            Destroy(_beamInstance, 0.5f);

        Destroy(gameObject, 1f);
    }

    private void DamageInRadius()
    {
        // OverlapSphere at ground target position
        Collider[] hits = Physics.OverlapSphere(_targetGround, _beamRadius);

        foreach (var hit in hits)
        {
            var hp = hit.GetComponent<HealthView>()?.Health;
            if (hp != null)
            {
                hp.TakeDamage(_damage * 0.2f); // damage per tick 
                                               // (5 ticks per second × 0.2 = full damage over 1s)
                Debug.Log($"Portal beam hit player for {_damage * 0.2f} damage!");
            }
        }
    }

    private GameObject CreateWarningIndicator()
    {
        // Simple red circle on the ground using a projector or quad
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        indicator.name = "BeamWarning";
        indicator.transform.localScale = new Vector3(
            _beamRadius * 2f, 0.02f, _beamRadius * 2f);

        // Remove collider so it doesn't interfere
        Destroy(indicator.GetComponent<Collider>());

        // Red transparent material
        Renderer rend = indicator.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(1f, 0f, 0f, 0.4f);
        SetMaterialTransparent(mat);
        rend.material = mat;

        // Pulse effect
        WarningPulse pulse = indicator.AddComponent<WarningPulse>();
        pulse.duration = _warningDelay;

        return indicator;
    }

    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3); // Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_targetGround, _beamRadius);
    }
}