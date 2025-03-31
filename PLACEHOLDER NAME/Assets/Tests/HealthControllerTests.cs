using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthControllerEditModeTest
{
    private GameObject playerGO;
    private PlayerHealthController playerHealthController;
    private MockDamageBlink mockDamageBlink;
    private MockDeathUiManager mockDeathUIManager;

    public class MockDamageBlink : DamageBlink
    {
        public int playBlinkCount = 0;

        public override void PlayBlink()
        {
            playBlinkCount++;
        }
    }

    public class MockDeathUiManager : DeathUiManager
    {
        public int showDeathUICount = 0;

        public override void ShowDeathUI()
        {
            showDeathUICount++;
        }
    }

    [SetUp]
    public void Setup()
    {
        // Create the test objects
        playerGO = new GameObject("Player");
        playerHealthController = playerGO.AddComponent<PlayerHealthController>();

        // Initialize health values
        playerHealthController.maxHP = 100;
        playerHealthController.currHP = 100;

        // Create and assign the mock DamageBlink
        var blinkGO = new GameObject("DamageBlink");
        mockDamageBlink = blinkGO.AddComponent<MockDamageBlink>();
        playerHealthController.damageBlink = mockDamageBlink;

        // Create and assign the mock DeathUiManager
        var deathUiGO = new GameObject("DeathUIManager");
        mockDeathUIManager = deathUiGO.AddComponent<MockDeathUiManager>();
        playerHealthController.deathUIManager = mockDeathUIManager;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up all objects
        Object.DestroyImmediate(playerGO);
        if (mockDamageBlink != null && mockDamageBlink.gameObject != null)
        {
            Object.DestroyImmediate(mockDamageBlink.gameObject);
        }
        if (mockDeathUIManager != null && mockDeathUIManager.gameObject != null)
        {
            Object.DestroyImmediate(mockDeathUIManager.gameObject);
        }
    }

    [Test]
    public void TakeDamage_CallsPlayBlink()
    {
        // Verify setup
        Assert.IsNotNull(playerHealthController.damageBlink, "damageBlink should be assigned");

        // First damage call
        playerHealthController.TakeDamage(10);
        Assert.AreEqual(1, mockDamageBlink.playBlinkCount);
        Assert.AreEqual(90, playerHealthController.currHP);

        // Second damage call
        playerHealthController.TakeDamage(20);
        Assert.AreEqual(2, mockDamageBlink.playBlinkCount);
        Assert.AreEqual(70, playerHealthController.currHP);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        playerHealthController.TakeDamage(30);
        Assert.AreEqual(70, playerHealthController.currHP);
    }

    [Test]
    public void TakeDamage_SetsHealthToZeroWhenDamageEqualsCurrentHealth()
    {
        playerHealthController.TakeDamage(100);
        Assert.AreEqual(0, playerHealthController.currHP);
        Assert.AreEqual(1, mockDamageBlink.playBlinkCount);
    }

    [Test]
    public void TakeDamage_SetsHealthToZeroWhenDamageExceedsCurrentHealth()
    {
        playerHealthController.TakeDamage(150);
        Assert.AreEqual(0, playerHealthController.currHP);
        Assert.AreEqual(1, mockDamageBlink.playBlinkCount);
    }

    [Test]
    public void TakeDamage_CallsPlayBlinkEvenWhenHealthBecomesZero()
    {
        playerHealthController.TakeDamage(100);
        Assert.AreEqual(1, mockDamageBlink.playBlinkCount);

        playerHealthController.currHP = 5;
        playerHealthController.TakeDamage(10);
        Assert.AreEqual(2, mockDamageBlink.playBlinkCount);
        Assert.AreEqual(0, playerHealthController.currHP);
    }

    [Test]
    public void TakeDamage_DoesNotCallShowDeathUI_WhenHealthIsAboveZero()
    {
        playerHealthController.TakeDamage(10);
        Assert.AreEqual(0, mockDeathUIManager.showDeathUICount);
    }

    [Test]
    public void TakeDamage_CallsShowDeathUI_WhenHealthBecomesZeroAndDeathUIManagerIsNotNull()
    {
        playerHealthController.TakeDamage(100);
        Assert.AreEqual(1, mockDeathUIManager.showDeathUICount);
    }
    
}