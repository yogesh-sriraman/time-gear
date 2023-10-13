using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLevels : MonoBehaviour
{
    [SerializeField]
    GameObject showLevels;
    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {

            showLevels.SetActive(true);
        }
    }
}
