using UnityEngine;
using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("Banshee/Check Distance Range")]
public class Condition_DistanceRange : GOCondition
{
    [InParam("target")] public Transform target;
    [InParam("minDistance")] public float minDistance = 0f;
    [InParam("maxDistance")] public float maxDistance = 5f;

    public override bool Check()
    {
        if (target == null) return false;

        Vector3 myFlatPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        Vector3 targetFlatPos = new Vector3(target.position.x, 0, target.position.z);

        float currentDistance = Vector3.Distance(myFlatPos, targetFlatPos);

        return (currentDistance >= minDistance && currentDistance <= maxDistance);
    }
}