using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playersetup : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] GameObject Camerarig;
    CharacterController characterController;
    CharacterAnimator characterAnimator;
    Character character;
    MovementInput input;
    JoystickMovement joystick;
    [SerializeField] GameObject uicam;
    public AudioSource myspeaker;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    public void Initializeplayer()
    {
        if (view.IsMine)
        {
            characterController = GetComponent<CharacterController>();
            character = GetComponent<Character>();
            characterAnimator = GetComponent<CharacterAnimator>();
            input = GetComponent<MovementInput>();
            joystick = GetComponent<JoystickMovement>();

            joystick.enabled = true;
            input.enabled = true;
            characterController.enabled = true;
            characterAnimator.enabled = true;
            character.enabled = true;
            Camerarig.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

     public void Toggleplayercontrol(bool value)
    {
        joystick.enabled = value;
        //input.enabled = value;
        characterController.enabled = value;
        characterAnimator.enabled = value;
        character.enabled = value;
        //uicam.SetActive(!value);
       // Camerarig.SetActive(value);
    }
}
