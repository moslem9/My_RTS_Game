using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            Destroy(other.transform.root.gameObject, 5);
            GameplayController.instance.AddGold(3);
            GameplayController.instance.AddWood(3);
        }
    }
}
