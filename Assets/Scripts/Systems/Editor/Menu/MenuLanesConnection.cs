#if UNITY_EDITOR
using UnityEditor;

public class MenuLanesConnection : Editor
{
    private const string MenuName = "Дороги/Автоконнект (прожорливая опция)";
    private const string Key = "AutoConnect";
    private const int Priority = 4;
    private static bool _isOn;

    static MenuLanesConnection()
    {
        _isOn = EditorPrefs.GetBool(Key, false);
    }

    [MenuItem(MenuName, false, Priority)]
    private static void Toggle()
    {
        _isOn = !_isOn;
        EditorPrefs.SetBool(Key, _isOn);

        if (_isOn)
            EditorApplication.update += OnUpdate;
        else
            EditorApplication.update -= OnUpdate;
    }

    [MenuItem(MenuName, true)]
    private static bool ValidateToggle()
    {
        _isOn = EditorPrefs.GetBool(Key, false);
        Menu.SetChecked(MenuName, _isOn);

        return true;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReload()
    {
        if (_isOn)
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }
    }

    private static void OnUpdate()
    {
        foreach (RoadNetwork network in RoadNetworkStorage.RoadBuilderList)
            network.ConnectPoints();
    }
}
#endif