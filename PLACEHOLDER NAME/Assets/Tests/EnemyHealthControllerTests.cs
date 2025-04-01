using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyHealthControllerTests
{
    private GameObject enemyObj;
    private EnemyHealthController healthController;
    private MockDamageBlink mockBlink;
    private MockFadeOut mockFade;

    // Testavimui skirta DamageBlink klasė
    public class MockDamageBlink : DamageBlink
    {
        public bool wasCalled;
        public override void PlayBlink()
        {
            wasCalled = true;
        }
    }

    // Testavimui skirta FadeOut klasė
    public class MockFadeOut : FadeOut
    {
        public bool fadeStarted;
        public override IEnumerator FadeAnimation()
        {
            fadeStarted = true;
            yield return null;
        }
    }

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriame priešo objektą ir pridedame jam komponentus
        enemyObj = new GameObject();
        enemyObj.AddComponent<BoxCollider2D>();
        healthController = enemyObj.AddComponent<EnemyHealthController>();

        mockBlink = enemyObj.AddComponent<MockDamageBlink>();
        mockFade = enemyObj.AddComponent<MockFadeOut>();

        healthController.damageBlink = mockBlink;
        healthController.fadeOut = mockFade;

        // Priskiriame reikšmes
        healthController.maxHP = 100;
        healthController.currHP = 100;
        healthController.scorePoints = 50;
        healthController.xp = 20;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyObj);
    }

    // Ar priešo gyvybių kiekis teisingai sumažėja po TakeDamage metodo
    [Test]
    public void TakeDamage_ReducesCurrentHP()
    {
        healthController.TakeDamage(30);

        Assert.AreEqual(70, healthController.currHP); // 100 - 30
    }

    // Ar po TakeDamage metodo iškviečiamas PlayBlink metodas
    [Test]
    public void TakeDamage_TriggersDamageBlink()
    {
        healthController.TakeDamage(10);

        Assert.IsTrue(mockBlink.wasCalled);
    }

    // Ar po TakeDamage metodo prasidėjo mirties animacija
    [Test]
    public void TakeDamage_WhenHPReachesZero_StartsDeathAnimation()
    {
        healthController.TakeDamage(100);

        Assert.IsTrue(mockFade.fadeStarted);
    }

    // Ar išjungiami visi reikalingi komponentai, kai priešo gyvybių kiekis pasiekia 0
    [Test]
    public void TakeDamage_WhenHPReachesZero_DisablesComponents()
    {
        var movement = enemyObj.AddComponent<EnemyMovement>();
        var skeleton = enemyObj.AddComponent<SkeletonController>();
        var animator = enemyObj.AddComponent<Animator>();
        var collider = enemyObj.GetComponent<BoxCollider2D>();

        healthController.TakeDamage(100);

        Assert.IsFalse(movement.enabled);
        Assert.IsFalse(skeleton.enabled);
        Assert.IsFalse(animator.enabled);
        Assert.IsFalse(collider.enabled);
    }
}
