using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class OrcMeleeUnit : UnitBase
{
    public int index = -1;
    public Animator meleeAnimator;

    EnemyArcher enemyArcher;

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
        index = UnityEngine.Random.Range(0, GameplayController.instance.enemyArchers.Length);
        if (GameplayController.instance.enemyArchers.Length > 0)
            enemyArcher = GameplayController.instance.enemyArchers[index];
        Attack();
    }

    void OnEnable()
    {
        GameplayController.instance.OnEnemyUnitDead += ChangeEnemyIndexToAttack;
        GameplayController.instance.CreatedUnit(gameObject);
    }

    void OnDisable()
    {
        GameplayController.instance.OnEnemyUnitDead -= ChangeEnemyIndexToAttack;
        GameplayController.instance.DeadUnit(gameObject);
    }

    void ChangeEnemyIndexToAttack()
    {
        index = -1;
        CancelateTokenSource();
        CreateTokenSource();
        meleeAnimator.Play("idle");
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
            if (enemyArcher == null)
                return;

            transform.position = Vector3.Lerp(transform.position
                , enemyArcher.transform.position - 3 * transform.forward, speed);
        }
    }

    public override void Attack()
    {
        //Grunt and Footman Attack
        GruntAndFootmanAttack();
    }

    public void GruntAndFootmanAttack()
    {
        var token = tokenSource.Token;
        if (GameplayController.instance.enemyArchers.Length < 1)
            return;

        if (enemyArcher == null)
            return;

        transform.LookAt(enemyArcher.transform);
        attackMethod = MeleeSlash;

        GenerateAttackByTask(2, attackMethod, token);
    }

    void MeleeSlash()
    {
        if (meleeAnimator == null)
            return;

        meleeAnimator.SetTrigger("slash");
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