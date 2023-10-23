using UnityEngine;

[System.Serializable]
public struct BuildableData
{
    public int Id;                  // save scriptable object id for easier loading
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
}
