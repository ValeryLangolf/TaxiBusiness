#if UNITY_EDITOR
using UnityEditor;

public class MenuDrawerSectionsAndPoints : Editor
{
    private const string MenuName = "Дороги/Показать на сцене";
    private const string Key = "ShowGizmos";
    private const int Priority = 1;
    private static bool _isOn;

    static MenuDrawerSectionsAndPoints()
    {
        _isOn = EditorPrefs.GetBool(Key, false);
    }

    [MenuItem(MenuName, false, Priority)]
    private static void Toggle()
    {
        _isOn = !_isOn;
        EditorPrefs.SetBool(Key, _isOn);
        UpdateFlag();

        if (_isOn)
            SceneView.duringSceneGui += OnUpdate;
        else
            SceneView.duringSceneGui -= OnUpdate;
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
        UpdateFlag();

        if (_isOn)
        {
            SceneView.duringSceneGui -= OnUpdate;
            SceneView.duringSceneGui += OnUpdate;
        }
    }

    private static void UpdateFlag()
    {
        foreach (RoadBuilder network in RoadNetworkStorage.RoadBuilderList)
            network.SetGizmosFlag(_isOn);
    }

    private static void OnUpdate(SceneView _)
    {
        foreach (RoadBuilder network in RoadNetworkStorage.RoadBuilderList)
            network.OnSceneGUI(null);
    }
}
#endif