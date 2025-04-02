using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField] public TMP_Dropdown resDropDown;

    Resolution[] allResolutions;
    int selectedResoltuionIndex;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    public void Start()
    {
        gameObject.SetActive(false);
        allResolutions = GetScreenResolutions();

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
        ApplyResolution(
            selectedResolutionList[selectedResoltuionIndex].width,
            selectedResolutionList[selectedResoltuionIndex].height,
            true
        );
    }

    public void OpenOptionsMenu()
    {
        if (this && gameObject)
        {
            gameObject.SetActive(true);
        }
    }

    public void CloseOptionsMenu()
    {
        if (this && gameObject)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual Resolution[] GetScreenResolutions()
    {
        return Screen.resolutions;
    }

    protected virtual void ApplyResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}