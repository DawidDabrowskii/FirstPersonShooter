using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 100f;
    [SerializeField] float mouseSensitivityZ = 100f;
    public Transform playerBody;

    private float xRotation;

    private void Start()
    {
        HideMouse();
    }
    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation() // camera rotation using mouse
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityZ;

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation,-80,80);
        transform.localRotation = Quaternion.Euler(xRotation, 0,0);

        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);
    }

    private void HideMouse() // hide mouse function
    {
        Cursor.lockState = CursorLockMode.Locked;
    }    
} 
