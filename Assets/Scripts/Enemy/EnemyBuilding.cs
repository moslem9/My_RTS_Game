using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyBuilding : MonoBehaviour
{
	public GameObject enemyPrefab;
	CancellationTokenSource tokenSource = new CancellationTokenSource();

	void Start()
    {
		health = Random.Range(20, 40);

		GenerateEnemyByTask();
	}

    public void GenerateEnemyByTask()
	{
		var token = tokenSource.Token;
		GenerateEnemyByTask(Random.Range(20, 30), enemyPrefab, "Enemy Generated!", token);
	}

	public async void GenerateEnemyByTask(float duration, GameObject go, string message, CancellationToken token)
	{
		await Task.Delay((int)(duration * 1000));

		if (token.IsCancellationRequested)
			return;

		Vector3 randomPoint = new Vector3(transform.position.x + Random.Range(-50, 50)
				  , 0, transform.position.z + Random.Range(0, 60));
		Instantiate(go, randomPoint, Quaternion.identity);
		GenerateEnemyByTask(duration, go, message, token);
	}

	private void OnDestroy()
	{
		
		GameplayController.instance.DestroyedEnemyBuilding();
		GameplayController.instance.AddGold(Random.Range(3, 7));
		GameplayController.instance.AddWood(Random.Range(3, 7));
		GameplayController.instance.CheckWinning();
		tokenSource.Cancel();
	}

	public float health = 10f;

	public void TakeDamage(float damage) {
		health -= damage;

		if (health <= 0f)
			Destroy(gameObject);
	}

    private void OnEnable()
    {
		GameplayController.instance.CreatedEnemyBuilding();
	}

    private void OnDisable()
    {
		GameplayController.instance.DestroyedEnemyBuilding();
	}
}
