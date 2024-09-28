using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{

    public Transform camera_Position;

    // Update is called once per frame
    private void Update()
    {
        transform.position =  camera_Position.position;
    }
}
