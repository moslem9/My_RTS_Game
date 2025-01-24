using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public float speed = 0.3f;
    public GameObject rangeObjects, meleeObjects, heroObjects;

    public CancellationTokenSource tokenSource = new CancellationTokenSource();

    public float health;
    [Range(1,5)] public float armor = 1f;

    public abstract void Attack();
    public virtual void SpecialAttack() { }

    public virtual void TakeDamage(float damage)
    {
        health -= damage / armor;
        if (health <= 0)
            Destroy(gameObject);
    }

    public void CreateTokenSource() { 
        tokenSource = new CancellationTokenSource();
    }

    public void CancelateTokenSource() {
        tokenSource.Cancel();
    }
}
