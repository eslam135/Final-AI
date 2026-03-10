using System;
using UnityEngine;

public interface IHealth
{
    float currentHealth{get;}
    float maxHealth{get;}
    void TakeHeal(float amount);
    bool IsAlive{get;}
}
