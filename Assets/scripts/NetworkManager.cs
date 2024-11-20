using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Lean.Transition;
using UnityEngine.XR;
using System;
using TMPro;
using Lean.Gui;
using UnityEngine.UI;
using static UserManager;
//using Vuplex.WebView;
using Lean.Transition.Method;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] ServerSettings serverSettings;
    TypedLobby typedLobby;
    public const string PLATFORM_PROP_KEY = "Platform";
    [SerializeField] GameObject reconnectCanvas;
    [SerializeField] TMP_Text offlinetext;
    string roomname;
    [SerializeField] List<LeanToggle> toggles_to_turnOff;
    [SerializeField] List<LeanToggle> toggles_to_turnOn;
    bool disconnect = false;
    [SerializeField] VRnetworkplayerspawner nps;
    //[SerializeField] List<BaseWebViewPrefab> webviews;
    //[SerializeField] GameObject Startbg;
    // Start is called before the first frame update

    public UnityEvent onPhotonDisconnect;
    public UnityEvent onPhotonReconnect;

    private void Awake()
    {
    }
    void Start()
    {

        ConnectToserver();

        //if(Application.platform ==RuntimePlatform.Android)
        //{
        //    platformvalue = 0;
        //}
        //else if(Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    platformvalue = 1;
        //}
        //else if(UnityEngine.XR.XRSettings.enabled)
        //{
        //    platformvalue = 2;
        //}
        //else
        //{
        //    platformvalue = 3;
        //}
        //Debug.LogFormat("<color=cyan>Platform val : {0}</color>", platformvalue);
    }

    public void ConnectToserver()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Joinlobby();
        }
        //PhotonNetwork.Disconnect();
        //PhotonNetwork.ConnectUsingSettings();
        // Debug.Log("connected");
        //}
    }

    public override void OnConnectedToMaster()
    {
        Joinlobby();

    }

    public override void OnJoinedLobby()
    {
        // Debug.Log("joined lobby");
        if (string.IsNullOrEmpty(roomname))
        {
            // Debug.Log("room name is empty" + roomname);
            Joinroom();
        }
        else
        {
            //if (PhotonNetwork.IsConnected)
            //{
            //    PhotonNetwork.RejoinRoom(roomname);
            //}
            //else
            // {
            //   Debug.Log("else part");
            // if (!PhotonNetwork.ReconnectAndRejoin())
            //{
            PhotonNetwork.RejoinRoom(roomname);
            //foreach (var W in webviews)
            //{
            //    W.WebView.Reload();
            //}
            //}
            // }

        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("unable to rehoin,joining new room error:" + message);
        Debug.Log("Rejoining.." + roomname);
        Joinroom();
    }

    void Joinroom()
    {
        if (reconnectCanvas != null)
        {
            reconnectCanvas.SetActive(false);
        }
        RoomOptions roomOp = new RoomOptions();
        roomOp.MaxPlayers = 8;
        roomOp.IsVisible = true;
        roomOp.IsOpen = true;
        roomOp.PublishUserId = true;
        //roomOp.PlayerTtl = -1;
        roomOp.CleanupCacheOnLeave = true;
        //roomOp.CustomRoomPropertiesForLobby =new string[] {PLATFORM_PROP_KEY};
        //roomOp.CustomRoomProperties =new ExitGames.Client.Photon.Hashtable { { PLATFORM_PROP_KEY,platformvalue } };
        //ExitGames.Client.Photon.Hashtable Expectedroomproperties = new ExitGames.Client.Photon.Hashtable { { PLATFORM_PROP_KEY,platformvalue } };
        //EnterRoomParams enterRoomParams = new EnterRoomParams();
        //enterRoomParams.RoomOptions = roomOp;
        //PhotonNetwork.NickName = "player " + Random.Range(0,50).ToString();
        //PhotonNetwork.NickName = "player_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        PhotonNetwork.JoinRandomOrCreateRoom(null, 8, MatchmakingMode.FillRoom, typedLobby, null, null, roomOp);
        //PhotonNetwork.CreateRoom("room1", roomOp);
        //PhotonNetwork.JoinRoom("room1");
    }
    void Joinlobby()
    {
        typedLobby = new TypedLobby(serverSettings.AppSettings.FixedRegion, LobbyType.Default);
        PhotonNetwork.JoinLobby(typedLobby);
    }



    public override void OnJoinedRoom()
    {
        if (reconnectCanvas != null)
        {
            reconnectCanvas.SetActive(false);
        }
        roomname = PhotonNetwork.CurrentRoom.Name;
        
        if (disconnect)
        {


            transform.EventTransition(() =>
            {
                if (UserManager.Instance.isUserSignIn)
                {
                    nps.toggledisplay(true);
                    nps.changename(UserManager.Instance.myUserName);
                }
                else
                {
                    nps.toggledisplay(true);
                    nps.changename("GUEST");
                }
                disconnect = false;

                //onPhotonReconnect?.Invoke();

            }, 1f);
        }
    }


    public void ReConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (reconnectCanvas != null)
            reconnectCanvas.SetActive(true);
        disconnect = true;
        //hideproductmenu.BeginAllTransitions();
        //UserManager.Instance.loginBtnInSettingPannel.interactable = true;
        //UserManager.Instance.loginBtnTextInSettingPannel.text = "Sign in";
        //UserManager.Instance.loginBtnTextInStartPannel.text = "Log in";
        //UserManager.Instance.loginBtnInStartPannel.interactable = true;
        //TTSandSoundManager.Instance.botIsTypingText.gameObject.SetActive(false);
        // Startbg.SetActive(false);
        if (cause == DisconnectCause.ServerTimeout)
        {
            offlinetext.text = "Player disconnected due to server timeout";
        }
        else if (cause == DisconnectCause.ClientTimeout)
        {
            offlinetext.text = "Player disconnected due to client timeout";
        }
        else if ((cause == DisconnectCause.Exception))
        {
            offlinetext.text = "Player disconnected due to Network Exception";
        }
        else
        {
            Debug.Log("Disconnection reason :" + cause.ToString());
        }

        foreach (var t in toggles_to_turnOff)
        {
            t.TurnOff();
        }

        foreach (var to in toggles_to_turnOn)
        {
            to.TurnOn();
        }


        //onPhotonDisconnect?.Invoke();

    }

}