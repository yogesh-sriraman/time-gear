using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpin : MonoBehaviour
{
    public GameObject pathTile;
    public float zVal;

    private void Start()
    {
        StartCoroutine(keepRotating());
    }
    private IEnumerator keepRotating()
    {
        while(true)
        {
            transform.Rotate(new Vector3(0, 0, zVal));
            yield return new WaitForSeconds(0.01f);
        }
    }
}
