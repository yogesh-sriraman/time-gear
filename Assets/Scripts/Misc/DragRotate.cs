using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotate : MonoBehaviour
{
    float initialAngle = 0.0f;
    Quaternion originalRotation;

    [SerializeField]
    GameObject Character;

    float characterInitialAngle = 0.0f;
    Quaternion characterOriginalRotation;
    bool mouseDown = false;
    long count = 0;
    float gearRot, updatedGR;

    void Start()
    {
        
        originalRotation = transform.rotation;
        characterOriginalRotation = Character.transform.rotation;
    }
    
    void OnMouseDown()
    {
          //Debug.Log("Initial angle: " + initialAngle + " transform.rotation.z: " + transform.rotation.z);
            var screenPos = Camera.main.WorldToScreenPoint(transform.position);
            var vec = Input.mousePosition - screenPos;
            initialAngle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            initialAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
            initialAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
          //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, gearZ.z);
            characterInitialAngle = initialAngle;
            characterInitialAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
            characterInitialAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
          //Debug.Log("Initial angle: " + initialAngle + " transform.rotation.z: " + transform.rotation.z);
    }

    void OnMouseDrag()
    {
          //Debug.Log("Initial angle: " + initialAngle + " transform.rotation.z: " + transform.rotation.z);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);    
            Vector3 vec = Input.mousePosition - screenPosition;
            float rotationAngle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            transform.rotation = originalRotation * Quaternion.AngleAxis(initialAngle - rotationAngle, Vector3.forward);
            Character.transform.rotation = characterOriginalRotation * Quaternion.AngleAxis(characterInitialAngle - rotationAngle, Vector3.up);
          //Debug.Log("Initial angle: " + initialAngle + " transform.rotation.z: " + transform.rotation.z);
    }

    private void Update()
    {
        gearRot = transform.localRotation.eulerAngles.z;
        //gearRot = transform.rotation.eulerAngles.z;
        if(count%60<1)
            Debug.Log(gearRot);
        gearRot = Mathf.Clamp(gearRot, -180, 180);
        //transform.localRotation = Quaternion.Euler(gearRot.x, gearRot.y, gearRot.z);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, gearRot);
        Character.transform.localEulerAngles = new Vector3(Character.transform.localEulerAngles.x, gearRot, Character.transform.localEulerAngles.z);
        count++;
    }

}
