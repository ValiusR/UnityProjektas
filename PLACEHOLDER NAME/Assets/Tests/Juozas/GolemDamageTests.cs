using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Animations;

public class GolemDamageTests
{
    private GameObject golemObj;
    private GolemDamage golemDamage;
    private GameObject playerObj;
    private MockPlayerHealth mockPlayerHealth;
    private Animator mockAnimator;

    // Testavimui skirta PlayerHealthController klasė
    public class MockPlayerHealth : PlayerHealthController
    {
        public int currentHealth = 100;
        public int damageTaken;

        public override void TakeDamage(int damage)
        {
            damageTaken = damage;
            currentHealth -= damage;
        }
    }

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriamas žaidėjas
        playerObj = new GameObject("Player");
        mockPlayerHealth = playerObj.AddComponent<MockPlayerHealth>();
        playerObj.AddComponent<PlayerMovementController>();

        // Sukuriamas golemas
        golemObj = new GameObject("Golem");
        golemDamage = golemObj.AddComponent<GolemDamage>();

        mockAnimator = golemObj.AddComponent<Animator>();
        var controller = new AnimatorController();
        controller.AddLayer("Test Layer");
        controller.AddParameter("Move", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Attack", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        mockAnimator.runtimeAnimatorController = controller;
        golemDamage.am = mockAnimator;
        golemDamage.pc = mockPlayerHealth;

        golemDamage.damage = 15;
        golemDamage.attackSpeed = 1.5f;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(golemObj);
        Object.DestroyImmediate(playerObj);
    }

    // Ar Start metodoas teisingą greičio reikšmę priskiria kintamajam
    [Test]
    public void Start_Initializes_Animator_Speed()
    {
        golemDamage.Start();

        Assert.AreEqual(1.5f, mockAnimator.GetFloat("Speed"));
    }

    // Ar atakos daro tinkamą kiekį žalos
    [Test]
    public void Attack_Deals_Correct_Damage()
    {
        int initialHealth = 100;
        mockPlayerHealth.currentHealth = initialHealth;

        golemDamage.Attack();

        Assert.AreEqual(initialHealth - 15, mockPlayerHealth.currentHealth);
    }

    // Ar paveldėtas OnCollisionStay2D metodas nieko nedaro,
    // nes skiriasi golemo atakavimas nuo kitų priešų
    // (jis atakuoja naudodamas kitą skirptą nei įprastai)
    [Test]
    public void OnCollisionStay2D_No_Base_Behavior()
    {
        var collision = new GameObject().AddComponent<BoxCollider2D>();
        int initialHealth = 100;
        mockPlayerHealth.currentHealth = initialHealth;

        golemDamage.OnCollisionStay2D(new Collision2D());

        Assert.AreEqual(initialHealth, mockPlayerHealth.currentHealth);
        Object.DestroyImmediate(collision.gameObject);
    }
}
