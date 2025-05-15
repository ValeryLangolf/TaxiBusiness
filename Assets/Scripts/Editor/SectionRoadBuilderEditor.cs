using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SectionRoadBuilder))]
public class SectionRoadBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SectionRoadBuilder builder = (SectionRoadBuilder)target;

        if (GUILayout.Button("Применить"))
            builder.UpdateCount();
    }
}