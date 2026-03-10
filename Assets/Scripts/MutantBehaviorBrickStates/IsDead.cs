using UnityEngine;
using Pada1.BBCore;
using BBUnity.Conditions;
public class IsDead : GOCondition 
{
    private HealthView healthView;

    public override bool Check()
    {
        if (healthView == null)
            healthView = gameObject.GetComponent<HealthView>();

        if (healthView == null)
        {
            Debug.LogWarning("HealthView component not found!");
            return false;
        }

        return healthView.Health.currentHealth <= 0;
    }
}