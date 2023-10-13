using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupController : MonoBehaviour
{
    private static UIPopupController _instance;
    public static UIPopupController instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void init()
    {

    }
}


public class UIPopup
{
    public string name;
    public bool isActive;
}
