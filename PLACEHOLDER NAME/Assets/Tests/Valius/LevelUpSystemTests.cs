using NUnit.Framework;
using UnityEngine;

public class LevelUpSystemTests
{
    [SetUp]
    public void Setup()
    {
        // nunuliname patirties taškus prieš kiekvieną testą
        LevelUpSystem.experience = 0;
    }
    [Test]
    public void GainXP_DidinamasPatirtiesTaskuKiekis()
    {
        // paruošiame lygių sistemą
        var system = new GameObject().AddComponent<LevelUpSystem>();
       
        system.experienceToNextLevel = 100;

        // pridedame patirties taškų
        system.GainExperience(50);

        // tikriname ar teisingai sumažėjo
        Assert.AreEqual(50, LevelUpSystem.experience);
    }
    
}