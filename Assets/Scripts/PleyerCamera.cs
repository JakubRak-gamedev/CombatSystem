using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleyerCamera : MonoBehaviour
{

    public float xSens = 400f;
    public float ySens = 400f;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float MouseX = Input.GetAxisRaw("Mouse X") * xSens * Time.deltaTime;
        float MouseY = Input.GetAxisRaw("Mouse Y") * ySens * Time.deltaTime;

        yRotation += MouseX;
        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
     }
}
