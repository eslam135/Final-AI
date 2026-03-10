using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("Conditions/Is Dead")]
public class IsDead : ConditionBase
{
    [InParam("Health")]
    public float health;

    public override bool Check()
    {
        return health <= 0;
    }
}