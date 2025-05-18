#if UNITY_EDITOR

using UnityEditor;

public class MenuWaypointAutoEqualizerEditor : Editor
{
    private const string MenuName = "Дороги/Автовыравнивание точек";
    private const string Key = "AutoDistance";
    private const int Priority = 4;

    private static bool _isAutoEqualize = false;

    static MenuWaypointAutoEqualizerEditor()
    {
        _isAutoEqualize = EditorPrefs.GetBool(Key, false);
    }

    [MenuItem(MenuName, false, Priority)]
    private static void ToggleAutoEqualize()
    {
        _isAutoEqualize = !_isAutoEqualize;
        EditorPrefs.SetBool(Key, _isAutoEqualize);

        if (_isAutoEqualize)
            EditorApplication.update += UpdateDistance;
        else
            EditorApplication.update -= UpdateDistance;
    }

    private static void UpdateDistance()
    {
        RoadBuilder[] networks = FindObjectsByType<RoadBuilder>(UnityEngine.FindObjectsSortMode.None);

        foreach (RoadBuilder network in networks)
            foreach (SectionRoadStrip section in network.Sections)
                if (section.TryGetComponent(out SectionRoadBuilder sectionRoadBuilder))
                    sectionRoadBuilder.UpdateCount();
    }

    [MenuItem(MenuName, true)]
    private static bool ValidateToggleAutoDistance()
    {
        _isAutoEqualize = EditorPrefs.GetBool(Key, false);
        Menu.SetChecked(MenuName, _isAutoEqualize);

        return true;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReload()
    {
        if (_isAutoEqualize)
        {
            EditorApplication.update -= UpdateDistance;
            EditorApplication.update += UpdateDistance;
        }
    }
}
#endif