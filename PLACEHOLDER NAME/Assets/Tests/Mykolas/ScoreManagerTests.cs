using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreManagerEditModeTest
{
    [SetUp]
    public void Setup()
    {
        // Reset the score before each test
        ScoreManager.currScore = 0;
        TimerManager.minutes = 0;
    }

    [Test]
    public void AddScore_IncrementsCurrentScore()
    {
        ScoreManager.addScore(10);
        Assert.AreEqual(10, ScoreManager.currScore);

        ScoreManager.addScore(5);
        Assert.AreEqual(15, ScoreManager.currScore);
    }

    [Test]
    public void Update_ScoreTextNotNull_UpdatesTextWithCorrectScore()
    {
        GameObject scoreManagerGO = new GameObject("ScoreManagerGO");

        ScoreManager scoreManager = scoreManagerGO.AddComponent<ScoreManager>();


        GameObject textGO = new GameObject("ScoreText");
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();

        scoreManager.scoreText = tmp;

        ScoreManager.currScore = 25;
        scoreManager.Update();

        Assert.AreEqual("Score: 25", tmp.text);

        Object.DestroyImmediate(scoreManagerGO);
        Object.DestroyImmediate(textGO);
    }

    [Test]
    public void Update_ScoreTextIsNull_DoesNotThrowError()
    {
        GameObject scoreManagerGO = new GameObject("ScoreManagerGO");

        ScoreManager scoreManager = scoreManagerGO.AddComponent<ScoreManager>();

        scoreManager.scoreText = null;
        ScoreManager.currScore = 50;

        Assert.DoesNotThrow(() => scoreManager.Update());
        Object.DestroyImmediate(scoreManagerGO);
    }
}
