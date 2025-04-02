using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class MainMenuManagerEditModeTest
{
    private GameObject mainMenuManagerObject;
    private TestableMainMenuManager mainMenuManager;
    private GameObject optionsMenuObject;
    private MockSceneManager mockSceneManager;

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
    private class TestableMainMenuManager : MainMenuManager
    {
        public MockSceneManager mockSceneManager = new MockSceneManager();

        protected override void LoadGameScene()
        {
            mockSceneManager.LoadScene("Main");
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
        mainMenuManagerObject = new GameObject();
        mainMenuManager = mainMenuManagerObject.AddComponent<TestableMainMenuManager>();

        // Create options menu
        optionsMenuObject = new GameObject();
        mainMenuManager.optionsMenu = optionsMenuObject;

        // Get the mock scene manager
        mockSceneManager = mainMenuManager.mockSceneManager;

        // Initial state
        optionsMenuObject.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up all created objects
        Object.DestroyImmediate(mainMenuManagerObject);
        Object.DestroyImmediate(optionsMenuObject);
    }

    [Test]
    public void NewGame_LoadsMainScene()
    {
        // Act
        mainMenuManager.NewGame();

        // Assert
        Assert.AreEqual("Main", mockSceneManager.loadedScene);
    }

    [Test]
    public void Options_ActivatesOptionsMenu()
    {
        // Arrange
        optionsMenuObject.SetActive(false);

        // Act
        mainMenuManager.Options();

        // Assert
        Assert.IsTrue(optionsMenuObject.activeSelf);
    }

    [Test]
    public void Options_DoesNothingWhenOptionsMenuIsNull()
    {
        // Arrange
        mainMenuManager.optionsMenu = null;

        // Act & Assert
        Assert.DoesNotThrow(() => mainMenuManager.Options());
    }

    [Test]
    public void QuitGame_CallsQuitApplication()
    {
        // Act
        mainMenuManager.QuitGame();

        // Assert
        Assert.IsTrue(mockSceneManager.quitCalled);
    }
}