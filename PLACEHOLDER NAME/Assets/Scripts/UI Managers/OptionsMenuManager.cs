using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resDropDown;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] Slider bGVolume;
    [SerializeField] Slider sFXVolume;
    [SerializeField] TextMeshProUGUI bGVolumeText = null;
    [SerializeField] TextMeshProUGUI sFXVolumeText = null;
    [SerializeField] float maxSliderAmount = 100.0f;
    [SerializeField] TMP_Dropdown screenModeDropDown;

    Resolution[] allResolutions;
    int selectedResoltuionIndex;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    [Header("Key Rebinding")]
    [SerializeField] private Button upKeyButton;
    [SerializeField] private Button downKeyButton;
    [SerializeField] private Button leftKeyButton;
    [SerializeField] private Button rightKeyButton;
    [SerializeField] private TextMeshProUGUI upKeyText;
    [SerializeField] private TextMeshProUGUI downKeyText;
    [SerializeField] private TextMeshProUGUI leftKeyText;
    [SerializeField] private TextMeshProUGUI rightKeyText;

    [Header("Rebind Settings")]
    [SerializeField] private GameObject rebindPanel;
    [SerializeField] private TextMeshProUGUI rebindPromptText;
    //[SerializeField] private float backgroundDimAlpha = 0.95f; 

    private Image rebindBackground;
    private KeyCode keyToRebind;
    private bool isRebinding = false;
    private const string BG_VOLUME_KEY = "BGVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";



    private void Awake()
    {
        LoadKeyBindings();
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>(); // Find it in the scene
        }
    }

    public void Start()
    {
        float savedBGVolume = PlayerPrefs.GetFloat(BG_VOLUME_KEY, AudioManager.instance._BackgroundMusicSource.volume);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, AudioManager.instance._SFXSource.volume);

        AudioManager.instance._BackgroundMusicSource.volume = savedBGVolume;
        AudioManager.instance._SFXSource.volume = savedSFXVolume;

        sFXVolumeText.text = (maxSliderAmount * savedSFXVolume).ToString("0");
        sFXVolume.value = savedSFXVolume;
        bGVolumeText.text = (maxSliderAmount * savedBGVolume).ToString("0");
        bGVolume.value = savedBGVolume;

        sFXVolumeText.text = (maxSliderAmount * AudioManager.instance._SFXSource.volume).ToString("0");
        sFXVolume.value = AudioManager.instance._SFXSource.volume;
        bGVolumeText.text = (maxSliderAmount * AudioManager.instance._BackgroundMusicSource.volume).ToString("0");
        bGVolume.value = AudioManager.instance._BackgroundMusicSource.volume;

        Debug.Log("options menu setActive false");
        gameObject.SetActive(false);

        allResolutions = Screen.resolutions;
        List<string> resolutionStringList = new List<string>();
        string newRes;

        selectedResolutionList.Clear();
        int currentResolutionIndex = 0;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            Resolution resolution = allResolutions[i];
            newRes = resolution.width + "x" + resolution.height;

            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(resolution);

                // Check if this matches current screen resolution
                if (resolution.width == Screen.currentResolution.width &&
                    resolution.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = selectedResolutionList.Count - 1;
                }
            }
        }

        resDropDown.ClearOptions();
        resDropDown.AddOptions(resolutionStringList);
        resDropDown.value = currentResolutionIndex;
        resDropDown.RefreshShownValue();


        List<string> screenModeOptions = new List<string> { "Windowed", "Fullscreen" };
        screenModeDropDown.AddOptions(screenModeOptions);

        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            screenModeDropDown.value = 1; // Fullscreen
        }
        else
        {
            screenModeDropDown.value = 0; // Windowed
        }

        UpdateKeyButtonTexts();
        upKeyButton.onClick.AddListener(() => StartRebinding("MoveUp"));
        downKeyButton.onClick.AddListener(() => StartRebinding("MoveDown"));
        leftKeyButton.onClick.AddListener(() => StartRebinding("MoveLeft"));
        rightKeyButton.onClick.AddListener(() => StartRebinding("MoveRight"));

        rebindBackground = rebindPanel.GetComponent<Image>();
        rebindPanel.SetActive(false);

    }

    private void Update()
    {
        if (isRebinding)
        {
            if (Input.anyKeyDown)
            {
                // Get the pressed key
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        // Skip mouse inputs
                        if (keyCode.ToString().Contains("Mouse")) continue;

                        FinishRebinding(keyCode);
                        LoadKeyBindings();
                        return;
                    }
                }
            }
        }
    }

    private string rebindingAction; // Add this class variable
    private void StartRebinding(string keyAction)
    {

        rebindingAction = keyAction; // Store which action we're rebinding
        rebindPromptText.text = $"Press a key for {keyAction}\n<size=24><color=#AAAAAA>ESC to cancel</color></size>";
        isRebinding = true;
        rebindPanel.SetActive(true);


        switch (keyAction)
        {
            case "MoveUp":
                rebindPromptText.text = "Press a key for Move Up";
                keyToRebind = moveUpKey;
                break;
            case "MoveDown":
                rebindPromptText.text = "Press a key for Move Down";
                keyToRebind = moveDownKey;
                break;
            case "MoveLeft":
                rebindPromptText.text = "Press a key for Move Left";
                keyToRebind = moveLeftKey;
                break;
            case "MoveRight":
                rebindPromptText.text = "Press a key for Move Right";
                keyToRebind = moveRightKey;
                break;
        }
    }

    private void FinishRebinding(KeyCode newKey)
    {
        isRebinding = false;
        rebindPanel.SetActive(false);

        switch (rebindingAction)
        {
            case "MoveUp":
                SetMoveUpKey(newKey);
                break;
            case "MoveDown":
                SetMoveDownKey(newKey);
                break;
            case "MoveLeft":
                SetMoveLeftKey(newKey);
                break;
            case "MoveRight":
                SetMoveRightKey(newKey);
                break;
        }

        SaveKeyBindings();
        UpdateKeyButtonTexts();
    }

    private void UpdateKeyButtonTexts()
    {
        upKeyText.text = moveUpKey.ToString();
        downKeyText.text = moveDownKey.ToString();
        leftKeyText.text = moveLeftKey.ToString();
        rightKeyText.text = moveRightKey.ToString();
    }

    public void SliderChangeBGVolume(float value)
    {
        float localValue = value * maxSliderAmount;
        bGVolumeText.text = localValue.ToString("0");
        AudioManager.instance._BackgroundMusicSource.volume = value;
        PlayerPrefs.SetFloat(BG_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    public void SliderChangeSFXVolume(float value)
    {
        float localValue = value * maxSliderAmount;
        sFXVolumeText.text = localValue.ToString("0");
        AudioManager.instance._SFXSource.volume = value;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    public void ChangeResolution()
    {
        selectedResoltuionIndex = resDropDown.value;
        bool isFullScreen = (Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen);
        Screen.SetResolution(selectedResolutionList[selectedResoltuionIndex].width, selectedResolutionList[selectedResoltuionIndex].height, isFullScreen);
    }

    public void ChangeScreenMode()
    {
        if (screenModeDropDown.value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (screenModeDropDown.value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    public void OpenOptionsMenu()
    {
        Debug.Log("open options menu");
        // optionsMenu.SetActive(true);
        gameObject.SetActive(true);
        if (gameObject != null) // Prevent multiple instances
        {
            gameObject.SetActive(true);
        }
    }


    public void CloseOptionsMenu()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }



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
        PlayerMovementController.LoadKeyBindings();
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