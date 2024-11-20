using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;


public class JoystickMovement : MonoBehaviour
{
    [SerializeField]public LeanJoystick leanJoystick;
    [SerializeField] LeanJoystick leanJoystickCamera;
    [SerializeField] Character character;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject FPSVirtualCam;
    [SerializeField] GameObject _mainCamera;
   // PhotonView photonView;
    Vector3 moveDirection = Vector3.zero;

    [SerializeField] float _horizontalCamMove;
    [SerializeField] float _verticalCamMove;
    private void Start()
    {
        FPSVirtualCam.SetActive(false);
        //photonView = transform.parent.GetComponent<PhotonView>();
        //_horizontalCamMove = .25f;
        //_verticalCamMove = .25f;
        character = GetComponent<Character>();
        characterController = GetComponent<CharacterController>();
        leanJoystick = UIElements.Instance.leanJoystick;
        leanJoystickCamera = UIElements.Instance.leanJoystickCamera;
        CameraSensitivity.changingVerticalSensitivity += SetVerticalSensitivity;
        CameraSensitivity.changingHorizontalSensitivity += SetHorizontalSensitivity;
        UIElements.Instance.enableTheVirtualFPSCamera += SetCamera;
    }

    private void Update()
    {
        SetMovementVector();
        SetCameraVector();
    }
    void SetMovementVector()
    {

        if (leanJoystick.ScaledValue.x > .1)
        {
            PlayerInput._horizontalAxisJS = .35f * leanJoystick.ScaledValue.x;
        }

        else if(leanJoystick.ScaledValue.x < -.1)
        {
            PlayerInput._horizontalAxisJS = 0.35f * leanJoystick.ScaledValue.x;
        }
        else
        {
            PlayerInput._horizontalAxisJS = 0;
        }


        if (leanJoystick.ScaledValue.y > .1)
        {
            PlayerInput._verticalAxisJS = .35f * leanJoystick.ScaledValue.y;
        }

        else if (leanJoystick.ScaledValue.y < -.1)
        {
            PlayerInput._verticalAxisJS = .35f* leanJoystick.ScaledValue.y;
        }
        else
        {
            PlayerInput._verticalAxisJS = 0;
        }
    }

    void SetCameraVector()
    {

        if (leanJoystickCamera.ScaledValue.x > .1)
        {
            PlayerInput._horizontalAxisCam = _horizontalCamMove* leanJoystickCamera.ScaledValue.x;
        }

        else if (leanJoystickCamera.ScaledValue.x < -.1)
        {
            PlayerInput._horizontalAxisCam = (_horizontalCamMove)* leanJoystickCamera.ScaledValue.x;
        }
        else
        {
            PlayerInput._horizontalAxisCam = 0;
        }


        if (leanJoystickCamera.ScaledValue.y > .1)
        {
            PlayerInput._verticalAxisCam = _verticalCamMove* leanJoystickCamera.ScaledValue.y;
        }

        else if (leanJoystickCamera.ScaledValue.y < -.1)
        {
            PlayerInput._verticalAxisCam = (_verticalCamMove)* leanJoystickCamera.ScaledValue.y;
        }
        else
        {
            PlayerInput._verticalAxisCam = 0;
        }
    }

    //Control Camera Sensitivity
    void SetHorizontalSensitivity(float sensi)
    {
        _horizontalCamMove = sensi;
    }

    void SetVerticalSensitivity(float sensi)
    {
        _verticalCamMove = sensi;
    }


    //First Person Camera Section

    public void SetCamera(bool canEnable)
    {
     
            FPSVirtualCam.SetActive(canEnable);
            _mainCamera.SetActive(!canEnable);
        
    }
}
