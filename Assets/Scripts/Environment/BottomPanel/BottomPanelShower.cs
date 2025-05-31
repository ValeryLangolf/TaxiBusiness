using UnityEngine;

public class BottomPanelShower : MonoBehaviour
{
    public void Show() =>
        transform.localScale = Vector3.one;

    public void Hide() =>
        transform.localScale = Vector3.zero;
}