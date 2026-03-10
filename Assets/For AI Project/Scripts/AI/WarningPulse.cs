// WarningPulse.cs
using UnityEngine;

public class WarningPulse : MonoBehaviour
{
    public float duration = 1.5f;
    private float _elapsed;
    private Renderer _rend;
    private Color _baseColor;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _baseColor = _rend.material.color;
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;

        // Pulse faster as the warning time runs out
        float urgency = _elapsed / duration; // 0 → 1
        float pulseSpeed = Mathf.Lerp(2f, 12f, urgency);
        float alpha = Mathf.Lerp(0.15f, 0.6f, (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f);

        Color c = _baseColor;
        c.a = alpha;
        _rend.material.color = c;
    }
}