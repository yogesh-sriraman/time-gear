using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private string sceneToLoad;

    public Image backgroundBG;

    private static LevelChanger _instance;
    public static LevelChanger instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void FadeScene (string sceneName)
    {
        sceneToLoad = sceneName;
        animator.SetBool("CanFadeOut", true); 
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OpenLevel(string levelName)
    {
        AudioManager.instance.PlaySound("Button Click1");
        GameController.instance.loadLevel(levelName);
    }
}
