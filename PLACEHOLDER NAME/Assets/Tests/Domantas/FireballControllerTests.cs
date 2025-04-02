using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

public class FireBallControllerTests
{
    private GameObject weaponObj;
    private FireBallController fireBall;
    private GameObject prefabMock;

    [SetUp]
    public void SetUp()
    {
        weaponObj = new GameObject();
        fireBall = weaponObj.AddComponent<FireBallController>();

        prefabMock = new GameObject();
        FireBallBehaviour prefabStats = prefabMock.AddComponent<FireBallBehaviour>();

        prefabStats.damage = 10;
        prefabStats.speed = 5;

        fireBall.damage = 10;
        fireBall.speed = 5;
        fireBall.prefab = prefabMock;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(weaponObj);
        Object.DestroyImmediate(prefabMock);
    }

    [Test]
    public void Attack_SpawnsFireball_WithCorrectStats()
    {
        fireBall.Invoke("Attack", 0f);

        FireBallBehaviour spawnedBall = Object.FindObjectOfType<FireBallBehaviour>();

        Assert.IsNotNull(spawnedBall, "Wave should be instantiated");
        Assert.AreEqual(fireBall.damage, spawnedBall.damage, "Wave damage should match weapon damage");
        Assert.AreEqual(fireBall.speed, spawnedBall.speed, "Wave speed should match weapon speed");

        Object.DestroyImmediate(spawnedBall.gameObject);
    }

    [Test]
    public void GetDescription_ReturnsExpectedString()
    {
        string expected = $"Fires a ball of fire that deals 10 damage per hit.";
        Assert.AreEqual(expected, fireBall.GetDescription());
    }

    [Test]
    public void GetName_ReturnsFreezeWave()
    {
        Assert.AreEqual("Fireball", fireBall.GetName());
    }

    [Test]
    public void GetPropertyCount_ReturnsCorrectPropertyCount()
    {
        FieldInfo[] properties = typeof(FreezeWaveController).GetFields();

        Assert.AreEqual(properties.Length, 6, "More properties were added or removed");
    }
}
