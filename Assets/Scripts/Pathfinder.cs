using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private PathWeights _pathWeights;

    public List<LaneComponent> FindPath(LaneComponent start, LaneComponent target)
    {
        if (start == null || target == null)
            return new List<LaneComponent>();

        if (start == target)
            return new List<LaneComponent> { start };

        PriorityQueue<PathNode> openSet = new();
        Dictionary<LaneComponent, LaneComponent> cameFrom = new();
        Dictionary<LaneComponent, float> gScore = new();

        openSet.Enqueue(new PathNode(start, NavigationUtils.CalculateHeuristic(start, target)));
        gScore[start] = 0f;

        while (openSet.Count > 0)
        {
            PathNode current = openSet.Dequeue();

            if (current.Lane == target)
                return ReconstructPath(cameFrom, current.Lane);

            foreach (LaneComponent neighbor in current.Lane.ConnectedLanes)
            {
                if (neighbor.Type == LaneType.Opposite)
                    continue;

                float tentativeGScore = gScore[current.Lane] + _pathWeights.GetWeight(neighbor.Type);

                if (gScore.ContainsKey(neighbor) == false)
                    gScore[neighbor] = Mathf.Infinity;

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current.Lane;
                    gScore[neighbor] = tentativeGScore;
                    float fScore = tentativeGScore + NavigationUtils.CalculateHeuristic(neighbor, target);

                    if (openSet.Contains(node => node.Lane == neighbor) == false)
                        openSet.Enqueue(new PathNode(neighbor, fScore));
                }
            }
        }

        return new List<LaneComponent>();
    }

    private List<LaneComponent> ReconstructPath(Dictionary<LaneComponent, LaneComponent> cameFrom, LaneComponent current)
    {
        List<LaneComponent> path = new() { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    private class PathNode : IComparable<PathNode>
    {
        public LaneComponent Lane { get; }
        public float FScore { get; }

        public PathNode(LaneComponent lane, float fScore)
        {
            Lane = lane;
            FScore = fScore;
        }

        public int CompareTo(PathNode other) => FScore.CompareTo(other.FScore);
    }
}