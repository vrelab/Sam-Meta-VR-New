using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Lean.Gui;
using Unity.VisualScripting;
using Lean.Transition;
using Photon.Realtime;
using System;
using Lean.Transition.Method;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using System.Threading;
using TMPro;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] LeanJoystick joystick;
    [SerializeField] LeanJoystick camerajoystick;
    [SerializeField] Transform avatarparent;
    [SerializeField] LeanToggle joysticktoggle;
    [SerializeField] Vector3 Position;
    public static Action<GameObject> Playerspawned;
    public List<GameObject> playerlist = new List<GameObject>();
    public GameObject player;
    PhotonView view;
    [SerializeField] PhotonView Spawnerphotonview;
    [SerializeField] LeanCanvasGroupAlpha Fade;
    [SerializeField] GameObject M_player;
    [SerializeField] Recorder recorder;
    GameObject MainplayerCam;
    Playersetup myplayersetup;
  
    public override void OnJoinedRoom()
    {

        player = PhotonNetwork.Instantiate("Networkplayer", Position, Quaternion.identity);
        player.transform.SetParent(avatarparent);
        player.GetComponentInChildren<Playersetup>().Initializeplayer();
        var character = player.GetComponentInChildren<Character>();
        character.uIElements.leanJoystick = joystick;
        character.uIElements.leanJoystickCamera = camerajoystick;

        //UserManager.Instance.character = character;

        //character.uIElements.DontUseJoystick();
        //character.uIElements.UseFPSCamera();
        //Enablefade(2.4f);
        //enablerecorder(4);
        view = player.GetComponent<PhotonView>();
        //Debug.Log("view ob :"+view.gameObject.name);
        joysticktoggle.TurnOn();

        
        if (view != null && view.IsMine)
        {
            //Debug.Log("viewismine :"+player.gameObject.name);
            Playerspawned?.Invoke(player);
            M_player = player;
            changename(DataSaver.Instance.userName);
            myplayersetup =M_player.GetComponentInChildren<Playersetup>();
        }
        else
        {
            transform.EventTransition(() =>
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.16f, player.transform.position.z);
                Debug.Log("adjustedpositon");
            }, 0.5f);
        }
    }



    public void ShowPassword(TMP_InputField text)
    {
        text.contentType = TMP_InputField.ContentType.Standard;
        var rectTransform =text.textComponent.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 17);
        text.ForceLabelUpdate();
    }


    public void HidePassword(TMP_InputField text)
    {
        text.contentType = TMP_InputField.ContentType.Password;
        var rectTransform = text.textComponent.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 2);
        text.ForceLabelUpdate();
    }


    public void ShowPasswordsettings(TMP_InputField text)
    {
        text.contentType = TMP_InputField.ContentType.Standard;
        var rectTransform = text.textComponent.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 11);
        text.ForceLabelUpdate();
    }
    void enablerecorder(float time)
    {
        transform.EventTransition(() =>
        {
            recorder.enabled = true;
        }, time);
    }
    void Enablefade(float time)
    {
        transform.EventTransition(() =>
        {
            Fade.BeginAllTransitions();
        }, time);
    }


    public void ToggleMymic(bool val)
    {
        if(myplayersetup != null)
        {
            myplayersetup.myspeaker.mute = val;
            recorder.TransmitEnabled = !val;
        }
    }

    public void togglemaincam(bool value)
    {
        if(MainplayerCam ==null)
        {
            MainplayerCam = Camera.main.gameObject;
        }

        MainplayerCam.SetActive(value);
    }

    public void toggledisplay(bool val)
    {
        if (M_player != null)
        {
            M_player.GetComponentInChildren<CustomAvatar>().Changedisplaylean(val);

        }
    }
    public void changename(string name)
    {
        if (M_player != null)
        {
            M_player.GetComponentInChildren<CustomAvatar>().Changedisplaynamerpc(name);

        }
    }

    public void controlPlayer(bool val)
    {
        if (M_player != null)
        {
            M_player.GetComponentInChildren<Playersetup>().Toggleplayercontrol(val);

        }
        else
        {

            Debug.Log("<color=red>M_player is null</color>");
        }
    }


    public void Fpscamera(bool value)
    {
        if (M_player != null)
        {
            M_player.GetComponentInChildren<JoystickMovement>().SetCamera(value);
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
                Debug.Log("destroying user" + photonView.ViewID);

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
            Debug.Log("player not found");
        }
    }
}

