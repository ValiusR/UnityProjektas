using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class PauseMenuManagerEditModeTest
{
    private GameObject pauseMenuObject;
    private TestablePauseMenuManager pauseMenuManager;
    private GameObject optionsMenuObject;
    private MockMenuListener mockMenuListener;
    private MockSceneManager mockSceneManager;

    // Mock MenuListener
    public class MockMenuListener : MonoBehaviour
    {
        public bool resumeGameCalled = false;

        public void ResumeGame()
        {
            resumeGameCalled = true;
        }
    }

    // Mock SceneManager
    public class MockSceneManager
    {
        public string loadedScene;
        public bool quitCalled = false;

        public void LoadScene(string sceneName)
        {
            loadedScene = sceneName;
        }

        public void QuitApplication()
        {
            quitCalled = true;
        }
    }

    // Testable subclass that uses our mocks
    private class TestablePauseMenuManager : PauseMenuManager
    {
        public MockSceneManager mockSceneManager = new MockSceneManager();
        protected override void LoadMainMenuScene()
        {
            mockSceneManager.LoadScene("MainMenu");
        }

        protected override void QuitApplication()
        {
            mockSceneManager.QuitApplication();
        }
    }

    [SetUp]
    public void Setup()
    {
        // Create a new empty scene for testing
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create test objects
        pauseMenuObject = new GameObject();
        pauseMenuManager = pauseMenuObject.AddComponent<TestablePauseMenuManager>();

        // Create options menu
        optionsMenuObject = new GameObject();
        pauseMenuManager.optionsMenu = optionsMenuObject;

        // Create and setup mock MenuListener
        var listenerObject = new GameObject();
        mockMenuListener = listenerObject.AddComponent<MockMenuListener>();

        mockSceneManager = pauseMenuManager.mockSceneManager;

        // Initial state
        optionsMenuObject.SetActive(false);
        Time.timeScale = 0f; // Simulate paused state
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up all created objects
        Object.DestroyImmediate(pauseMenuObject);
        Object.DestroyImmediate(optionsMenuObject);
        if (mockMenuListener != null)
            Object.DestroyImmediate(mockMenuListener.gameObject);
        Time.timeScale = 1f; // Reset time scale
    }

    [Test]
    public void Resume_DoesNothingWhenNoMenuListenerFound()
    {
        // Arrange
        Object.DestroyImmediate(mockMenuListener);

        // Act
        pauseMenuManager.Resume();

        // Assert
        // Just verifying no exception is thrown
        Assert.Pass();
    }

    [Test]
    public void QuitToMainMenu_ResetsTimeScaleAndLoadsMainMenu()
    {
        // Arrange
        Time.timeScale = 0f;

        // Act
        pauseMenuManager.QuitToMainMenu();

        // Assert
        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual("MainMenu", mockSceneManager.loadedScene);
    }

    [Test]
    public void Options_ActivatesOptionsMenu()
    {
        // Arrange
        optionsMenuObject.SetActive(false);

        // Act
        pauseMenuManager.Options();

        // Assert
        Assert.IsTrue(optionsMenuObject.activeSelf);
    }

    [Test]
    public void QuitGame_CallsQuitApplication()
    {
        // Act
        pauseMenuManager.QuitGame();

        // Assert
        Assert.IsTrue(mockSceneManager.quitCalled);
    }
}