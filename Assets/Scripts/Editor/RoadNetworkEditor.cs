#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RoadNetworkEditor : Editor
{
    [MenuItem("Roads/Connect All Lanes")]
    static void ConnectAllLanes()
    {
        LaneComponent[] allLanes = FindObjectsByType<LaneComponent>(FindObjectsSortMode.None);

        foreach (LaneComponent lane in allLanes)
        {
            lane.AutoConnectLanes();
            EditorUtility.SetDirty(lane);
        }

        Debug.Log($"Автоматически соединено {allLanes.Length} полос!");
    }
}
#endif