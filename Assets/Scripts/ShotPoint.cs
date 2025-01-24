using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPoint : MonoBehaviour
{
	[SerializeField]
	float refireRate = 2f;
	[SerializeField]
	Ammo ammo;
	float fireTimer = 0;

	private void Update()
	{
		fireTimer += Time.deltaTime;
		if (fireTimer >= refireRate)
		{
			fireTimer = 0;
			Fire();
		}
	}

	private void Fire()
	{
		var shot = ammo.Get();
		shot.transform.position = transform.position;
		shot.transform.rotation = transform.rotation;
		shot.gameObject.SetActive(true);
	}
}
