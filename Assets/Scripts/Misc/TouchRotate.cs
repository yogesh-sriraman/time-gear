using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchRotate : MonoBehaviour
{
    [SerializeField]
    GameObject Character;

    [SerializeField]
    Animator animator;

    [SerializeField]
    GameObject levelChanger;

    public int angle = 180;
    public float time = 1.5f;
    static int count = 0;
    float rotationAmount;
    Quaternion initialRotation;
    bool canPlayAnimation;
    bool helloComplete;
    bool isRotating;
    

    private void Start()
    {
        isRotating = false;
        helloComplete = false;
        canPlayAnimation = true;
        initialRotation = transform.rotation;
        rotationAmount = (float)angle / 360;
    }
    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            helloComplete = true;
        if(count == 0)
        {
            animator.SetBool("wobbleForever", true);
            helloComplete = false;
            iTween.RotateBy(this.gameObject, iTween.Hash(
                    "z", -7,
                    "speed", 400f,
                    "easetype", iTween.EaseType.easeInSine
                    ));
            iTween.RotateBy(Character, iTween.Hash(
                "y", -7,
                "speed", 400f,
                "easetype", iTween.EaseType.easeInSine,
                "onstart", "CallFade",
                "onstarttarget", gameObject,
                "onstartparams", "ChapterSelect",
                "oncomplete", "ResetBool",
                "oncompletetarget", gameObject
                ));
            count++;
        }
    }
    //private void OnMouseDown()
    //{
    //    if(canPlayAnimation && helloComplete)
    //    {
    //        canPlayAnimation = false;
    //        if(count<2)
    //        {
    //            isRotating = true;
    //            animator.SetBool("isRotating", isRotating);
    //            iTween.RotateBy(this.gameObject, iTween.Hash(
    //                "z", rotationAmount,
    //                "time", 1.5f*2,
    //                "easetype", iTween.EaseType.easeOutBack
    //                ));
    //            iTween.RotateBy(Character, iTween.Hash(
    //                "y", rotationAmount,
    //                "time", time*2,
    //                "easetype", iTween.EaseType.easeOutBack,
    //                "oncomplete", "ResetBool",
    //                "oncompletetarget", gameObject
    //                ));
    //        }
    //    }
    //}
    void ResetBool()
    {
        isRotating = false;
        animator.SetBool("isRotating", isRotating);
        Debug.Log("It reached");
        canPlayAnimation = true;
        count++;
    }
    void CallFade(string sceneName)
    {
        StartCoroutine(FadeScene(sceneName));
    }
    private IEnumerator FadeScene(string sceneName)
    {
        yield return new WaitForSeconds(4);
        levelChanger.GetComponent<LevelChanger>().FadeScene(sceneName);
    }
}
