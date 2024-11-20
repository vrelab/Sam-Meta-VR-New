using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpirintButtonSupporter : SprintControlWrapper
{

    //PhotonView photonView;
    [SerializeField] Character character;

    private void Start()
    {
        //photonView = transform.parent.GetComponent<PhotonView>();

        character = GetComponent<Character>();
        //sprintingButton.pointerDown += DoSprint;
    }

    private void OnDestroy()
    {
        //sprintingButton.pointerDown -= DoSprint;
    }

    public void DoSprint(bool val)
    {
     
           character.IsSprinting = val;
    }
}
