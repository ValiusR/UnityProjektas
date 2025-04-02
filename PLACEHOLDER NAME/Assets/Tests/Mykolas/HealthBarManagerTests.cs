using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class HealthBarManagerEditModeTest
{
    private GameObject healthBarManagerObject;
    private HealthBarManager healthBarManager;
    private MockPlayerHealthController mockPlayerController;
    private Image healthBarFill;
    private TextMeshProUGUI healthMeterText;

    // Make the mock inherit from PlayerHealthController
    public class MockPlayerHealthController : PlayerHealthController
    {
        public new int currHP = 100;
        public new int maxHP = 100;

        public void SetHealth(int current, int max)
        {
            currHP = current;
            maxHP = max;
        }
    }

    [SetUp]
    public void Setup()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 1. First create the UI components
        var canvasObject = new GameObject("Canvas");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create health bar fill image as a child of Canvas
        var fillObject = new GameObject("HealthBarFill");
        fillObject.transform.SetParent(canvasObject.transform);
        healthBarFill = fillObject.AddComponent<Image>();

        // Create health text as a child of Canvas
        var textObject = new GameObject("HealthText");
        textObject.transform.SetParent(canvasObject.transform);
        healthMeterText = textObject.AddComponent<TextMeshProUGUI>();

        // 2. Create the player controller
        var playerObject = new GameObject("Player");
        mockPlayerController = playerObject.AddComponent<MockPlayerHealthController>();

        // 3. Finally create the health bar manager
        healthBarManagerObject = new GameObject("HealthBarManager");
        healthBarManager = healthBarManagerObject.AddComponent<HealthBarManager>();

        // Assign all references
        healthBarManager.playerController = mockPlayerController;
        healthBarManager.healthBarFill = healthBarFill;
        healthBarManager.healthMeterText = healthMeterText;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up in reverse order of creation
        Object.DestroyImmediate(healthMeterText.gameObject);
        Object.DestroyImmediate(healthBarFill.gameObject);
        Object.DestroyImmediate(mockPlayerController.gameObject);
        Object.DestroyImmediate(healthBarManagerObject);
    }


    [Test]
    public void UpdateHealthBar_UpdatesTextCorrectly()
    {
        // Arrange
        mockPlayerController.SetHealth(50, 150);

        // Act
        healthBarManager.UpdateHealthBar(mockPlayerController.currHP, mockPlayerController.maxHP);

        // Assert
        Assert.AreEqual("50/150", healthMeterText.text);
    }

    [Test]
    public void UpdateHealthBar_HandlesZeroMaxHealth()
    {
        // Arrange
        mockPlayerController.SetHealth(0, 0);

        // Act
        healthBarManager.UpdateHealthBar(mockPlayerController.currHP, mockPlayerController.maxHP);

        // Assert
        Assert.AreEqual(0f, healthBarFill.fillAmount);
        Assert.AreEqual("0/0", healthMeterText.text);
    }

    [Test]
    public void UpdateHealthBar_DoesNothingWhenHealthBarFillIsNull()
    {
        // Arrange
        healthBarManager.healthBarFill = null;
        mockPlayerController.SetHealth(80, 100);

        // Act & Assert (should not throw)
        Assert.DoesNotThrow(() =>
            healthBarManager.UpdateHealthBar(mockPlayerController.currHP, mockPlayerController.maxHP));
    }

    [Test]
    public void UpdateHealthBar_DoesNothingWhenHealthMeterTextIsNull()
    {
        // Arrange
        healthBarManager.healthMeterText = null;
        mockPlayerController.SetHealth(25, 50);

        // Act & Assert (should not throw)
        Assert.DoesNotThrow(() =>
            healthBarManager.UpdateHealthBar(mockPlayerController.currHP, mockPlayerController.maxHP));
    }
}