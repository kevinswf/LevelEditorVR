using UnityEngine;

public class Buildable : MonoBehaviour
{
    [SerializeField] private BuildableSO _buildableSO;  // the scriptable object used to create this prefab
    private BuildableData _buildableData;

    private void Start()
    {
        // populate initiate buildable data
        _buildableData = new BuildableData
        {
            Id = _buildableSO.Id,
            Name = _buildableSO.Name,
            Position = Vector3.zero,
            Rotation = Quaternion.identity
        };
    }

    public BuildableData BuildableData
    {
        get
        {
            // update the transform and return (id and name is unchanged)
            _buildableData.Position = transform.position;
            _buildableData.Rotation = transform.rotation;
            return _buildableData;
        }
    }
}
