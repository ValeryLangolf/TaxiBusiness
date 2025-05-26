#if UNITY_EDITOR

using UnityEditor;

public class MenuWaypointAutoEqualizerEditor : Editor
{
    private const string MenuName = "Дороги/Автовыравнивание точек";
    private const string Key = "AutoDistance";
    private const int Priority = 4;
    private static bool _isOn;

    static MenuWaypointAutoEqualizerEditor()
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
        RoadBuilder[] networks = FindObjectsByType<RoadBuilder>(UnityEngine.FindObjectsSortMode.None);

        foreach (RoadBuilder network in networks)
            foreach (SectionRoadStrip section in network.Sections)
                if (section.TryGetComponent(out SectionRoadBuilder sectionRoadBuilder))
                    sectionRoadBuilder.UpdateCount();
    }
}
#endif