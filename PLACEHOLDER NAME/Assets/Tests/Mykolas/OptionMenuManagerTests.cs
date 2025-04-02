using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class OptionsMenuManagerEditModeTest
{
    private GameObject optionsMenuObject;
    private OptionsMenuManager optionsMenuManager;
    private TMP_Dropdown mockDropdown;
    private MockScreen mockScreen;

    public class MockScreen
    {
        public static Resolution[] resolutions = new Resolution[]
        {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 800, height = 600 }
        };

        public static void SetResolution(int width, int height, bool fullscreen)
        {
            LastSetResolution = new Resolution { width = width, height = height };
        }

        public static Resolution LastSetResolution { get; set; }
    }

    // Testable subclass that uses our mocks
    private class TestableOptionsMenuManager : OptionsMenuManager
    {
        protected override Resolution[] GetScreenResolutions()
        {
            return MockScreen.resolutions;
        }

        protected override void ApplyResolution(int width, int height, bool fullscreen)
        {
            MockScreen.SetResolution(width, height, fullscreen);
        }
    }

    [SetUp]
    public void Setup()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        optionsMenuObject = new GameObject();
        optionsMenuManager = optionsMenuObject.AddComponent<TestableOptionsMenuManager>();

        var dropdownObject = new GameObject();
        mockDropdown = dropdownObject.AddComponent<TMP_Dropdown>();
        optionsMenuManager.resDropDown = mockDropdown;

        MockScreen.LastSetResolution = default;
    }

    [TearDown]
    public void Teardown()
    {
        if (optionsMenuManager != null && optionsMenuManager.resDropDown != null)
        {
            Object.DestroyImmediate(optionsMenuManager.resDropDown.gameObject);
        }
        Object.DestroyImmediate(optionsMenuObject);
    }

    [Test]
    public void Start_PopulatesDropdownWithUniqueResolutions()
    {
        optionsMenuManager.Start();
        Assert.AreEqual(3, optionsMenuManager.resDropDown.options.Count);
        Assert.AreEqual("1920x1080", optionsMenuManager.resDropDown.options[0].text);
        Assert.AreEqual("1280x720", optionsMenuManager.resDropDown.options[1].text);
        Assert.AreEqual("800x600", optionsMenuManager.resDropDown.options[2].text);
    }

    [Test]
    public void ChangeResolution_SetsCorrectResolution()
    {
        optionsMenuManager.Start();
        optionsMenuManager.resDropDown.value = 1;
        optionsMenuManager.ChangeResolution();
        Assert.AreEqual(1280, MockScreen.LastSetResolution.width);
        Assert.AreEqual(720, MockScreen.LastSetResolution.height);
    }

    [Test]
    public void OpenOptionsMenu_ActivatesGameObject()
    {
        optionsMenuManager.gameObject.SetActive(false);
        optionsMenuManager.OpenOptionsMenu();
        Assert.IsTrue(optionsMenuManager.gameObject.activeSelf);
    }

    [Test]
    public void CloseOptionsMenu_DeactivatesGameObject()
    {
        optionsMenuManager.gameObject.SetActive(true);
        optionsMenuManager.CloseOptionsMenu();
        Assert.IsFalse(optionsMenuManager.gameObject.activeSelf);
    }

    [Test]
    public void CloseOptionsMenu_DoesNothingWhenGameObjectIsNull()
    {
        Object.DestroyImmediate(optionsMenuObject);
        Assert.DoesNotThrow(() => optionsMenuManager.CloseOptionsMenu());
    }
}
