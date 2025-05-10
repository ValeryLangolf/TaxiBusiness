#if UNITY_EDITOR

using UnityEditor;

public class RoadNetworkEditor : Editor
{
    private static bool _isShowGizmos = false;
    private static bool _isAutoFind = false;
    private static bool _isAutoConnect = false;

    [MenuItem("Дороги/Объединить в дорожную сеть")]
    private static void ConnectAllLanes()
    {
        foreach (RoadBuilder network in FindRoadBuilders())
            network.ConnectAllLanes();
    }

    [MenuItem("Дороги/Показать на сцене", false, 1)]
    private static void ToggleShowGizmos()
    {
        _isShowGizmos = !_isShowGizmos;
        EditorPrefs.SetBool("ShowGizmos", _isShowGizmos);
        UpdateGizmosFlag();
    }

    private static void UpdateGizmosFlag()
    {
        foreach (RoadBuilder network in FindRoadBuilders())
            network.SetGizmosFlag(_isShowGizmos);
    }

    [MenuItem("Дороги/Показать на сцене", true)]
    private static bool ValidateToggleShowGizmos()
    {
        _isShowGizmos = EditorPrefs.GetBool("ShowGizmos", false);
        Menu.SetChecked("Дороги/Показать на сцене", _isShowGizmos);

        return true;
    }

    [MenuItem("Дороги/Автопоиск секций и точек", false, 3)]
    private static void ToggleAutoFind()
    {
        _isAutoFind = !_isAutoFind;
        EditorPrefs.SetBool("AutoFind", _isAutoFind);
        UpdateAutoFindFlag();
    }

    private static void UpdateAutoFindFlag()
    {
        foreach (RoadBuilder network in FindRoadBuilders())
            network.SetAutoFindFlag(_isAutoFind);
    }

    [MenuItem("Дороги/Автопоиск секций и точек", true)]
    private static bool ValidateToggleAutoFind()
    {
        _isAutoFind = EditorPrefs.GetBool("AutoFind", false);
        Menu.SetChecked("Дороги/Автопоиск секций и точек", _isAutoFind);

        return true;
    }

    [MenuItem("Дороги/Автоконнект", false, 2)]
    private static void ToggleAutoConnect()
    {
        _isAutoConnect = !_isAutoConnect;
        EditorPrefs.SetBool("AutoConnect", _isAutoConnect);
        UpdateAutoConnectFlag();
    }

    private static void UpdateAutoConnectFlag()
    {
        RoadBuilder[] networks = FindObjectsByType<RoadBuilder>(UnityEngine.FindObjectsSortMode.None);

        foreach (RoadBuilder network in networks)
            network.SetAutoConnectFlag(_isAutoConnect);
    }

    [MenuItem("Дороги/Автоконнект", true)]
    private static bool ValidateToggleAutoConnect()
    {
        _isAutoConnect = EditorPrefs.GetBool("AutoConnect", false);
        Menu.SetChecked("Дороги/Автоконнект", _isAutoConnect);

        return true;
    }

    private static RoadBuilder[] FindRoadBuilders() =>
        FindObjectsByType<RoadBuilder>(UnityEngine.FindObjectsSortMode.None);
}

#endif