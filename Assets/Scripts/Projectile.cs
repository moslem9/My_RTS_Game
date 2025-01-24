using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Ammo ammo;
	public Ammo Ammo
	{
		get { return ammo; }
		set
		{
			if (ammo == null)
				ammo = value;
		}
	}

	public float moveSpeed = 30f;
	private float lifeTime;
	private float maxLifeTime = 5f;

	private void OnEnable()
	{
		lifeTime = 0;
	}

	private void Update()
	{
		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		lifeTime += Time.deltaTime;
		if (lifeTime > maxLifeTime)
		{
			ammo.ReturnToPool(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("enemy"))
		{
			if (other.transform.parent.TryGetComponent<EnemyBuilding>(out EnemyBuilding enemyBuilding))
			{
				enemyBuilding.TakeDamage(1);
				ammo.ReturnToPool(gameObject);
			}

			if (other.transform.parent.TryGetComponent<EnemyArcher>(out EnemyArcher enemyArcher))
			{
				enemyArcher.TakeDamage(1);
				ammo.ReturnToPool(gameObject);
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("selectable"))
		{
			if (other.transform.parent.TryGetComponent<UnitBase>(out UnitBase unitBase))
			{
				unitBase.TakeDamage(1);
				ammo.ReturnToPool(gameObject);
			}

			if (other.transform.parent.TryGetComponent<BuildingBase>(out BuildingBase buildingBase))
			{
				buildingBase.TakeDamage(1);
				ammo.ReturnToPool(gameObject);
			}
		}

	}
}
