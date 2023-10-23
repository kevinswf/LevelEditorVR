using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections.Generic;
public class SaveLoadSystem : MonoBehaviour
{
    [SerializeField] private Transform _levelEditorParent;
    [SerializeField] private BuildableListSO _buildableListSO;  // list of possible buildables

    private LevelEditorInputActions _inputActions;
    private string _saveFile;
    private List<BuildableData> _buildableDataList;             // hold the list of buildable data to be saved

    private void Awake()
    {
        // enable custom input actions
        _inputActions = new LevelEditorInputActions();
        _inputActions.LevelEditor.Enable();

        _buildableDataList = new List<BuildableData>();
    }

    void Start()
    {
        _inputActions.LevelEditor.SaveLevel.started += SaveLevel;

        // save to json (assume a single save file for now)
        _saveFile = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SavedLevel.json";

        // auto load previously saved level if any
        if (File.Exists(_saveFile))
            LoadLevel();
    }

    private void SaveLevel(InputAction.CallbackContext context)
    {
        using StreamWriter writer = new StreamWriter(_saveFile);

        // clear any previous data in the list
        _buildableDataList.Clear();

        // iterate through each buildable, save its Id, name, position, rotation
        foreach (Transform childBuildable in _levelEditorParent)
        {
            if (childBuildable.TryGetComponent(out Buildable buildable))
            {
                _buildableDataList.Add(buildable.BuildableData);
            }
        }

        // convert data to json and save
        string json = JsonHelper.ToJson(_buildableDataList);
        writer.Write(json);
    }

    private void LoadLevel()
    {
        // load the saved level json file
        using StreamReader reader = new StreamReader(_saveFile);
        string json = reader.ReadToEnd();

        // convert to buildable data and instantiate level
        _buildableDataList.Clear();
        _buildableDataList = JsonHelper.FromJson<BuildableData>(json);
        foreach (BuildableData buildableData in _buildableDataList)
        {
            // get the scriptable object from the saved Id
            BuildableSO buildableSO = _buildableListSO.Buildables[buildableData.Id];

            // instantiate the game object from the scriptiable object
            Instantiate(buildableSO.Prefab, buildableData.Position, buildableData.Rotation, _levelEditorParent);
        }
    }
}
