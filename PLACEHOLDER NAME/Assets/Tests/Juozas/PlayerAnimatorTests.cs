using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Animations;

public class PlayerAnimatorTests
{
    private PlayerAnimator playerAnimator;
    private Animator mockAnimator;
    private PlayerMovementController mockMovementController;
    private GameObject testObject;

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriame naują gameobject testavimui ir priskiriame jam komponentus
        testObject = new GameObject();
        playerAnimator = testObject.AddComponent<PlayerAnimator>();
        mockAnimator = testObject.AddComponent<Animator>();
        mockMovementController = testObject.AddComponent<PlayerMovementController>();

        var controller = new AnimatorController();
        controller.AddLayer("Test Layer");
        controller.AddParameter("MoveRight", AnimatorControllerParameterType.Bool);
        controller.AddParameter("MoveLeft", AnimatorControllerParameterType.Bool);
        controller.AddParameter("MoveUp", AnimatorControllerParameterType.Bool);
        controller.AddParameter("MoveDown", AnimatorControllerParameterType.Bool);
        mockAnimator.runtimeAnimatorController = controller;

        mockAnimator.SetBool("MoveRight", false);
        mockAnimator.SetBool("MoveLeft", false);
        mockAnimator.SetBool("MoveUp", false);
        mockAnimator.SetBool("MoveDown", false);

        playerAnimator.Start();

        mockMovementController.playerInput = Vector2.zero;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testObject);
    }

    // Ar priskirti komponentai paleidus Start
    [Test]
    public void PlayerAnimator_InitializesComponentsInStart()
    {
        var testObj = new GameObject();
        var animator = testObj.AddComponent<Animator>();
        var movementController = testObj.AddComponent<PlayerMovementController>();
        var playerAnim = testObj.AddComponent<PlayerAnimator>();

        playerAnim.am = null;
        playerAnim.pc = null;

        playerAnim.Start();

        Assert.IsNotNull(playerAnim.am);
        Assert.IsNotNull(playerAnim.pc);
        Assert.AreEqual(animator, playerAnim.am);
        Assert.AreEqual(movementController, playerAnim.pc);

        Object.DestroyImmediate(testObj);
    }

    // Ar Update teisingai nustato animatoriaus parametrus, kai žaidėjas bando judėti į dešinę
    [Test]
    public void Update_SetsMoveRightTrue_WhenInputXIsPositive()
    {
        mockMovementController.playerInput = new Vector2(1f, 0f);

        playerAnimator.Update();

        Assert.IsTrue(mockAnimator.GetBool("MoveRight"));
        Assert.IsFalse(mockAnimator.GetBool("MoveLeft"));
        Assert.IsFalse(mockAnimator.GetBool("MoveUp"));
        Assert.IsFalse(mockAnimator.GetBool("MoveDown"));
    }

    // Ar Update teisingai nustato animatoriaus parametrus, kai žaidėjas bando judėti į kairę
    [Test]
    public void Update_SetsMoveLeftTrue_WhenInputXIsNegative()
    {
        mockMovementController.playerInput = new Vector2(-1f, 0f);

        playerAnimator.Update();

        Assert.IsFalse(mockAnimator.GetBool("MoveRight"));
        Assert.IsTrue(mockAnimator.GetBool("MoveLeft"));
        Assert.IsFalse(mockAnimator.GetBool("MoveUp"));
        Assert.IsFalse(mockAnimator.GetBool("MoveDown"));
    }

    // Ar Update teisingai nustato animatoriaus parametrus, kai žaidėjas bando judėti į viršų
    [Test]
    public void Update_SetsMoveUpTrue_WhenInputYIsPositive()
    {
        mockMovementController.playerInput = new Vector2(0f, 1f);

        playerAnimator.Update();

        Assert.IsFalse(mockAnimator.GetBool("MoveRight"));
        Assert.IsFalse(mockAnimator.GetBool("MoveLeft"));
        Assert.IsTrue(mockAnimator.GetBool("MoveUp"));
        Assert.IsFalse(mockAnimator.GetBool("MoveDown"));
    }

    // Ar Update teisingai nustato animatoriaus parametrus, kai žaidėjas bando judėti į apačią
    [Test]
    public void Update_SetsMoveDownTrue_WhenInputYIsNegative()
    {
        mockMovementController.playerInput = new Vector2(0f, -1f);

        playerAnimator.Update();

        Assert.IsFalse(mockAnimator.GetBool("MoveRight"));
        Assert.IsFalse(mockAnimator.GetBool("MoveLeft"));
        Assert.IsFalse(mockAnimator.GetBool("MoveUp"));
        Assert.IsTrue(mockAnimator.GetBool("MoveDown"));
    }

    // Ar Update teisingai nustato animatoriaus parametrus, kai žaidėjas niekur nebando judėti
    [Test]
    public void Update_SetsAllFalse_WhenInputIsZero()
    {
        mockMovementController.playerInput = Vector2.zero;

        mockAnimator.SetBool("MoveRight", true);
        mockAnimator.SetBool("MoveLeft", true);
        mockAnimator.SetBool("MoveUp", true);
        mockAnimator.SetBool("MoveDown", true);

        playerAnimator.Update();

        Assert.IsFalse(mockAnimator.GetBool("MoveRight"));
        Assert.IsFalse(mockAnimator.GetBool("MoveLeft"));
        Assert.IsFalse(mockAnimator.GetBool("MoveUp"));
        Assert.IsFalse(mockAnimator.GetBool("MoveDown"));
    }

    // Ar Update teisingai nustato animatoriaus parametrus, kai žaidėjas bando judėti
    // į viršų ir į dešinę vienu metu
    [Test]
    public void Update_PrioritizesXInput_WhenBothXAndYAreNonZero()
    {
        mockMovementController.playerInput = new Vector2(1f, 1f);

        playerAnimator.Update();

        Assert.IsTrue(mockAnimator.GetBool("MoveRight"));
        Assert.IsFalse(mockAnimator.GetBool("MoveLeft"));
        Assert.IsFalse(mockAnimator.GetBool("MoveUp"));
        Assert.IsFalse(mockAnimator.GetBool("MoveDown"));
    }
}
