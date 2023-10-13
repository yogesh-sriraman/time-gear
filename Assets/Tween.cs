using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        iTween.RotateBy(this.gameObject, iTween.Hash(
                    "y", 20,
                    "speed", 350f,
                    "easetype", iTween.EaseType.easeInSine
                    ));
    }


}
