using UnityEngine;

[CreateAssetMenu(menuName = "LevelEditor/Buildables")]
public class BuildableSO : ScriptableObject
{
    [SerializeField] private int _id;               // Id to identify the Buildable, same as its index stored in the BuildableListSO
    [SerializeField] private string _name;
    [SerializeField] private Transform _prefab;     // prefab to be instantiated when this buildable is created

    public int Id => _id;
    public string Name => _name;
    public Transform Prefab => _prefab;
}
