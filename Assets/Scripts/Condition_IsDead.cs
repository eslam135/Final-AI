using UnityEngine;
using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("Banshee/Is Dead")]
public class Condition_IsDead : GOCondition
{
    private BansheeHealth healthScript;

    public override bool Check()
    {
        if (healthScript == null)
        {
            healthScript = gameObject.GetComponent<BansheeHealth>();
        }

        if (healthScript == null) return false;

        return healthScript.isDead;
    }
}