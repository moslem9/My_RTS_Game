using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class OrcTownBuilding : BuildingBase
{
    public GameObject workerPrefab;
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
        unitBuilderMethod = HumanWorker;
        GenerateByTask(20, unitBuilderMethod, spawnPoints, token);
    }

    int i = 0;

    public void HumanWorker(bool asButton, Transform[] spawnPoints)
    {
        if (i == 5)
            return;

        GameObject go = Instantiate(workerPrefab
            , spawnPoints[1].position - 6 * i * transform.right, Quaternion.identity);
        go.GetComponentInChildren<TMP_Text>().text = "Peon";
        go.GetComponent<OrcWorkerUnit>().enabled = true;

        GameObject go2 = Instantiate(workerPrefab
           , spawnPoints[0].position + 6 * i * transform.right, Quaternion.identity);
        go2.GetComponentInChildren<TMP_Text>().text = "Peon";
        go2.GetComponent<OrcWorkerUnit>().enabled = true;

        i++;
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
