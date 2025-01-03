﻿using UnityEngine;

public static class PlayerInput
{
    public static float lookAngle = 0f;
    public static float tiltAngle = 0f;

    public static float _horizontalAxisJS = 0;
    public static float _verticalAxisJS = 0;   
    public static float _horizontalAxisCam = 0;
    public static float _verticalAxisCam = 0;
    public static Quaternion controlRotation;

    public static Vector3 GetMovementInput(Camera relativeCamera)
    {
        Vector3 moveVector;
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (relativeCamera != null)
        {
            // Calculate the move vector relative to camera rotation
            Vector3 scalerVector = new Vector3(1f, 0f, 1f);
            Vector3 cameraForward = Vector3.Scale(relativeCamera.transform.forward, scalerVector).normalized;
            Vector3 cameraRight = Vector3.Scale(relativeCamera.transform.right, scalerVector).normalized;

            moveVector = (cameraForward * verticalAxis + cameraRight * horizontalAxis);
        }
        else
        {
            // Use world relative directions
            moveVector = (Vector3.forward * verticalAxis + Vector3.right * horizontalAxis);
        }

        if (moveVector.magnitude > 1f)
        {
            moveVector.Normalize();
        }

        return moveVector;
    }

    public static Vector3 GetMovementInputJoystick(Camera relativeCamera)
    {
        Vector3 moveVector;

        if (relativeCamera != null)
        {
            // Calculate the move vector relative to camera rotation
            Vector3 scalerVector = new Vector3(1f, 0f, 1f);
            Vector3 cameraForward = Vector3.Scale(relativeCamera.transform.forward, scalerVector).normalized;
            Vector3 cameraRight = Vector3.Scale(relativeCamera.transform.right, scalerVector).normalized;

            moveVector = (cameraForward * _verticalAxisJS + cameraRight * _horizontalAxisJS);
        }
        else
        {
            // Use world relative directions
            moveVector = (Vector3.forward * _verticalAxisJS + Vector3.right * _horizontalAxisJS);
        }

        if (moveVector.magnitude > 1f)
        {
            moveVector.Normalize();
        }

        return moveVector;
    }

    public static Quaternion GetMouseRotationInput(float mouseSensitivity = 3f, float minTiltAngle = -75f, float maxTiltAngle = 45f)
    {
        //if (!Input.GetMouseButton(1))
        //{
        //    return;
        //}

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += mouseY * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = MathfExtensions.ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
        return controlRotation;
    }

    public static Quaternion GetMouseRotationInputJoystick(float mouseSensitivity = .7f, float minTiltAngle = -75f, float maxTiltAngle = 45f)
    {
        //if (!Input.GetMouseButton(1))
        //{
        //    return;
        //}

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the look angle (Y Rotation)
        lookAngle += _horizontalAxisCam * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += _verticalAxisCam * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = MathfExtensions.ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
        return controlRotation;
    }

    public static bool GetSprintInput()
    {
       // return Input.GetButton("Sprint");
        return Input.GetKey(KeyCode.LeftShift);
    }

    public static bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public static bool GetToggleWalkInput()
    {
       // return Input.GetButtonDown("Toggle Walk");
        return Input.GetKeyDown(KeyCode.CapsLock);
    }
}
