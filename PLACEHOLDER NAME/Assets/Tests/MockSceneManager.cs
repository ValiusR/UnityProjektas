using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MockSceneManager
{
    public List<string> loadedScenes = new List<string>();

    public void LoadScene(string sceneName, LoadSceneMode mode)
    {
        if (!loadedScenes.Contains(sceneName))
        {
            loadedScenes.Add(sceneName);
        }
    }

    public void UnloadScene(string sceneName)
    {
        loadedScenes.Remove(sceneName);
    }
}

