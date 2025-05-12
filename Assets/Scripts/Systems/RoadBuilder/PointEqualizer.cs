#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointEqualizer
{
    private int _minCount;

    public PointEqualizer()
    {
        _minCount = SectionRoadBuilder.MinWaypointsCount;
    }

    public void EqualizeDistanceBetweenPoints(List<Transform> transforms, Vector3 arcValue)
    {
        if (transforms.Count <= _minCount)
            return;

        Vector3 firstPosition = transforms.First().position;
        Vector3 lastPosition = transforms.Last().position;

        float totalDistance = Vector3.Distance(firstPosition, lastPosition);
        float smoothFactor = Mathf.Clamp(totalDistance * 0.05f, 0.5f, 2.5f);
        Vector3 scaledArc = arcValue * smoothFactor;

        for (int i = 1; i < transforms.Count - 1; i++)
        {
            float t = (float)i / (transforms.Count - 1);
            Vector3 pointOnLine = Vector3.Lerp(firstPosition, lastPosition, t);

            float curveFactor = Mathf.Sin(t * Mathf.PI);
            Vector3 arcOffset = scaledArc * curveFactor;

            transforms[i].position = pointOnLine + arcOffset;
        }
    }
}
#endif