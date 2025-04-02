using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class DeathUiManagerEditModeTest
{
    private GameObject deathUiManagerObject;
    private DeathUiManager deathUiManager;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timeText;

    private MockSceneManager mockSceneManager;




    public class TestableDeathUiManager : DeathUiManager
    {
        public MockSceneManager mockSceneManager = new MockSceneManager();

        protected override void LoadMainMenuScene()
        {
            mockSceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    [SetUp]
    public void Setup()
    {
        // Create a new empty scene for testing
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create test objects
        deathUiManagerObject = new GameObject();
        deathUiManager = deathUiManagerObject.AddComponent<TestableDeathUiManager>();

        // Create UI text objects
        var scoreTextObj = new GameObject();
        scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        deathUiManager.scoreText = scoreText;

        var timeTextObj = new GameObject();
        timeText = timeTextObj.AddComponent<TextMeshProUGUI>();
        deathUiManager.timeText = timeText;

        // Initialize mock dependencies

        mockSceneManager = ((TestableDeathUiManager)deathUiManager).mockSceneManager;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up all created objects
        Object.DestroyImmediate(deathUiManagerObject);
        if (deathUiManager.scoreText != null)
            Object.DestroyImmediate(deathUiManager.scoreText.gameObject);
        if (deathUiManager.timeText != null)
            Object.DestroyImmediate(deathUiManager.timeText.gameObject);
    }

    [Test]
    public void HideUI_DeactivatesGameObjectAndResumesTime()
    {
        // Arrange
        deathUiManager.gameObject.SetActive(true);
        Time.timeScale = 0f;

        // Act
        deathUiManager.HideUI();

        // Assert
        Assert.IsFalse(deathUiManager.gameObject.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
    }

    [Test]
    public void QuitToMainMenu_HidesUIAndLoadsMainMenuScene()
    {
        // Arrange
        deathUiManager.gameObject.SetActive(true);
        Time.timeScale = 0f;

        // Act
        deathUiManager.QuitToMainMenu();

        // Assert
        Assert.IsFalse(deathUiManager.gameObject.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsTrue(mockSceneManager.loadedScenes.Contains("MainMenu"));
    }

    [Test]
    public void ShowDeathUI_ActivatesGameObjectAndPausesTime()
    {
        // Arrange
        deathUiManager.gameObject.SetActive(false);
        Time.timeScale = 1f;
        ScoreManager.currScore = 1500;
        TimerManager.formattedTime = "02:15";


        // Act
        deathUiManager.ShowDeathUI();

        // Assert
        Assert.IsTrue(deathUiManager.gameObject.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);
    }

    [Test]
    public void ShowDeathUI_SetsCorrectScoreText()
    {
        // Arrange
        ScoreManager.currScore = 2000;

        // Act
        deathUiManager.ShowDeathUI();

        // Assert
        Assert.AreEqual("YOUR SCORE: 2000", deathUiManager.scoreText.text);
    }

    [Test]
    public void ShowDeathUI_SetsCorrectTimeText()
    {
        // Arrange
        TimerManager.formattedTime = "03:45";

        // Act
        deathUiManager.ShowDeathUI();

        // Assert
        Assert.AreEqual("YOU HAVE SURVIVED: 03:45", deathUiManager.timeText.text);
    }

}