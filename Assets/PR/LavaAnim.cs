using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaAnim : MonoBehaviour
{
    [Range(0, 10)]
    public float brightnessLavaTop_Min;
    [Range(0, 10)]
    public float brightnessLavaTop_Max;

    public float lavaTopSpeed;

    Material lavaMat;
    float currentVal;
    bool isIncrementing;

    // Start is called before the first frame update
    void Start()
    {
        lavaMat = GetComponent<MeshRenderer>().material; // .GetComponent<Material>();
        currentVal = brightnessLavaTop_Min;
        isIncrementing = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentVal += (Time.deltaTime * lavaTopSpeed) * (isIncrementing ? 1 : -1);

        lavaMat.SetFloat("_StrengthTop", currentVal);

        // Bounds checking
        if (currentVal > brightnessLavaTop_Max)
        {
            currentVal = brightnessLavaTop_Max;
            isIncrementing = false;
        }
        else if (currentVal < brightnessLavaTop_Min)
        {
            currentVal = brightnessLavaTop_Min;
            isIncrementing = true;
        }
    }
}
