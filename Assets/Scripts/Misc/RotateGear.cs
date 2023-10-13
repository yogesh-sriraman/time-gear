using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour
{
    float rotateSpeed = 2000f;
    bool dragging = false;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnMouseDrag()
    {
        dragging = true;
    }
    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }
    private void FixedUpdate()
    {
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 currentMousePosition = Input.mousePosition;

        if (dragging)
        {
            float x = Input.GetAxis("Mouse X") * rotateSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotateSpeed * Time.fixedDeltaTime;
            
            /*
            if ( currentMousePosition.y > transform.position.y && currentMousePosition.x > transform.position.x )
            {
                rb.AddTorque(Vector3.forward * -x);
                rb.AddTorque(Vector3.forward * y);
            }
            else if ( currentMousePosition.y < transform.position.y && currentMousePosition.x > transform.position.x )
            {
                rb.AddTorque(Vector3.forward * x);
                rb.AddTorque(Vector3.forward * y);
            }
            else if ( currentMousePosition.y < transform.position.y && currentMousePosition.x < transform.position.x )
            {
                rb.AddTorque(Vector3.forward * x);
                rb.AddTorque(Vector3.forward * -y);
            }
            else
            {
                rb.AddTorque(Vector3.forward * -x);
                rb.AddTorque(Vector3.forward * -y);
            }
            */

            
            if(currentMousePosition.y > transform.position.y)
            {
                
                rb.AddTorque(Vector3.forward * -x);
            }
            else
            {
                rb.AddTorque(Vector3.forward * x);
            }
            if(currentMousePosition.x > transform.position.x)
            {
                rb.AddTorque(Vector3.forward * y);
            }
            else
            {
                rb.AddTorque(Vector3.forward * -y);
            }
            
            
        }
    }
}
