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

    private PlayerMovementController playerMovement;
    private KeyCode keyToRebind;
    private bool isRebinding = false;



    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>(); // Find it in the scene
        }
    }

    public void Start()
    {

        sFXVolumeText.text = (maxSliderAmount * AudioManager.instance._SFXSource.volume).ToString("0");
        sFXVolume.value = AudioManager.instance._SFXSource.volume;
        bGVolumeText.text = (maxSliderAmount * AudioManager.instance._BackgroundMusicSource.volume).ToString("0");
        bGVolume.value = AudioManager.instance._BackgroundMusicSource.volume;

        Debug.Log("options menu setActive false");
        gameObject.SetActive(false);

        allResolutions = Screen.resolutions;


        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution resolution in allResolutions)
        {
            newRes = resolution.width.ToString() + "x" + resolution.height.ToString();
            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(resolution);
            }
        }
        resDropDown.AddOptions(resolutionStringList);

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

        playerMovement = FindObjectOfType<PlayerMovementController>();
        UpdateKeyButtonTexts();
        upKeyButton.onClick.AddListener(() => StartRebinding("MoveUp"));
        downKeyButton.onClick.AddListener(() => StartRebinding("MoveDown"));
        leftKeyButton.onClick.AddListener(() => StartRebinding("MoveLeft"));
        rightKeyButton.onClick.AddListener(() => StartRebinding("MoveRight"));


        // Get reference to the background image
        rebindBackground = rebindPanel.GetComponent<Image>();

        // Initialize with transparent color
        /*
        Color bgColor = rebindBackground.color;
        bgColor.a = 0f;
        rebindBackground.color = bgColor;
        */

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
                        return;
                    }
                }
            }
        }
    }

    private void StartRebinding(string keyAction)
    {

        // Set the prompt text
        rebindPromptText.text = $"Press a key for {keyAction}\n<size=24><color=#AAAAAA>ESC to cancel</color></size>";

        isRebinding = true;
        rebindPanel.SetActive(true);


        switch (keyAction)
        {
            case "MoveUp":
                rebindPromptText.text = "Press a key for Move Up";
                keyToRebind = playerMovement.moveUpKey;
                break;
            case "MoveDown":
                rebindPromptText.text = "Press a key for Move Down";
                keyToRebind = playerMovement.moveDownKey;
                break;
            case "MoveLeft":
                rebindPromptText.text = "Press a key for Move Left";
                keyToRebind = playerMovement.moveLeftKey;
                break;
            case "MoveRight":
                rebindPromptText.text = "Press a key for Move Right";
                keyToRebind = playerMovement.moveRightKey;
                break;
        }
    }

    private void FinishRebinding(KeyCode newKey)
    {
        isRebinding = false;
        rebindPanel.SetActive(false);

        // Update the appropriate key in PlayerMovementController
        switch (rebindPromptText.text)
        {
            case "Press a key for Move Up":
                playerMovement.SetMoveUpKey(newKey);
                break;
            case "Press a key for Move Down":
                playerMovement.SetMoveDownKey(newKey);
                break;
            case "Press a key for Move Left":
                playerMovement.SetMoveLeftKey(newKey);
                break;
            case "Press a key for Move Right":
                playerMovement.SetMoveRightKey(newKey);
                break;
        }

        playerMovement.SaveKeyBindings();
        UpdateKeyButtonTexts();
    }

    private void UpdateKeyButtonTexts()
    {
        upKeyText.text = playerMovement.moveUpKey.ToString();
        downKeyText.text = playerMovement.moveDownKey.ToString();
        leftKeyText.text = playerMovement.moveLeftKey.ToString();
        rightKeyText.text = playerMovement.moveRightKey.ToString();
    }

    public void SliderChangeBGVolume(float value)
    {
        float localValue = value * maxSliderAmount;
        bGVolumeText.text = localValue.ToString("0");
        AudioManager.instance._BackgroundMusicSource.volume = value;
    }

    public void SliderChangeSFXVolume(float value)
    {
        float localValue = value * maxSliderAmount;
        sFXVolumeText.text = localValue.ToString("0");
        AudioManager.instance._SFXSource.volume = value;
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
}