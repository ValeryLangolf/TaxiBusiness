using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private RoadManager _roadManager;

    public List<SectionRoadStrip> FindPath(SectionRoadStrip start, SectionRoadStrip target)
    {
        if (start == null || target == null)
            return new List<SectionRoadStrip>();

        if (start == target)
            return new List<SectionRoadStrip> { start };

        PriorityQueue<PathNode> openSet = new();
        Dictionary<SectionRoadStrip, SectionRoadStrip> cameFrom = new();
        Dictionary<SectionRoadStrip, float> gScore = new();

        openSet.Enqueue(new PathNode(start, NavigationUtils.CalculateDistance(start, target)));
        gScore[start] = 0f;

        while (openSet.Count > 0)
        {
            PathNode current = openSet.Dequeue();

            if (current.Lane == target)
                return ReconstructPath(cameFrom, current.Lane);

            foreach (SectionRoadStrip neighbor in current.Lane.ConnectedLanes)
            {
                float tentativeGScore = gScore[current.Lane];

                if (gScore.ContainsKey(neighbor) == false)
                    gScore[neighbor] = Mathf.Infinity;

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current.Lane;
                    gScore[neighbor] = tentativeGScore;
                    float fScore = tentativeGScore + NavigationUtils.CalculateDistance(neighbor, target);

                    if (openSet.Contains(node => node.Lane == neighbor) == false)
                        openSet.Enqueue(new PathNode(neighbor, fScore));
                }
            }
        }

        return new List<SectionRoadStrip>();
    }

    private List<SectionRoadStrip> ReconstructPath(Dictionary<SectionRoadStrip, SectionRoadStrip> cameFrom, SectionRoadStrip current)
    {
        List<SectionRoadStrip> path = new() { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    private class PathNode : IComparable<PathNode>
    {
        public SectionRoadStrip Lane { get; }

        public float FScore { get; }

        public PathNode(SectionRoadStrip lane, float fScore)
        {
            Lane = lane;
            FScore = fScore;
        }

        public int CompareTo(PathNode other) => FScore.CompareTo(other.FScore);
    }
}