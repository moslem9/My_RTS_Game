using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public LayerMask layerMask;
    public LayerMask layerMask2;
    public Transform go;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SelectObject();
        }
        if (Input.GetMouseButton(0))  {
            MovePoint();
        }
        if (Input.GetMouseButtonUp(0)) {
            UnparentObject();
        }
    }

    public void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            Transform tr = hitInfo.collider.transform.parent;
            go.position = tr.position;
            tr.SetParent(go);
        }
    }

    public void MovePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask2))
        {
            go.position = hitInfo.point;
        }
    }

    private void UnparentObject()
    {
        if (go.childCount == 0)
            return;
        go.GetChild(0).SetParent(null);
    }

}
