#if UNITY_EDITOR
using UnityEditor;

public class RoadNetworkEditor : Editor
{
    [MenuItem("������/���������� � �������� ����")]
    private static void ConnectAllLanes()
    {
        RoadNetwork[] networks = FindObjectsByType<RoadNetwork>(UnityEngine.FindObjectsSortMode.None);

        foreach (RoadNetwork network in networks)
            network.ConnectAllLanes();
    }
}
#endif