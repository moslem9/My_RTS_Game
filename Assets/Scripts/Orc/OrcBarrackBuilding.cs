using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class OrcBarrackBuilding : BuildingBase
{
    public GameObject meleePrefab, rangePrefab;
    public Transform[] spawnPoints;

    public delegate void UnitBuilder(bool asButton, Transform[] spawnPoints);
    UnitBuilder unitBuilderMethod;

    private void Start()
    {
        EnemySpawner.instance.UpdateUnitAndBuildingList();
        GenerateUnit();
    }

    private void OnDestroy()
    {
        GameplayController.instance.DeadBuilding(gameObject);
        if (EnemySpawner.instance.UpdateUnitAndBuildingList())
            GameplayController.instance.CheckWinning();

        CancelateTokenSource();
    }
    void OnEnable()
    {
        GameplayController.instance.CreatedBuilding(gameObject);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    void OnDisable()
    {
        GameplayController.instance.DeadBuilding(gameObject);
    }

    public override void GenerateUnit()
    {
        var token = tokenSource.Token;
        unitBuilderMethod = HumanArcherAndFootman;
        GenerateByTask(20, unitBuilderMethod, spawnPoints, token);
    }

    public void HumanArcherAndFootman(bool asButton, Transform[] spawnPoints)
    {
        GameObject go = Instantiate(meleePrefab
            , spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        go.GetComponentInChildren<TMP_Text>().text = "Grunt";
        go.GetComponent<OrcMeleeUnit>().enabled = true;
        go.GetComponent<OrcMeleeUnit>().meleeObjects.SetActive(true);

        GameObject go2 = Instantiate(rangePrefab
          , spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        go2.GetComponentInChildren<TMP_Text>().text = "Berserker";
        go2.GetComponent<OrcRangeUnit>().enabled = true;
        go2.GetComponent<OrcRangeUnit>().rangeObjects.SetActive(true);
    }

    public async void GenerateByTask(float duration, UnitBuilder unitBuilderMethod
        , Transform[] spawnPoints, CancellationToken token)
    {

        await Task.Delay((int)(duration * 1000));
        if (token.IsCancellationRequested)
            return;

        unitBuilderMethod(false, spawnPoints);
        GenerateByTask(duration, unitBuilderMethod, spawnPoints, token);
    }
}
