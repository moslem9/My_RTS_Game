using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;

    public UnitBase[] units;
    public BuildingBase[] buildings;

    public static EnemySpawner instance;

    public event Action OnDestroyedPlayerObject;

    private void Awake()
    {
        instance = this;
    }

    public bool UpdateUnitAndBuildingList() {
        units = FindObjectsOfType<UnitBase>();
        buildings = FindObjectsOfType<BuildingBase>();
        OnDestroyedPlayerObject?.Invoke();
        return true;
    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomPoint = new Vector3(transform.position.x + UnityEngine.Random.Range(-50, 50)
                , 0, transform.position.z + UnityEngine.Random.Range(0, 50));
            Instantiate(prefabsToSpawn[UnityEngine.Random.Range(0, prefabsToSpawn.Length)], randomPoint, Quaternion.identity);
        }
    }
}
