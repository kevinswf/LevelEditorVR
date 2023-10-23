using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "LevelEditor/BuildableList")]
public class BuildableListSO : ScriptableObject
{
    [SerializeField] private List<BuildableSO> _buildables;

    public List<BuildableSO> Buildables => _buildables;
    public int TotalBuildables => _buildables.Count;
}
