using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    void Update()
    {
       Vector3 dir = transform.position - Camera.main.transform.position ;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
