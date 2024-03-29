using UnityEngine;
using System.Collections.Generic;

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;
    public bool canMove = true;

    Vector2 clampDegrees = new Vector2(360, 180);
    Vector2 sensitivity = new Vector2(2, 2);
    Vector2 mouseAbsolute, smoothMouse, myLocalEulerAngles;


    void Awake()
    {
        instance = this;
        LockHideCursor();
        myLocalEulerAngles = transform.localEulerAngles;
    }

    void LockHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canMove)
            Move();
    }

    void Move()
    {
        // Allow the script to clamp based on a desired target value.
        Quaternion targetOrientation = Quaternion.Euler(myLocalEulerAngles);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x, sensitivity.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f);

        // Find the absolute mouse movement value from point zero.
        mouseAbsolute += smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampDegrees.x < 360)
            mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampDegrees.x * 0.5f, clampDegrees.x * 0.5f);

        Quaternion xRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right);
        transform.localRotation = xRotation;

        // Then clamp and apply the global y value.
        if (clampDegrees.y < 360)
            mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampDegrees.y * 0.5f, clampDegrees.y * 0.5f);

        transform.localRotation *= targetOrientation;

        // If there's a character body that acts as a parent to the camera
        Quaternion yRotation = Quaternion.AngleAxis(mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
        transform.localRotation *= yRotation;

        Vector3 camRot = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        Vector3 playerRot = new Vector3(0, transform.localEulerAngles.y, 0);

        Camera.main.transform.localEulerAngles = camRot;
        transform.localEulerAngles = playerRot;
    }
}
