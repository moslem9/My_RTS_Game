using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    public List<GameObject> aimObjects;
    int index = -1;

    private void Start()
    {
        health = Random.Range(5, 20);

        EnemySpawner.instance.UpdateUnitAndBuildingList();

        UpdateAimObjectList();
    }

    public void UpdateAimObjectList()
    {

        aimObjects = new List<GameObject>();

        foreach (BuildingBase building in EnemySpawner.instance.buildings)
            aimObjects.Add(building.gameObject);

        foreach (UnitBase unit in EnemySpawner.instance.units)
            aimObjects.Add(unit.gameObject);

        ChangeAimingIndex();
    }

    private void OnEnable()
    {
        EnemySpawner.instance.OnDestroyedPlayerObject += UpdateAimObjectList;
        GameplayController.instance.CreatedEnemyUnit();

    }

    private void OnDisable()
    {
        EnemySpawner.instance.OnDestroyedPlayerObject -= UpdateAimObjectList;
        GameplayController.instance.DeadEnemyUnit();
    }

    public float health = 5f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (index != -1)
        {
            if (aimObjects.Count < 1)
                return;
            LookAtAimObject(aimObjects[index]);
        }
    }

    public void ChangeAimingIndex()
    {
        index = Random.Range(0, aimObjects.Count);
    }

    public void LookAtAimObject(GameObject aimObject)
    {
        if (gameObject == null)
            return;
        Vector3 direction = aimObject.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnDestroy()
    {
        GameplayController.instance.DeadEnemyUnit();
        GameplayController.instance.AddGold(Random.Range(1, 5));
        GameplayController.instance.AddWood(Random.Range(1, 5));
        GameplayController.instance.CheckWinning();
    }
}
