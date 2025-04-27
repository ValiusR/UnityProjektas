using UnityEngine;
using UnityEngine.Events; // Add this namespace

[System.Serializable]
public class CursedUpgrade
{
    public string name;
    public string description;
    public GameObject effectPrefab; // Optional visual effect
    public UnityEvent<GameObject> ApplyEffectEvent; // Change to UnityEvent

    public CursedUpgrade(string name, string description, GameObject effectPrefab = null)
    {
        this.name = name;
        this.description = description;
        this.effectPrefab = effectPrefab;
        this.ApplyEffectEvent = new UnityEvent<GameObject>(); // Initialize the UnityEvent
    }
}