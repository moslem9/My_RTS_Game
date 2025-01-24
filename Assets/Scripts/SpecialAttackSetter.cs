using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackSetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
            transform.root.GetComponent<UnitBase>().SpecialAttack();
    }
}
