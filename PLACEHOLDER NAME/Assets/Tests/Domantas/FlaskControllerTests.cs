using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

public class FlaskControllerTests
{
    private GameObject weaponObj;
    private MagicFlaskController flask;
    private GameObject prefabMock;

    [SetUp]
    public void SetUp()
    {
        // Create weapon GameObject
        weaponObj = new GameObject();
        flask = weaponObj.AddComponent<MagicFlaskController>();

        // Create a mock prefab for the freeze wave
        prefabMock = new GameObject();
        MagicFlaskBehaviour prefabStats = prefabMock.AddComponent<MagicFlaskBehaviour>();

        prefabStats.damage = 10;
        prefabStats.damage = 10;
        prefabStats.speed = 5;

        // Set weapon stats
        flask.damage = 10;
        flask.speed = 5;
        flask.prefab = prefabMock;
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
        // Call attack
        flask.Invoke("Attack", 0f);

        // Find spawned object
        MagicFlaskBehaviour flaskProj = Object.FindObjectOfType<MagicFlaskBehaviour>();

        Assert.IsNotNull(flask, "Wave should be instantiated");
        Assert.AreEqual(flaskProj.damage, flask.damage, "Wave damage should match weapon damage");
        Assert.AreEqual(flaskProj.speed, flask.speed, "Wave speed should match weapon speed");

        // Cleanup
        Object.DestroyImmediate(flask.gameObject);
    }

    [Test]
    public void GetDescription_ReturnsExpectedString()
    {
        string expected = $"Sends out a magic flask, that explodes in to a damaging zone.";
        Assert.AreEqual(expected, flask.GetDescription());
    }

    [Test]
    public void GetName_ReturnsExpectedString()
    {
        Assert.AreEqual("Magic flask", flask.GetName());
    }

    [Test]
    public void GetPropertyCount_ReturnsCorrectPropertyCount()
    {
        FieldInfo[] properties = typeof(MagicFlaskController).GetFields();

        Assert.AreEqual(properties.Length, 4, "More properties were added or removed");
    }
}
