using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public List<Transform> FindPath(PointInRoadSection startPoint, PointInRoadSection targetPoint)
    {
        if (startPoint.Section == targetPoint.Section && startPoint.Position == targetPoint.Position)
            return new List<Transform> { startPoint.Point };

        PriorityQueue<PathNode> openSet = new();
        Dictionary<SectionRoadStrip, SectionRoadStrip> cameFrom = new();
        Dictionary<SectionRoadStrip, float> gScore = new();

        openSet.Enqueue(new PathNode(startPoint.Section, Utils.CalculateDistance(startPoint.Position, targetPoint.Position)));
        gScore[startPoint.Section] = 0f;

        while (openSet.Count > 0)
        {
            PathNode current = openSet.Dequeue();

            if (current.Lane == targetPoint.Section)
                return ReconstructPath(cameFrom, current.Lane, startPoint, targetPoint);

            foreach (SectionRoadStrip neighbor in current.Lane.ConnectedLanes)
            {
                float tentativeGScore = gScore[current.Lane];

                if (!gScore.ContainsKey(neighbor))
                    gScore[neighbor] = Mathf.Infinity;

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current.Lane;
                    gScore[neighbor] = tentativeGScore;
                    float fScore = tentativeGScore + Utils.CalculateDistance(neighbor, targetPoint.Section);

                    if (!openSet.Contains(node => node.Lane == neighbor))
                        openSet.Enqueue(new PathNode(neighbor, fScore));
                }
            }
        }

        return new List<Transform>();
    }

    private List<Transform> ReconstructPath(Dictionary<SectionRoadStrip, SectionRoadStrip> cameFrom, SectionRoadStrip current, PointInRoadSection startPoint, PointInRoadSection targetPoint)
    {
        List<Transform> totalPath = new() { targetPoint.Point }; // Начинаем с целевой точки

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(GetPointOnSection(current)); // Получите точку на секции
        }

        totalPath.Reverse();
        totalPath.Insert(0, startPoint.Point); // Добавьте начальную точку в начало списка
        return totalPath;
    }

    private Transform GetPointOnSection(SectionRoadStrip section) =>
        section.Points[section.Points.Count / 2];

    private class PathNode : IComparable<PathNode>
    {
        public SectionRoadStrip Lane { get; }

        public float FScore { get; }

        public PathNode(SectionRoadStrip lane, float fScore)
        {
            Lane = lane;
            FScore = fScore;
        }

        public int CompareTo(PathNode other) => 
            FScore.CompareTo(other.FScore);
    }
}