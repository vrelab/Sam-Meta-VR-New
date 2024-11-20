using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PhotonSpawnhandler : MonoBehaviour
{
    PhotonView pview;
    [SerializeField] GameObject Camerarig;

    private void Awake()
    {
        pview = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if(pview.IsMine)
        {
            Camerarig.SetActive(true);
        }
    }
}
