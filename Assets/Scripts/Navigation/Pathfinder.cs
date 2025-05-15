using System;
using System.Collections.Generic;

public static class Pathfinder
{
    public static List<SectionRoadStrip> FindPath(Waypoint startPoint, Waypoint targetPoint) =>
        FindShortestPathByLength(startPoint.Section, targetPoint.Section);

    private static List<SectionRoadStrip> FindShortestPathByLength(SectionRoadStrip startSection, SectionRoadStrip targetSection)
    {
        PriorityQueue<PathNode> frontier = new();
        Dictionary<SectionRoadStrip, SectionRoadStrip> cameFrom = new();
        Dictionary<SectionRoadStrip, float> costSoFar = new();

        frontier.Enqueue(new PathNode(startSection, 0f));
        costSoFar[startSection] = 0f;

        while (frontier.Count > 0)
        {
            PathNode current = frontier.Dequeue();

            if (current.Section == targetSection)
                return BuildPathFromCameFrom(cameFrom, current.Section);

            foreach (SectionRoadStrip neighbor in current.Section.ConnectedSections)
            {
                float newCost = costSoFar[current.Section] + current.Section.LenghtInMeter;

                if (costSoFar.ContainsKey(neighbor) == false || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    float priority = newCost + EstimateRemainingCost(neighbor, targetSection);
                    frontier.Enqueue(new PathNode(neighbor, priority));
                    cameFrom[neighbor] = current.Section;
                }
            }
        }

        return new List<SectionRoadStrip>();
    }

    private static float EstimateRemainingCost(SectionRoadStrip _, SectionRoadStrip __) =>
        0f;

    private static List<SectionRoadStrip> BuildPathFromCameFrom(Dictionary<SectionRoadStrip, SectionRoadStrip> cameFrom, SectionRoadStrip endNode)
    {
        List<SectionRoadStrip> path = new();
        SectionRoadStrip current = endNode;

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
    public SectionRoadStrip Section { get; }
    public float Priority { get; }

    public PathNode(SectionRoadStrip section, float priority)
    {
        Section = section;
        Priority = priority;
    }

    public int CompareTo(PathNode other) => Priority.CompareTo(other.Priority);
}