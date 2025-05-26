#if UNITY_EDITOR

using UnityEditor;

public class RoadNetworkEditor : Editor
{
    [MenuItem("Дороги/Объединить в дорожную сеть")]
    private static void ConnectAllLanes()
    {
        foreach (RoadBuilder network in RoadNetworkStorage.RoadBuilderList)
            network.ConnectAllLanes();
    }
}
#endif