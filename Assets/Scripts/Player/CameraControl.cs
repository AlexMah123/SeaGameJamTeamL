using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float xAxis_Sens, yAxis_Sens;
    public Transform playerOrientation;

    private float xAxis_Rot, yAxis_Rot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xAxis_Sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * yAxis_Sens;

        yAxis_Rot += mouseX;

        xAxis_Rot -= mouseY;
        xAxis_Rot = Mathf.Clamp(xAxis_Rot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xAxis_Rot, yAxis_Rot, 0);
        playerOrientation.rotation = Quaternion.Euler(0, yAxis_Rot, 0);
    }
}