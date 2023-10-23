using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BuildableSystem : MonoBehaviour
{
    [SerializeField] private Transform _leftController;             // used to raycast from it (TODO: double check if there is a way to access from API directly instead of assigning)
    [SerializeField] private BuildableListSO _buildableListSO;      // list of possible buildables, used to instantiate actual game objects
    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private LayerMask _deletableLayerMask;         // which game objects can be deleted
    [SerializeField] private Transform _buildableOutlineParent;
    [SerializeField] private Transform _levelEditorParent;      // instantiate all buildables as child of this, so easier to manage and save
    [SerializeField] private float _rotateSpeed = 100f;

    private LevelEditorInputActions _inputActions;
    private List<Transform> _buildableOutlines;                     // list of outlines existed in scene, used to enable/disable at runtime
    private Transform _currentBuildableOutline;
    private int _currentBuildableIndex = 0;
    private float _raycastDistance = 100f;                          // no point in casting to infinity
    private RaycastHit _hit;
    private bool _hitting = false;

    private void Awake()
    {
        // enable custom input actions
        _inputActions = new LevelEditorInputActions();
        _inputActions.LevelEditor.Enable();
    }

    private void Start()
    {
        // bind input actions to callbacks
        _inputActions.LevelEditor.Create.performed += CreateBuildable;
        _inputActions.LevelEditor.Delete.performed += DeleteBuildable;
        _inputActions.LevelEditor.NextBuildable.performed += NextBuildable;

        // populate the buildable outline list
        _buildableOutlines = new List<Transform>();
        foreach (Transform outline in _buildableOutlineParent)
        {
            _buildableOutlines.Add(outline);
        }

        // assign the current outline  (assume order is the same as in BuildableList SO)
        _currentBuildableOutline = _buildableOutlines[_currentBuildableIndex];
    }

    private void Update()
    {
        // raycast to place buildables
        if (Physics.Raycast(_leftController.position, _leftController.forward, out _hit, _raycastDistance, _raycastLayerMask))
        {
            // going from not hitting to hitting, so enable outline
            if (!_hitting)
            {
                _hitting = true;
                _currentBuildableOutline.gameObject.SetActive(true);
            }

            // move the outline to the hit position
            _currentBuildableOutline.position = _hit.point;

            // read controller input and rotate on the y axis;
            float horizontalValue = _inputActions.LevelEditor.Rotate.ReadValue<Vector2>().x;
            _currentBuildableOutline.Rotate(0, -horizontalValue * _rotateSpeed * Time.deltaTime, 0);
        }
        else
        {
            // going from hit to no hit, hide the outline as there is no raycast hit
            if (_hitting)
            {
                _hitting = false;
                _currentBuildableOutline.gameObject.SetActive(false);
            }
        }
    }

    private void CreateBuildable(InputAction.CallbackContext context)
    {
        // instantiate the buildable at the raycast hit outline location
        if (_hitting)
            Instantiate(_buildableListSO.Buildables[_currentBuildableIndex].Prefab, _currentBuildableOutline.position, _currentBuildableOutline.rotation, _levelEditorParent);
    }

    private void DeleteBuildable(InputAction.CallbackContext context)
    {
        // can only delete while there is a hit and the hit is on deletable layer
        if (_hitting && ((_deletableLayerMask & (1 << _hit.transform.gameObject.layer)) != 0))
        {
            Destroy(_hit.transform.gameObject);
        }
    }

    private void NextBuildable(InputAction.CallbackContext context)
    {
        // disable the current outline
        _currentBuildableOutline.gameObject.SetActive(false);

        // get the next buildable index (with cycle)
        _currentBuildableIndex = ++_currentBuildableIndex % _buildableListSO.TotalBuildables;

        // enable the outline
        _currentBuildableOutline = _buildableOutlines[_currentBuildableIndex];
        _currentBuildableOutline.gameObject.SetActive(true);
    }
}
