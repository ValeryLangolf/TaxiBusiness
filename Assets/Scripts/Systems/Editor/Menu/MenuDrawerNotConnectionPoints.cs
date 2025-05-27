using UnityEditor;
using UnityEngine;

public class MenuDrawerNotConnectionPoints : MonoBehaviour
{
    private const string MenuName = "Дороги/Показывать точки без соединений (прожорливая опция)";
    private const string Key = "ShowNotConnectPoints";
    private const int Priority = 2;
    private static bool _isOn;

    static MenuDrawerNotConnectionPoints()
    {
        _isOn = EditorPrefs.GetBool(Key, false);
    }

    [MenuItem(MenuName, false, Priority)]
    private static void Toggle()
    {
        _isOn = !_isOn;
        EditorPrefs.SetBool(Key, _isOn);
        UpdateFlag();
    }

    [MenuItem(MenuName, true)]
    private static bool ValidateToggle()
    {
        _isOn = EditorPrefs.GetBool(Key, false);
        Menu.SetChecked(MenuName, _isOn);

        return true;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReload() =>
        UpdateFlag();

    private static void UpdateFlag()
    {
        foreach (RoadNetwork network in RoadNetworkStorage.RoadBuilderList)
            network.SetFlagNotConnectPoints(_isOn);
    }
}