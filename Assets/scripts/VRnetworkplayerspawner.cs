using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Lean.Gui;
using Unity.VisualScripting;
using Lean.Transition;
using Photon.Realtime;
using System;

public class VRnetworkplayerspawner : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject Avatar;
    public static Action<GameObject> Playerspawned;
    public GameObject player;
    PhotonView view;
    [SerializeField] GameObject M_player;
    public Transform pos;
    public PhotonView Spawnerphotonview;
    [SerializeField] Canvas loadingScreen;
    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate(DataSaver.Instance.characterVersion, pos.position, Quaternion.identity);
        player.transform.SetParent(Avatar.transform);

        base.OnJoinedRoom();

        view = player.GetComponent<PhotonView>();
       
        if (view != null && view.IsMine)
        {
            Playerspawned?.Invoke(player);
            M_player = player;
            loadingScreen.enabled = false;
            changename(DataSaver.Instance.userName);
        }
        //GameManager.Instance.robotController.CallRobot();
    }

    public void toggledisplay(bool val)
    {
        if (M_player != null)
        {
            M_player.GetComponentInChildren<CustomAvatarVR>().Changedisplaylean(val);


        }
    }
    public void changename(string name)
    {
        if (M_player != null)
        {
            M_player.GetComponentInChildren<CustomAvatarVR>().Changedisplaynamerpc(name);


        }
    }

    public GameObject Findplayer(int playerActorNumber)
    {
        int actorNr = playerActorNumber;
        GameObject M_player = null;
        for (int viewId = actorNr * PhotonNetwork.MAX_VIEW_IDS + 1; viewId < (actorNr + 1) * PhotonNetwork.MAX_VIEW_IDS; viewId++)
        {
            PhotonView photonView = PhotonView.Find(viewId);
            if (photonView /*&& (photonView.OwnerActorNr == actorNr || photonView.ControllerActorNr == actorNr)*/)
            {
                M_player = photonView.transform.gameObject;

            }

        }
        return M_player;

    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Spawnerphotonview.RPC("Destroyplayer", RpcTarget.MasterClient, otherPlayer.ActorNumber);
    }


    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(M_player);
    }


    [PunRPC]
    void Destroyplayer(int actorNo)
    {
        var leavingplayer = Findplayer(actorNo);
        if (leavingplayer != null)
        {
            PhotonNetwork.Destroy(leavingplayer);
        }
        else
        {
        }
    }
}
