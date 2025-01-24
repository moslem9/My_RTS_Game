using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HumanWorkerUnit : UnitBase
{
    public delegate void AttackMethod();
    AttackMethod attackMethod;
    public GameObject landminePrefab;
    float offset;

    private void Start()
    {
        offset = landminePrefab.transform.localScale.x;
        StartCoroutine(InitAttackCoroutine());
        EnemySpawner.instance.UpdateUnitAndBuildingList();
    }

    IEnumerator InitAttackCoroutine()
    {
        yield return new WaitForSeconds(7);
        Attack();
    }

    void OnEnable()
    {
        GameplayController.instance.CreatedUnit(gameObject);
    }

    void OnDisable()
    {
        GameplayController.instance.DeadUnit(gameObject);
    }

    private void OnDestroy()
    {
        if (EnemySpawner.instance.UpdateUnitAndBuildingList())
            GameplayController.instance.CheckWinning();

        CancelateTokenSource();
    }

    public override void Attack()
    {
        var token = tokenSource.Token;
        attackMethod = GenerateLandmine;
        GenerateAttackByTask(20, attackMethod, token);
    }

    int i = 0;
    public void GenerateLandmine()
    {
        if (i == 5)
            return;
        Vector3 randomPoint = transform.position + i * offset * transform.forward;
        i++;
        Instantiate(landminePrefab, randomPoint, Quaternion.identity);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public async void GenerateAttackByTask(float duration, AttackMethod attackMethod, CancellationToken token)
    {
        await Task.Delay((int)(duration * 1000));
        if (token.IsCancellationRequested)
            return;

        attackMethod();
        GenerateAttackByTask(duration, attackMethod, token);
    }
}
