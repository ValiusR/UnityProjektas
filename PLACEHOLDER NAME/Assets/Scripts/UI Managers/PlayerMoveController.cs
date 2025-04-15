using UnityEngine;
using System.Collections; // Needed for Coroutine

public class PlayerMoveController : MonoBehaviour
{
    [Header("Controls")]

    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    // Key names for PlayerPrefs
    private const string MOVE_UP_KEY = "MoveUpKey";
    private const string MOVE_DOWN_KEY = "MoveDownKey";
    private const string MOVE_LEFT_KEY = "MoveLeftKey";
    private const string MOVE_RIGHT_KEY = "MoveRightKey";

    void Awake()
    {
        // Load saved keybindings or use defaults
        LoadKeyBindings();
    }

    public void LoadKeyBindings()
    {
        moveUpKey = (KeyCode)PlayerPrefs.GetInt(MOVE_UP_KEY, (int)KeyCode.W);
        moveDownKey = (KeyCode)PlayerPrefs.GetInt(MOVE_DOWN_KEY, (int)KeyCode.S);
        moveLeftKey = (KeyCode)PlayerPrefs.GetInt(MOVE_LEFT_KEY, (int)KeyCode.A);
        moveRightKey = (KeyCode)PlayerPrefs.GetInt(MOVE_RIGHT_KEY, (int)KeyCode.D);
        Debug.Log("Player controls loaded.");
    }

    public void SaveKeyBindings()
    {
        PlayerPrefs.SetInt(MOVE_UP_KEY, (int)moveUpKey);
        PlayerPrefs.SetInt(MOVE_DOWN_KEY, (int)moveDownKey);
        PlayerPrefs.SetInt(MOVE_LEFT_KEY, (int)moveLeftKey);
        PlayerPrefs.SetInt(MOVE_RIGHT_KEY, (int)moveRightKey);
        PlayerPrefs.Save(); // Ensure changes are written to disk
        Debug.Log("Player controls saved.");
    }

    // Methods called by OptionsMenuManager to update keys
    public void SetMoveUpKey(KeyCode newKey) { moveUpKey = newKey; }
    public void SetMoveDownKey(KeyCode newKey) { moveDownKey = newKey; }
    public void SetMoveLeftKey(KeyCode newKey) { moveLeftKey = newKey; }
    public void SetMoveRightKey(KeyCode newKey) { moveRightKey = newKey; }

    // Optional: Reset to defaults
    public void ResetKeyBindings()
    {
        moveUpKey = KeyCode.W;
        moveDownKey = KeyCode.S;
        moveLeftKey = KeyCode.A;
        moveRightKey = KeyCode.D;
        SaveKeyBindings(); // Save defaults after resetting
        Debug.Log("Player controls reset to defaults.");
    }
}