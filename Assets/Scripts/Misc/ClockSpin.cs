using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSpin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        init();
        StartCoroutine(keepRotating());
    }

    private void init()
	{
        transform.localEulerAngles = new Vector3(180, 90, 0);
	}

    private IEnumerator keepRotating()
    {
        while(true)
        {
            Vector3 angleOfRot = new Vector3(6, 0, 0);

            transform.Rotate(angleOfRot);
            yield return new WaitForSeconds(1);

        }
    }
}
