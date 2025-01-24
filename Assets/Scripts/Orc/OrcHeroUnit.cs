using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class OrcHeroUnit : UnitBase
{
    public int index = -1;
    public Animator heroAnimator;
    public GameObject orcGunPrefab;
    public GameObject orcBulletPrefab;

    EnemyArcher enemyArcher;

    public delegate void AttackMethod();
    AttackMethod attackMethod;

    private void Start()
    {
        StartCoroutine(InitAttackCoroutine());
        EnemySpawner.instance.UpdateUnitAndBuildingList();
    }

    IEnumerator InitAttackCoroutine()
    {
        yield return new WaitForSeconds(4);
        index = UnityEngine.Random.Range(0, GameplayController.instance.enemyArchers.Length);
        if (GameplayController.instance.enemyArchers.Length > 0)
            enemyArcher = GameplayController.instance.enemyArchers[index];
        Attack();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }

    public override void Attack()
    {
        //Blademaster and Paladin Attack and Special Attack
        PaladinAndBlademasterAttack();
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
        RemoveAllHeroGuns();
        CancelateTokenSource();
        CreateTokenSource();
        heroAnimator.Play("idle");
        StartCoroutine(InitAttackCoroutine());
    }

    public void PaladinAndBlademasterAttack()
    {
        var token = tokenSource.Token;

        if (GameplayController.instance.enemyArchers.Length < 1)
            return;

        if (enemyArcher == null)
            return;

        transform.LookAt(enemyArcher.transform);
        attackMethod = HeroSlash;
        GenerateAttackByTask(2, attackMethod, token);
    }

    public override void SpecialAttack()
    {
        var token = tokenSource.Token;
        attackMethod = HeroFire;
        GenerateHeroAttackByTask(2, attackMethod, token);
    }

    void HeroSlash()
    {
        if (heroAnimator == null)
            return;

        heroAnimator.SetTrigger("slash");
    }

    void HeroFire()
    {
        for (int i = 0; i < GameplayController.instance.enemyArchers.Length; i++)
        {
            if (GameplayController.instance.enemyArchers[i] == null)
                continue;
            if (heroObjects == null)
                return;
            GameObject go = Instantiate(orcGunPrefab, heroObjects.transform.GetChild(1));
            go.transform.LookAt(GameplayController.instance.enemyArchers[i].transform.GetChild(0));
            go.GetComponent<Ammo>().prefab = orcBulletPrefab;
        }
    }

    private void RemoveAllHeroGuns()
    {
        Ammo[] ammos = heroObjects.transform.GetChild(1).GetComponentsInChildren<Ammo>();

        for (int i = 0; i < ammos.Length; i++)
        {
            Destroy(ammos[i].gameObject);
        }
    }

    public async void GenerateAttackByTask(float duration, AttackMethod attackMethod, CancellationToken token)
    {
        await Task.Delay((int)(duration * 1000));

        if (token.IsCancellationRequested)
            return;

        attackMethod();
        GenerateAttackByTask(duration, attackMethod, token);
    }

    public async void GenerateHeroAttackByTask(float duration, AttackMethod attackMethod, CancellationToken token)
    {
        await Task.Delay((int)(duration * 1000));
        if (token.IsCancellationRequested)
            return;
        attackMethod();
    }

    private void OnDestroy()
    {
        if (EnemySpawner.instance.UpdateUnitAndBuildingList())
            GameplayController.instance.CheckWinning();
        CancelateTokenSource();
    }
}
