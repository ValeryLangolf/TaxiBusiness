#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoadNetworkStorage : Editor
{
    private static List<RoadBuilder> s_roadBuilders = new();
    private static bool s_isInitialized;

    public static List<RoadBuilder> RoadBuilderList => new(s_roadBuilders);

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        if (s_isInitialized == false)
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
            EditorApplication.quitting += CleanUp;
            s_isInitialized = true;
        }

        UpdateStorageData();
    }

    private static void OnBeforeAssemblyReload() =>
        CleanUp();

    private static void OnAfterAssemblyReload()
    {
        s_isInitialized = false;
        Initialize();
    }

    private static void OnHierarchyChanged() =>
        UpdateStorageData();

    private static void UpdateStorageData()
    {
        UpdateRoadBuildersList();
        UpdateSections();
    }

    private static void UpdateRoadBuildersList() =>
        s_roadBuilders = FindObjectsByType<RoadBuilder>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

    private static void UpdateSections()
    {
        foreach (RoadBuilder network in s_roadBuilders)
            network.UpdateSections();        
    }

    private static void CleanUp()
    {
        if (s_isInitialized)
        {
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            EditorApplication.quitting -= CleanUp;
            s_isInitialized = false; 
        }
    }
}
#endif