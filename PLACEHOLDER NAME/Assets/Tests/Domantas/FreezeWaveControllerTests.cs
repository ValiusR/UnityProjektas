using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

public class FreezeWaveControllerTests
{
    private GameObject weaponObj;
    private FreezeWaveController freezeWave;
    private GameObject prefabMock;

    [SetUp]
    public void SetUp()
    {
        weaponObj = new GameObject();
        freezeWave = weaponObj.AddComponent<FreezeWaveController>();

        prefabMock = new GameObject();
        FreezeWaveBehaviour prefabStats = prefabMock.AddComponent<FreezeWaveBehaviour>();

        prefabStats.damage = 10;
        prefabStats.damage = 10;
        prefabStats.speed = 5;
        prefabStats.freezeLength = 3f;
        prefabStats.freezeStrength = 0.5f;

        freezeWave.damage = 10;
        freezeWave.speed = 5;
        freezeWave.freezeLength = 3f;
        freezeWave.freezeStrength = 0.5f;
        freezeWave.prefab = prefabMock;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(weaponObj);
        Object.DestroyImmediate(prefabMock);
    }

    [Test]
    public void Attack_SpawnsWave_WithCorrectStats()
    {
        freezeWave.Invoke("Attack", 0f);

        FreezeWaveBehaviour spawnedWave = Object.FindObjectOfType<FreezeWaveBehaviour>();

        Assert.IsNotNull(spawnedWave, "Wave should be instantiated");
        Assert.AreEqual(freezeWave.damage, spawnedWave.damage, "Wave damage should match weapon damage");
        Assert.AreEqual(freezeWave.speed, spawnedWave.speed, "Wave speed should match weapon speed");
        Assert.AreEqual(freezeWave.freezeLength, spawnedWave.freezeLength, "Wave freeze length should match weapon freeze length");
        Assert.AreEqual(freezeWave.freezeStrength, spawnedWave.freezeStrength, "Wave freeze strength should match weapon freeze strength");

        Object.DestroyImmediate(spawnedWave.gameObject);
    }

    [Test]
    public void GetDescription_ReturnsExpectedString()
    {
        string expected = "Deals 10 damage per hit. Slows down hit enemies.";
        Assert.AreEqual(expected, freezeWave.GetDescription());
    }

    [Test]
    public void GetName_ReturnsExpectedString()
    {
        Assert.AreEqual("Freeze wave", freezeWave.GetName());
    }

    [Test]
    public void GetPropertyCount_ReturnsCorrectPropertyCount()
    {
        FieldInfo[] properties = typeof(FreezeWaveController).GetFields();

        Assert.AreEqual(properties.Length, 6, "More properties were added or removed");
    }
}
