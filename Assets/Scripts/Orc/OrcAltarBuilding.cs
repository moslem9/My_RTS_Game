using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class OrcAltarBuilding : BuildingBase
{
    public GameObject heroPrefab;
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
        unitBuilderMethod = HumanHero;
        GenerateByTask(20, unitBuilderMethod, spawnPoints, token);
    }

    public void HumanHero(bool asButton, Transform[] spawnPoints) {
        GameObject go = Instantiate(heroPrefab
            , spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        go.GetComponentInChildren<TMP_Text>().text = "Blademaster";
        go.GetComponent<OrcHeroUnit>().enabled = true;
        go.GetComponent<OrcHeroUnit>().heroObjects.SetActive(true);
    }

    public async void GenerateByTask(float duration, UnitBuilder unitBuilderMethod
        , Transform[] spawnPoints, CancellationToken token) {

        await Task.Delay((int)(duration * 1000));
        if (token.IsCancellationRequested)
            return;

        unitBuilderMethod(false, spawnPoints);
        GenerateByTask(duration, unitBuilderMethod, spawnPoints, token);
    }
}
