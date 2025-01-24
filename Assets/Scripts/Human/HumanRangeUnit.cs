using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HumanRangeUnit : UnitBase
{
    public int index = -1;

    EnemyBuilding enemyBuilding;

    public delegate void AttackMethod();
    AttackMethod attackMethod;

    private void Start()
    {
        StartCoroutine(InitAttackCoroutine());
        EnemySpawner.instance.UpdateUnitAndBuildingList();
    }
    private void OnDestroy()
    {
        if (EnemySpawner.instance.UpdateUnitAndBuildingList())
            GameplayController.instance.CheckWinning();

        CancelateTokenSource();
    }

    IEnumerator InitAttackCoroutine()
    {
        yield return new WaitForSeconds(7);
        index = UnityEngine.Random.Range(0, GameplayController.instance.enemyBuildings.Length);
        if (GameplayController.instance.enemyBuildings.Length > 0)
            enemyBuilding = GameplayController.instance.enemyBuildings[index];
        Attack();
    }

    void OnEnable()
    {
        GameplayController.instance.OnEnemyBuildingDestroyed += ChangeEnemyIndexToAttack;
        GameplayController.instance.CreatedUnit(gameObject);
    }

    void OnDisable()
    {
        GameplayController.instance.OnEnemyBuildingDestroyed -= ChangeEnemyIndexToAttack;
        GameplayController.instance.DeadUnit(gameObject);
    }

    void ChangeEnemyIndexToAttack()
    {
        index = -1;
        CancelateTokenSource();
        CreateTokenSource();
        StartCoroutine(InitAttackCoroutine());
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private void Update()
    {
        if (index != -1)
        {
            if (enemyBuilding == null)
                return;

            transform.position = Vector3.Lerp(transform.position
                , enemyBuilding.transform.position - 20 * transform.forward, speed);
        }
    }

    public override void Attack()
    {
        //Berserker and Archer Attack
        BerserkerAndArcherAttack();
    }

    public void BerserkerAndArcherAttack()
    {
        var token = tokenSource.Token;

        if (GameplayController.instance.enemyBuildings.Length < 1)
            return;

        if (enemyBuilding == null)
            return;

        transform.LookAt(enemyBuilding.transform);
        attackMethod = ArcherFire;

        GenerateAttackByTask(5, attackMethod, token);
    }

    void ArcherFire()
    {
        Debug.Log("Archer Firing...");
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
