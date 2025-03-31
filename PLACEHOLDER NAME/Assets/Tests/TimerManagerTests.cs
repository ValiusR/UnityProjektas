using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NUnit.Framework;
using UnityEngine;
using TMPro;

using NUnit.Framework;
using UnityEngine;
using TMPro;

public class TimerManagerEditModeTest
{
    [SetUp]
    public void Setup()
    {
        TimerManager.seconds = 0;
        TimerManager.minutes = 0;
        TimerManager.elapsedTime = 0f; 
        TimerManager.formattedTime = "00:00";
    }

    [Test]
    public void Update_IncrementsTimeCorrectly()
    {
        GameObject timerManagerGO = new GameObject("TimerManagerGO");
        TimerManager timerManager = timerManagerGO.AddComponent<TimerManager>();

        GameObject textGO = new GameObject("TimerText");
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        timerManager.timerText = tmp;

        float deltaTime = 1f;
        TimerManager.elapsedTime = deltaTime;
        timerManager.Update();
        Assert.AreEqual(0, TimerManager.minutes);
        Assert.AreEqual(1, TimerManager.seconds);
        Assert.AreEqual("00:01", TimerManager.formattedTime);

        deltaTime = 60.5f;
        TimerManager.elapsedTime = deltaTime;
        timerManager.Update();
        Assert.AreEqual(1, TimerManager.minutes);
        Assert.AreEqual(0, TimerManager.seconds);
        Assert.AreEqual("01:00", TimerManager.formattedTime);

        deltaTime = 125.3f;
        TimerManager.elapsedTime = deltaTime;
        timerManager.Update();
        Assert.AreEqual(2, TimerManager.minutes);
        Assert.AreEqual(5, TimerManager.seconds);
        Assert.AreEqual("02:05", TimerManager.formattedTime);

        Object.DestroyImmediate(timerManagerGO);
        Object.DestroyImmediate(textGO);
    }

    [Test]
    public void Update_TimerTextNotNull_UpdatesTextCorrectly()
    {
        GameObject timerManagerGO = new GameObject("TimerManagerGO");
        TimerManager timerManager = timerManagerGO.AddComponent<TimerManager>();

        GameObject textGO = new GameObject("TimerText");
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        timerManager.timerText = tmp;

        float deltaTime = 35f;
        TimerManager.elapsedTime = deltaTime;
        timerManager.Update();

        Assert.AreEqual("00:35", tmp.text);

        deltaTime = 92.8f;
        TimerManager.elapsedTime = deltaTime;
        timerManager.Update();
        Assert.AreEqual("01:32", tmp.text);

        Object.DestroyImmediate(timerManagerGO);
        Object.DestroyImmediate(textGO);
    }

    [Test]
    public void Update_TimerTextIsNull_DoesNotThrowError()
    {
        GameObject timerManagerGO = new GameObject("TimerManagerGO");
        TimerManager timerManager = timerManagerGO.AddComponent<TimerManager>();

        timerManager.timerText = null;
        Assert.DoesNotThrow(() => timerManager.Update());

        Object.DestroyImmediate(timerManagerGO);
    }
}