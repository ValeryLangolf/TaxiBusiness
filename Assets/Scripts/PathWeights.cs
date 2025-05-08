using UnityEngine;

[CreateAssetMenu(menuName = "Pathfinding/Path Weights")]
public class PathWeights : ScriptableObject
{
    [SerializeField] private float _main = 1f;
    [SerializeField] private float _turn = 1.5f;
    [SerializeField] private float _transition = 2f;

    public float GetWeight(LaneType type) => type switch
    {
        LaneType.Main => _main,
        LaneType.LeftTurn => _turn,
        LaneType.RightTurn => _turn,
        LaneType.Transition => _transition,
        _ => float.MaxValue
    };
}