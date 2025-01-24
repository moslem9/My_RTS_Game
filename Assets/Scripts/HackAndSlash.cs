using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackAndSlash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemy"))
            if (other.transform.parent.TryGetComponent<EnemyArcher>(out EnemyArcher enemyArcher))
                enemyArcher.TakeDamage(1);
    }
}
