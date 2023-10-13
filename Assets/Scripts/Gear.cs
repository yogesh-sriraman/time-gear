using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public GameObject[] pathTile;
    public bool canMoveUpDown;
    public bool canMoveLeftRight;
    public GameObject attachedPath;
    public int gearDir;

    private Quaternion childRotation;
    private Vector3 childPosition;
    private Transform childTransform;

    private void Start()
    {
        childTransform = null;
        if (transform.childCount == 1)
        {
            childTransform = transform.GetChild(0);
            childRotation = childTransform.rotation;
            childPosition = childTransform.position;
        }
    }

    private void Update()
    {
        if (childTransform != null)
        {
            childTransform.rotation = childRotation;
            childTransform.position = childPosition;
        }
    }
}
