﻿using System;
using System.Collections.Generic;

public static class Pathfinder
{
    public const float DefaultEstimateCost = 0f;

    public static List<Waypoint> FindPath(Waypoint startPoint, Waypoint targetPoint)
    {
        PriorityQueue<PathNode> frontier = new();
        Dictionary<Waypoint, Waypoint> cameFrom = new();
        Dictionary<Waypoint, float> costSoFar = new();

        frontier.Enqueue(new PathNode(startPoint, 0f));
        costSoFar[startPoint] = 0f;

        while (frontier.Count > 0)
        {
            PathNode current = frontier.Dequeue();

            if (current.Waypoint == targetPoint)
                return BuildPathFromCameFrom(cameFrom, current.Waypoint);

            foreach (Waypoint neighbor in current.Waypoint.ConnectedPoints)
            {
                float newCost = costSoFar[current.Waypoint] + current.Waypoint.LenghtInMeter;

                if (costSoFar.ContainsKey(neighbor) && newCost >= costSoFar[neighbor])
                    continue;

                costSoFar[neighbor] = newCost;
                float priority = newCost + EstimateRemainingCost(neighbor, targetPoint);
                frontier.Enqueue(new PathNode(neighbor, priority));
                cameFrom[neighbor] = current.Waypoint;
            }
        }

        return new List<Waypoint>();
    }

    private static float EstimateRemainingCost(Waypoint _, Waypoint __) =>
        DefaultEstimateCost;

    private static List<Waypoint> BuildPathFromCameFrom(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint endNode)
    {
        List<Waypoint> path = new();
        Waypoint current = endNode;

        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, current);
            current = cameFrom[current];
        }

        path.Insert(0, current);

        return path;
    }
}

public class PathNode : IComparable<PathNode>
{
    public Waypoint Waypoint { get; }

    public float Priority { get; }

    public PathNode(Waypoint section, float priority)
    {
        Waypoint = section;
        Priority = priority;
    }

    public int CompareTo(PathNode other) =>
        Priority.CompareTo(other.Priority);
}