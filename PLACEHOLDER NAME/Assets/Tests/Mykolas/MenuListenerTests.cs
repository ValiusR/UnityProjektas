using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListenerEditModeTest
{
    private GameObject menuListenerObject;
    private TestableMenuListener menuListener;
    private string testSceneName = "TestScene";
    private string pauseMenuScenePath = "Assets/Tests/TestPauseMenu.unity";

    // Mock SceneManager wrapper
    private class MockSceneManager
    {
        public List<string> loadedScenes = new List<string>();

        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            if (!loadedScenes.Contains(sceneName))
            {
                loadedScenes.Add(sceneName);
            }
        }

        public void UnloadScene(string sceneName)
        {
            loadedScenes.Remove(sceneName);
        }
    }

    // Testable MenuListener subclass that uses our mock
    private class TestableMenuListener : MenuListener
    {
        public MockSceneManager mockSceneManager = new MockSceneManager();

        protected override void LoadPauseMenuScene()
        {
            mockSceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
        }

        protected override void UnloadPauseMenuScene()
        {
            mockSceneManager.UnloadScene(pauseMenuSceneName);
        }
    }

    [SetUp]
    public void Setup()
    {
        // Create a new empty scene for testing
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create test pause menu scene if it doesn't exist
        if (!System.IO.File.Exists(pauseMenuScenePath))
        {
            var pauseScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(pauseScene, pauseMenuScenePath);
        }

        // Create TestableMenuListener GameObject
        menuListenerObject = new GameObject();
        menuListener = menuListenerObject.AddComponent<TestableMenuListener>();
        menuListener.pauseMenuSceneName = "TestPauseMenu";
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up GameObjects
        if (menuListenerObject != null)
        {
            Object.DestroyImmediate(menuListenerObject);
        }

        if (menuListenerObject != null)
        {
            Object.DestroyImmediate(menuListenerObject);
        }

        // Delete the TestPauseMenu scene if it exists
        if (System.IO.File.Exists(pauseMenuScenePath))
        {
            UnityEditor.AssetDatabase.DeleteAsset(pauseMenuScenePath);
            UnityEditor.AssetDatabase.Refresh();
        }

    }

    [Test]
    public void PauseGame_SetsTimeScaleToZero()
    {
        // Act
        menuListener.PauseGame();

        // Assert
        Assert.AreEqual(0f, Time.timeScale);
    }

    [Test]
    public void PauseGame_SetsIsPausedToTrue()
    {
        // Act
        menuListener.PauseGame();

        // Assert
        Assert.IsTrue(menuListener.isPaused);
    }

    [Test]
    public void PauseGame_LoadsPauseMenuScene()
    {
        // Act
        menuListener.PauseGame();

        // Assert
        Assert.IsTrue(menuListener.mockSceneManager.loadedScenes.Contains(menuListener.pauseMenuSceneName));
    }

    [Test]
    public void ResumeGame_SetsTimeScaleToOne()
    {
        // Arrange
        menuListener.PauseGame();

        // Act
        menuListener.ResumeGame();

        // Assert
        Assert.AreEqual(1f, Time.timeScale);
    }

    [Test]
    public void ResumeGame_SetsIsPausedToFalse()
    {
        // Arrange
        menuListener.PauseGame();

        // Act
        menuListener.ResumeGame();

        // Assert
        Assert.IsFalse(menuListener.isPaused);
    }

    [Test]
    public void ResumeGame_UnloadsPauseMenuScene()
    {
        // Arrange
        menuListener.PauseGame();

        // Act
        menuListener.ResumeGame();

        // Assert
        Assert.IsFalse(menuListener.mockSceneManager.loadedScenes.Contains(menuListener.pauseMenuSceneName));
    }
}