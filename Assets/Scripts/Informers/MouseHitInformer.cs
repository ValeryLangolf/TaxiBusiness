using System;
using UnityEngine;

public class MouseHitInformer : MonoBehaviour
{
    public static event Action<Collider, Vector3> LeftHitted;
    public static event Action<Collider, Vector3> RightHitted;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                LeftHitted?.Invoke(hit.collider, hit.point);

        if (Input.GetMouseButtonDown(1))
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                RightHitted?.Invoke(hit.collider, hit.point);
    }
}