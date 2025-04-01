using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyDamageTests
{
    private GameObject enemyObj;
    private EnemyDamage enemyDamage;
    private GameObject playerObj;
    private MockPlayerHealth mockPlayerHealth;

    // Testavimui skirta PlayerHealthController klasė
    public class MockPlayerHealth : PlayerHealthController
    {
        public int currentHealth = 100;
        public int lastDamageTaken;
        
        public override void TakeDamage(int damage)
        {
            lastDamageTaken = damage;
            currentHealth -= damage;
        }
    }

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriame priešo objektą
        enemyObj = new GameObject();
        enemyDamage = enemyObj.AddComponent<EnemyDamage>();
        
        // Sukuriame žaidėjo objektą su testavimui skirtu komponentu
        playerObj = new GameObject();
        playerObj.tag = "Player";
        mockPlayerHealth = playerObj.AddComponent<MockPlayerHealth>();
        
        // Priskiriame priešo kintamiesiems reikšmes
        enemyDamage.pc = mockPlayerHealth;
        enemyDamage.damage = 10;
        enemyDamage.attackSpeed = 1f;
        enemyDamage.timer = 0;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyObj);
        Object.DestroyImmediate(playerObj);
    }

    // Ar Start priskiria teisingas reikšmes
    [Test]
    public void Start_InitializesPlayerReferenceAndTimer()
    {
        var testObj = new GameObject();
        var testDamage = testObj.AddComponent<EnemyDamage>();
        var player = new GameObject().AddComponent<MockPlayerHealth>();

        testDamage.Start();

        Assert.IsNotNull(testDamage.pc);
        Assert.AreEqual(0f, testDamage.timer);
        
        Object.DestroyImmediate(testObj);
        Object.DestroyImmediate(player.gameObject);
    }

    // Ar įvykus kolizijai priešas sumažina žaidėjo gyvybių kiekį, kai priešas yra pasiruošęs atakuoti
    [Test]
    public void OnCollisionStay2D_DamagesPlayer_WhenTimerExpires()
    {
        enemyDamage.timer = 0; // Pasiruošęs atakuoti

        enemyDamage.SimulateCollision(playerObj);

        Assert.AreEqual(90, mockPlayerHealth.currentHealth); // Nes 100 - 10
        Assert.AreEqual(10, mockPlayerHealth.lastDamageTaken);
    }

    // Ar įvykus kolizijai, kai priešas atakuoja, jo timer yra prilyginamas jo attackspeed
    [Test]
    public void OnCollisionStay2D_ResetsTimer_AfterDamage()
    {
        enemyDamage.timer = 0;

        enemyDamage.SimulateCollision(playerObj);

        Assert.AreEqual(1f, enemyDamage.timer); // Nes attackSpeed = 1f
    }

    // Ar įvykus kolizijai, kai priešas nėra pasiruošęs atakuoti, žaidėjo gyvybių kiekis nepasikeičia
    [Test]
    public void OnCollisionStay2D_DoesNotDamage_WhenTimerNotExpired()
    {
        enemyDamage.timer = 0.5f; // Nepasiruošęs atakuoti

        enemyDamage.SimulateCollision(playerObj);

        Assert.AreEqual(100, mockPlayerHealth.currentHealth);
    }

    // Ar įvykus kolizijai, kai priešas nėra pasiruošęs atakuoti, priešo timer sumažėja
    [Test]
    public void OnCollisionStay2D_DecrementsTimer_WhenColliding()
    {
        // Arrange
        enemyDamage.timer = 0.5f;

        // Act
        enemyDamage.SimulateCollision(playerObj);

        // Assert
        Assert.Less(enemyDamage.timer, 0.5f);
    }

    // Ar įvykus kolizijai ne su žaidėju, kai priešas yra pasiruošęs atakuoti,
    // žaidėjo gyvybių kiekis nepasikeičia
    [Test]
    public void OnCollisionStay2D_IgnoresNonPlayerCollisions()
    {
        enemyDamage.timer = 0;
        var nonPlayer = new GameObject(); // Neturi "Player" tag

        enemyDamage.SimulateCollision(nonPlayer);

        Assert.AreEqual(100, mockPlayerHealth.currentHealth);
        Object.DestroyImmediate(nonPlayer);
    }
}
