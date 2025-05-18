using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHitInformer : MonoBehaviour
{
    public static event Action<Vector3> PlaneLeftClicked;
    public static event Action<Collider, Vector3> RightHitted;
    public static event Action<Passenger> PassengerClicked;
    public static event Action<Vehicle> VehicleClicked;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            HandleUi();
        else
            HandleClickObject();
    }

    private void HandleUi()
    {
        if (Input.GetMouseButtonDown(0) == false)
            return;

        GameObject uiObject = EventSystem.current.currentSelectedGameObject;

        if (uiObject != null && uiObject.transform.parent.TryGetComponent(out Passenger passenger))
            PassengerClicked?.Invoke(passenger);
    }

    private void HandleClickObject()
    {
        if (Input.GetMouseButtonDown(0))
            HandleLeftClickObject();

        if (Input.GetMouseButtonDown(1))
            HandleRightClickObject();
    }

    private void HandleLeftClickObject()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) == false)
            return;

        if (hit.collider.TryGetComponent(out Plane _))
            PlaneLeftClicked?.Invoke(hit.point);

        if (hit.collider.TryGetComponent(out VehicleCollider vehicleCollider))
            VehicleClicked?.Invoke(vehicleCollider.Vehicle);
    }

    private void HandleRightClickObject()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            RightHitted?.Invoke(hit.collider, hit.point);
    }
}