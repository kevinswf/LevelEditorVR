using UnityEngine;

[CreateAssetMenu(menuName = "LevelEditor/Buildables")]
public class BuildableSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Transform _prefab;     // prefab to be instantiated when this buildable is created

    public string Name => _name;
    public Transform Prefab => _prefab;
}
