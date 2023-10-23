using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuildableOutline : MonoBehaviour
{
    private Vector3 _outlineOffset;

    private void Start()
    {
        // assuming collider encapuslates the mesh, then offset the outline by half size so visually no intersection with existing hit object
        _outlineOffset = GetComponent<Collider>().bounds.size * 0.5f;

        // outlines start as deactivated
        gameObject.SetActive(false);
    }

    public Vector3 OutlineOffset => _outlineOffset;
}
