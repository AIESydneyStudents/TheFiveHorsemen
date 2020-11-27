using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTo : MonoBehaviour
{
    public void SetScene(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
    }
}
