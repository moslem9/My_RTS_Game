using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
    public CancellationTokenSource tokenSource = new CancellationTokenSource();
    public float health;
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    public abstract void GenerateUnit();

    public void CreateTokenSource()
    {
        tokenSource = new CancellationTokenSource();
    }

    public void CancelateTokenSource()
    {
        tokenSource.Cancel();
    }
}
