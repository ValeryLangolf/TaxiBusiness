#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RoadNetworkEditor : Editor
{
    [MenuItem("Roads/Connect All Lanes")]
    static void ConnectAllLanes()
    {
        SectionRoadStrip[] allLanes = FindObjectsByType<SectionRoadStrip>(FindObjectsSortMode.None);

        foreach (SectionRoadStrip lane in allLanes)
        {
            lane.AutoConnectLanes();
            EditorUtility.SetDirty(lane);
        }

        Debug.Log($"Автоматически соединено {allLanes.Length} полос!");
    }
}
#endif