using UnityEngine;
using UnityEngine.InputSystem;

public class BuildableSystem : MonoBehaviour
{
    [SerializeField] private Transform _leftController;             // used to raycast from it (TODO: double check if there is a way to access from API directly instead of assigning)
    [SerializeField] private BuildableListSO _buildableListSO;     // list of possible buildables, used to instantiate actual game objects
    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private Transform _buildableOutline;

    private LevelEditorInputActions _inputActions;
    private int _currentBuildableIndex = 0;
    private float _raycastDistance = 100f;
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
    }

    private void Update()
    {
        // raycast to place buildables
        if (Physics.Raycast(_leftController.position, _leftController.forward, out RaycastHit hit, _raycastDistance, _raycastLayerMask))
        {
            // going from not hitting to hitting, so enable outline
            if (!_hitting)
            {
                _hitting = true;
                _buildableOutline.gameObject.SetActive(true);
            }

            // move the outline to the hit position
            _buildableOutline.position = hit.point;
        }
        else
        {
            // going from hit to no hit, hide the outline as there is no raycast hit
            if (_hitting)
            {
                _hitting = false;
                _buildableOutline.gameObject.SetActive(false);
            }
        }
    }

    private void CreateBuildable(InputAction.CallbackContext context)
    {
        // instantiate the buildable at the raycast hit outline location
        if (_hitting)
            Instantiate(_buildableListSO.Buildables[_currentBuildableIndex].Prefab, _buildableOutline.position, _buildableOutline.rotation);
    }
}
