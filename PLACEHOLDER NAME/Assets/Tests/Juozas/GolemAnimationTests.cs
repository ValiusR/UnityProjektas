using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Animations;

public class GolemAnimationTests
{
    private GameObject golemObj;
    private GolemAnimation golemAnim;
    private GameObject playerObj;
    private Animator mockAnimator;
    private SpriteRenderer mockSpriteRenderer;

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriame žaidėją
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<PlayerMovementController>();
        playerObj.transform.position = Vector3.zero;

        // Sukuriame golemą
        golemObj = new GameObject("Golem");
        golemAnim = golemObj.AddComponent<GolemAnimation>();

        // Paruošiame reikiamus komponentus
        mockAnimator = golemObj.AddComponent<Animator>();
        var controller = new AnimatorController();
        controller.AddLayer("Test Layer");
        controller.AddParameter("Move", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Attack", AnimatorControllerParameterType.Bool);
        mockAnimator.runtimeAnimatorController = controller;
        golemAnim.am = mockAnimator;
        golemAnim.am.SetBool("Move", true);
        golemAnim.am.SetBool("Attack", false);

        mockSpriteRenderer = golemObj.AddComponent<SpriteRenderer>();
        golemAnim.sr = mockSpriteRenderer;
        golemAnim.player = playerObj.GetComponent<PlayerMovementController>().transform;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(golemObj);
        Object.DestroyImmediate(playerObj);
    }

    // Ar Start metodas priskiria komponentus ir nustato Move į true
    [Test]
    public void Start_Initializes_Components_And_Sets_Move_True()
    {
        golemAnim.Start();

        Assert.IsNotNull(golemAnim.GetComponent<Animator>());
        Assert.IsNotNull(golemAnim.GetComponent<SpriteRenderer>());
        Assert.IsTrue(mockAnimator.GetBool("Move"));
    }

    // Ar Update metodas apverčia golemo sprite horizontaliai, kai žaidėjas yra po dešine
    [Test]
    public void Update_Flips_Sprite_When_Player_Is_Right()
    {
        playerObj.transform.position = new Vector3(1f, 0f, 0f); // Player to the right
        golemObj.transform.position = Vector3.zero;

        golemAnim.Update();

        Assert.IsTrue(mockSpriteRenderer.flipX);
    }

    // Ar Update metodas apverčia golemo sprite horizontaliai, kai žaidėjas yra po kaire
    [Test]
    public void Update_Unflips_Sprite_When_Player_Is_Left()
    {
        playerObj.transform.position = new Vector3(-1f, 0f, 0f); // Player to the left
        golemObj.transform.position = Vector3.zero;

        golemAnim.Update();

        Assert.IsFalse(mockSpriteRenderer.flipX);
    }

    // Ar nustatomas Attack animacija, kai įvyksta kolizija su žaidėju
    [Test]
    public void OnCollisionEnter2D_Sets_Attack_True_When_Player_Collides()
    {
        golemAnim.SimulateCollisionEnter(playerObj);

        Assert.IsTrue(mockAnimator.GetBool("Attack"));
        Assert.IsFalse(mockAnimator.GetBool("Move"));
    }

    // Ar grįžtama į Move animaciją, kai pasibaigia kolizija su žaidėju
    [Test]
    public void OnCollisionExit2D_Resets_To_Move_When_Player_Leaves()
    {
        mockAnimator.SetBool("Attack", true);
        mockAnimator.SetBool("Move", false);

        golemAnim.SimulateCollisionExit(playerObj);

        Assert.IsFalse(mockAnimator.GetBool("Attack"));
        Assert.IsTrue(mockAnimator.GetBool("Move"));
    }

    // Ar animacijos nekinta, kai vyksta kolizija ne su žaidėju
    [Test]
    public void OnCollision_Ignores_NonPlayer_Collisions()
    {
        var wall = new GameObject("Wall");

        golemAnim.SimulateCollisionEnter(wall);
        golemAnim.SimulateCollisionExit(wall);

        Assert.IsFalse(mockAnimator.GetBool("Attack"));
        Assert.IsTrue(mockAnimator.GetBool("Move")); // Visdar nepakitęs nuo Start metodo
        Object.DestroyImmediate(wall);
    }
}
