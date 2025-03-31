using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resDropDown;
    [SerializeField] private AudioManager audioManager;

    //[SerializeField] Slider bGVolume;
    //[SerializeField] Slider sFXVolume;
    [SerializeField] TextMeshProUGUI bGVolumeText = null;
    [SerializeField] float maxSliderAmount = 100.0f;

    //[SerializeField] TextMeshProUGUI sFXVolumeText;



    Resolution[] allResolutions;
    int selectedResoltuionIndex;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        bGVolumeText.text = localValue.ToString("0");   
    }

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>(); // Find it in the scene
        }
    }

    public void Start()
    {
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
    }

    public void ChangeResolution()
    {
        selectedResoltuionIndex = resDropDown.value;
        Screen.SetResolution(selectedResolutionList[selectedResoltuionIndex].width, selectedResolutionList[selectedResoltuionIndex].height, true);
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
