using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

public class WeaponControllerTests
{
    private GameObject weaponObj;
    private WeaponController weapon;

    [SetUp]
    public void SetUp()
    {
        // Create the weapon GameObject
        weaponObj = new GameObject();
        weapon = weaponObj.AddComponent<WeaponController>();

        weapon.speed = 5f;
        weapon.damage = 10;
    }

    [TearDown]
    public void TearDown()
    {
        // Use DestroyImmediate to avoid issues in Edit Mode
        if (weaponObj != null)
        {
            Object.DestroyImmediate(weaponObj);
        }
    }

    [Test]
    public void GetPropertyCount_ReturnsCorrectPropertyCount()
    {
        FieldInfo[] properties = typeof(WeaponController).GetFields();

        Assert.AreEqual(properties.Length, 4, "More properties were added or removed");
    }


}
