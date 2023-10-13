using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGearAlternate : MonoBehaviour
{
    [SerializeField]
    GameObject Character;
    [SerializeField]
    GameObject Platform;

    float rotateSpeed = 20f;
    float yAxisDifference;
    float xAxisDifference;

    private void OnMouseDrag()
    {
        float CameraY = Camera.main.transform.position.y;
        float GearY = transform.position.y;
        yAxisDifference = CameraY - GearY;

        float CameraX = Camera.main.transform.position.x;
        float GearX = transform.position.x;
        xAxisDifference = CameraX - GearX;

        Vector3 offset = Camera.main.transform.position - transform.position;

        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 currentMousePosition = Input.mousePosition;


        float RotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
        float RotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;

        /*
        transform.RotateAround(Vector3.forward, -RotX);
        transform.RotateAround(Vector3.forward, RotY);
        */

        #region Orthographic rotation

        
        if (currentMousePosition.y > transform.position.y)
        {
            transform.RotateAround(Vector3.forward, -RotX);
            Character.transform.RotateAround(Vector3.up, RotX);
            Platform.transform.RotateAround(Vector3.up, RotX);
        }
        else
        {
            transform.RotateAround(Vector3.forward, RotX);
            Character.transform.RotateAround(Vector3.up, -RotX);
            Platform.transform.RotateAround(Vector3.up, -RotX);
        }
        if (currentMousePosition.x > transform.position.x)
        {
            transform.RotateAround(Vector3.forward, RotY);
            Character.transform.RotateAround(Vector3.up, -RotY);
            Platform.transform.RotateAround(Vector3.up, -RotY);
        }
        else
        {
            transform.RotateAround(Vector3.forward, -RotY);
            Character.transform.RotateAround(Vector3.up, RotY);
            Platform.transform.RotateAround(Vector3.up, RotY);
        }
        

        #endregion

        #region Alternate Orthographic rotation

        /*
        if (currentMousePosition.y > (transform.position.y) && currentMousePosition.x > transform.position.x)
        {
            transform.RotateAround(Vector3.forward, -RotX);
            transform.RotateAround(Vector3.forward, RotY);
            Character.transform.RotateAround(Vector3.up, RotX);
            Platform.transform.RotateAround(Vector3.up, RotX);
            Character.transform.RotateAround(Vector3.up, -RotY);
            Platform.transform.RotateAround(Vector3.up, -RotY);
        }
        if (currentMousePosition.y < (transform.position.y) && currentMousePosition.x > transform.position.x)
        {
            transform.RotateAround(Vector3.forward, RotX);
            transform.RotateAround(Vector3.forward, RotY);
            Character.transform.RotateAround(Vector3.up, -RotX);
            Platform.transform.RotateAround(Vector3.up, -RotX);
            Character.transform.RotateAround(Vector3.up, -RotY);
            Platform.transform.RotateAround(Vector3.up, -RotY);
        }
        if (currentMousePosition.y < (transform.position.y) && currentMousePosition.x < transform.position.x)
        {
            transform.RotateAround(Vector3.forward, RotX);
            transform.RotateAround(Vector3.forward, -RotY);
            Character.transform.RotateAround(Vector3.up, -RotX);
            Platform.transform.RotateAround(Vector3.up, -RotX);
            Character.transform.RotateAround(Vector3.up, RotY);
            Platform.transform.RotateAround(Vector3.up, RotY);
        }
        if (currentMousePosition.y > (transform.position.y) && currentMousePosition.x < transform.position.x)
        {
            transform.RotateAround(Vector3.forward, -RotX);
            transform.RotateAround(Vector3.forward, -RotY);
            Character.transform.RotateAround(Vector3.up, RotX);
            Platform.transform.RotateAround(Vector3.up, RotX);
            Character.transform.RotateAround(Vector3.up, RotY);
            Platform.transform.RotateAround(Vector3.up, RotY);
        }
        */

        #endregion

        /*
        if (currentMousePosition.y > (transform.position.y + yAxisDifference) && currentMousePosition.x > transform.position.x)
        {
            transform.RotateAround(Vector3.forward, -RotX);
            transform.RotateAround(Vector3.forward, RotY);
            Character.transform.RotateAround(Vector3.up, RotX);
            Platform.transform.RotateAround(Vector3.up, RotX);
            Character.transform.RotateAround(Vector3.up, -RotY);
            Platform.transform.RotateAround(Vector3.up, -RotY);
        }
        if (currentMousePosition.y < (transform.position.y - yAxisDifference) && currentMousePosition.x > transform.position.x)
        {
            transform.RotateAround(Vector3.forward, RotX);
            transform.RotateAround(Vector3.forward, RotY);
            Character.transform.RotateAround(Vector3.up, -RotX);
            Platform.transform.RotateAround(Vector3.up, -RotX);
            Character.transform.RotateAround(Vector3.up, -RotY);
            Platform.transform.RotateAround(Vector3.up, -RotY);
        }
        if (currentMousePosition.y < (transform.position.y - yAxisDifference) && currentMousePosition.x < transform.position.x)
        {
            transform.RotateAround(Vector3.forward, RotX);
            transform.RotateAround(Vector3.forward, -RotY);
            Character.transform.RotateAround(Vector3.up, -RotX);
            Platform.transform.RotateAround(Vector3.up, -RotX);
            Character.transform.RotateAround(Vector3.up, RotY);
            Platform.transform.RotateAround(Vector3.up, RotY);
        }
        if (currentMousePosition.y > (transform.position.y + yAxisDifference) && currentMousePosition.x < transform.position.x)
        {
            transform.RotateAround(Vector3.forward, -RotX);
            transform.RotateAround(Vector3.forward, -RotY);
            Character.transform.RotateAround(Vector3.up, RotX);
            Platform.transform.RotateAround(Vector3.up, RotX);
            Character.transform.RotateAround(Vector3.up, RotY);
            Platform.transform.RotateAround(Vector3.up, RotY);
        }
        */

        /*
        if (currentMousePosition.y > (transform.position.y + yAxisDifference))
        {
            transform.RotateAround(Vector3.forward, -RotX);
            Character.transform.RotateAround(Vector3.up, RotX);
            Platform.transform.RotateAround(Vector3.up, RotX);
        }
        else
        {
            transform.RotateAround(Vector3.forward, RotX);
            Character.transform.RotateAround(Vector3.up, -RotX);
            Platform.transform.RotateAround(Vector3.up, -RotX);
        }
        if (currentMousePosition.x > transform.position.x)
        {
            transform.RotateAround(Vector3.forward, RotY);
            Character.transform.RotateAround(Vector3.up, -RotY);
            Platform.transform.RotateAround(Vector3.up, -RotY);
        }
        else
        {
            transform.RotateAround(Vector3.forward, -RotY);
            Character.transform.RotateAround(Vector3.up, RotY);
            Platform.transform.RotateAround(Vector3.up, RotY);
        }
        */
    }
}
