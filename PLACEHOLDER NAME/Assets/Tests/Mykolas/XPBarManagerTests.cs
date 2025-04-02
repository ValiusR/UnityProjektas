using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class XPBarManagerEditModeTest
{
    private GameObject xpBarManagerObject;
    private XPBarManager xpBarManager;
    private MockLevelUpSystem mockLevelUpSystem;
    private Image xpBarFill;
    private TextMeshProUGUI xpMeterText;

    // Mock LevelUpSystem
    public class MockLevelUpSystem : LevelUpSystem
    {
        public static int experience = 100;
        public int experienceToNextLevel = 200;

        public void SetExperience(int current, int toNextLevel)
        {
            experience = current;
            experienceToNextLevel = toNextLevel;
        }
    }

    [SetUp]
    public void Setup()
    {
        // Create a new empty scene for testing
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create Canvas for UI elements
        var canvasObject = new GameObject("Canvas");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create XP bar fill image as child of Canvas
        var fillObject = new GameObject("XPBarFill");
        fillObject.transform.SetParent(canvasObject.transform);
        xpBarFill = fillObject.AddComponent<Image>();

        // Create XP text as child of Canvas
        var textObject = new GameObject("XPMeterText");
        textObject.transform.SetParent(canvasObject.transform);
        xpMeterText = textObject.AddComponent<TextMeshProUGUI>();

        // Create mock level up system
        var levelUpObject = new GameObject();
        mockLevelUpSystem = levelUpObject.AddComponent<MockLevelUpSystem>();

        // Create XP bar manager
        xpBarManagerObject = new GameObject();
        xpBarManager = xpBarManagerObject.AddComponent<XPBarManager>();

        // Assign references
        xpBarManager.levelUpSystem = mockLevelUpSystem;
        xpBarManager.xpBarFill = xpBarFill;
        xpBarManager.xpMeterText = xpMeterText;

        // Set initial values
        mockLevelUpSystem.SetExperience(50, 100);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up in reverse order
        Object.DestroyImmediate(xpBarManagerObject);
        Object.DestroyImmediate(mockLevelUpSystem.gameObject);

        // Destroy Canvas and children
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Object.DestroyImmediate(canvas);
        }
    }

    [Test]
    public void UpdateXpBar_UpdatesFillAmountCorrectly()
    {
        // Arrange
        mockLevelUpSystem.SetExperience(75, 100);

        // Act
        xpBarManager.UpdateXpBar(MockLevelUpSystem.experience, mockLevelUpSystem.experienceToNextLevel);

        // Assert
        Assert.AreEqual(0.75f, xpBarFill.fillAmount, 0.001f);
    }

    [Test]
    public void UpdateXpBar_UpdatesTextCorrectly()
    {
        // Arrange
        mockLevelUpSystem.SetExperience(30, 60);

        // Act
        xpBarManager.UpdateXpBar(MockLevelUpSystem.experience, mockLevelUpSystem.experienceToNextLevel);

        // Assert
        Assert.AreEqual("XP: 30/60", xpMeterText.text);
    }

    [Test]
    public void UpdateXpBar_HandlesZeroExperienceToNextLevel()
    {
        // Arrange
        mockLevelUpSystem.SetExperience(50, 0);

        // Act
        xpBarManager.UpdateXpBar(MockLevelUpSystem.experience, mockLevelUpSystem.experienceToNextLevel);

        // Assert
        Assert.AreEqual(0f, xpBarFill.fillAmount);
        Assert.AreEqual("XP: 50/0", xpMeterText.text);
    }

    [Test]
    public void UpdateXpBar_DoesNothingWhenXpBarFillIsNull()
    {
        // Arrange
        xpBarManager.xpBarFill = null;
        mockLevelUpSystem.SetExperience(25, 50);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            xpBarManager.UpdateXpBar(MockLevelUpSystem.experience, mockLevelUpSystem.experienceToNextLevel));
    }

    [Test]
    public void UpdateXpBar_DoesNothingWhenXpMeterTextIsNull()
    {
        // Arrange
        xpBarManager.xpMeterText = null;
        mockLevelUpSystem.SetExperience(10, 20);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            xpBarManager.UpdateXpBar(MockLevelUpSystem.experience, mockLevelUpSystem.experienceToNextLevel));
    }
}