using Lean.Gui;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystickhandler : MonoBehaviour
{
    public static Action<bool> Joystickstate;
    

    public void Changetogglestate(Toggle toggle)
    {
        Joystickstate?.Invoke(toggle.isOn);
    }
   
    
}

