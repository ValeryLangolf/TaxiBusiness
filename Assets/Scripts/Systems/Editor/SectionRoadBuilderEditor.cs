using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SectionRoadStrip))]
public class SectionRoadBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SectionRoadStrip builder = (SectionRoadStrip)target;

        if (GUILayout.Button("Применить"))
            builder.UpdateCount();
    }
}