using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
	public GameObject prefab;
	private Queue<GameObject> objects = new Queue<GameObject>();

	public GameObject Get()
	{
		if (objects.Count == 0)
			AddObject();

		return objects.Dequeue();
	}

	public void ReturnToPool(GameObject objectToReturn)
	{
		objectToReturn.SetActive(false);
		objects.Enqueue(objectToReturn);
	}

	void AddObject()
	{
		GameObject newObject = Instantiate(prefab);
		newObject.gameObject.SetActive(false);
		objects.Enqueue(newObject);
		newObject.GetComponent<Projectile>().Ammo = this;
	}
}
