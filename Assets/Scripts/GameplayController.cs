using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;
    public GameObject vicrotyTextParent;
    public TMP_Text infoText;

    Dictionary<int, Race[]> races = new Dictionary<int, Race[]>();

    public Transform unitActionButtonParent;
    public Transform buildingActionButtonParent;

    [HideInInspector]
    public int wood;
    public TMP_Text woodText;

    [HideInInspector]
    public int gold;
    public TMP_Text goldText;

    bool isGameOver;

    public event Action<GameObject> OnCreatedBuilding;
    public event Action<GameObject> OnDeadBuilding;

    public event Action<GameObject> OnCreatedUnit;
    public event Action<GameObject> OnDeadUnit;

    public EnemyArcher[] enemyArchers;
    public EnemyBuilding[] enemyBuildings;

    public void AddGold(int count) {
        gold += count;
        goldText.text = $"{gold}";
    }

    public void ShowingInfoText(string message) {
        StartCoroutine(ShowingInfoTextCoroutine(message));
    }

    IEnumerator ShowingInfoTextCoroutine(string message) {
        infoText.text = message;
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        infoText.gameObject.SetActive(false);
    }

    public bool RemoveGoldAndWood(int goldCount, int woodCount)
    {
        if (CheckIfGoldIsEnough(goldCount)&& CheckIfWoodIsEnough(woodCount))
        {
            gold -= goldCount;
            goldText.text = $"{gold}";

            wood -= woodCount;
            woodText.text = $"{wood}";

            return true;
        }
        else
            return false;
    }

    public bool RemoveGold(int count)
    {
        if (CheckIfGoldIsEnough(count))
        {
            gold -= count;
            goldText.text = $"{gold}";
            return true;
        }else
            return false;
    }

    public void AddWood(int count)
    {
        wood += count;
        woodText.text = $"{wood}";
    }

    public bool RemoveWood(int count)
    {
        if (CheckIfWoodIsEnough(count))
        {
            wood -= count;
            woodText.text = $"{wood}";
            return true;
        }
        else
            return false;
    }

    public bool CheckIfWoodIsEnough(int amount)
    {
        bool b;
        int r = wood - amount;
        if (r > 0)
            b = true;
        else
            b = false;
        return b;
    }

    public bool CheckIfGoldIsEnough(int amount)
    {
        bool b;
        int r = gold - amount;
        if (r > 0)
            b = true;
        else
            b = false;
        return b;
    }


    public event Action OnCheckPlayerWinning;
    public int playerObjectsCount;

    public void CheckWinning() {
        OnCheckPlayerWinning?.Invoke();
    }

    public void CheckPlayerWinning()
    {

        if (enemyArchers.Length < 1 && enemyBuildings.Length < 1 && !isGameOver)
        {
            isGameOver = true;
            vicrotyTextParent.SetActive(true);
            vicrotyTextParent.GetComponentInChildren<TMP_Text>().text = "CONGRADULATION, YOU WON!";
        }

        if (EnemySpawner.instance.units.Length < 1 && EnemySpawner.instance.buildings.Length < 1 && !isGameOver)
        {
            isGameOver = true;
            vicrotyTextParent.SetActive(true);
            vicrotyTextParent.GetComponentInChildren<TMP_Text>().text = "HA HA HA YOU LOST!";
        }
    }

    private void OnEnable()
    {
        OnCheckPlayerWinning += CheckPlayerWinning;
        OnEnemyBuildingDestroyed += UpdateEnemyBuildingLists;
        OnEnemyUnitDead += UpdateEnemyUnitLists;
        OnEnemyBuildingCreated += UpdateEnemyBuildingLists;
        OnEnemyUnitCreated += UpdateEnemyUnitLists;
    }

    private void OnDisable()
    {
        OnCheckPlayerWinning -= CheckPlayerWinning;
        OnEnemyBuildingDestroyed -= UpdateEnemyBuildingLists;
        OnEnemyUnitDead -= UpdateEnemyUnitLists;
        OnEnemyBuildingCreated -= UpdateEnemyBuildingLists;
        OnEnemyUnitCreated -= UpdateEnemyUnitLists;
    }

    public event Action OnEnemyBuildingDestroyed;
    public event Action OnEnemyUnitDead;

    public void DestroyedEnemyBuilding() {
        OnEnemyBuildingDestroyed?.Invoke();
    }

    public void DeadEnemyUnit(){
        OnEnemyUnitDead?.Invoke();
    }

    public event Action OnEnemyBuildingCreated;
    public event Action OnEnemyUnitCreated;

    public void CreatedEnemyBuilding()
    {
        OnEnemyBuildingCreated?.Invoke();
    }

    public void CreatedEnemyUnit()
    {
        OnEnemyUnitCreated?.Invoke();
    }

    public void UpdateEnemyUnitLists()
    {
        enemyArchers = FindObjectsOfType<EnemyArcher>();
    }

    public void UpdateEnemyBuildingLists()
    {
        enemyBuildings = FindObjectsOfType<EnemyBuilding>();
    }

    public void CreatedBuilding(GameObject go) {
        OnCreatedBuilding?.Invoke(go);
    }
    public void DeadBuilding(GameObject go)
    {
        OnDeadBuilding?.Invoke(go);
    }
    public void CreatedUnit(GameObject go)
    {
        OnCreatedUnit?.Invoke(go);
    }
    public void DeadUnit(GameObject go)
    {
        OnDeadUnit?.Invoke(go);
    }

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        races.Add(0, FindObjectsOfType<Human>());
        races.Add(1, FindObjectsOfType<Orc>());
    }

    public void ChangeRace(int i) {
        AssignRace(races[i]);
    }

    public void AssignRace(Race[] race)
    {
        RemoveButtonContainer();

        for (int i = 0; i < race.Length; i++)
        {
            race[i].InitRace();
        }
    }

    void RemoveButtonContainer() {
        if (unitActionButtonParent.childCount > 0)
            for (int i = 0; i < unitActionButtonParent.childCount; i++)
                Destroy(unitActionButtonParent.GetChild(i).gameObject);

        if (buildingActionButtonParent.childCount > 0)
            for (int i = 0; i < buildingActionButtonParent.childCount; i++)
                Destroy(buildingActionButtonParent.GetChild(i).gameObject);
    }
}
