//using Oculus.Platform;
using Photon.Pun;
//using ReadyPlayerMe.AvatarLoader;
using System.Collections;
using System.Collections.Generic;
//using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//using Photon.Voice.PUN;

public class CustomAvatar : MonoBehaviour
{
    [SerializeField] PhotonView photonView;

    [Header("AVATAR")]
    [SerializeField] SkinnedMeshRenderer lefteye;
    [SerializeField] SkinnedMeshRenderer righteye;
    [SerializeField] SkinnedMeshRenderer head;
    [SerializeField] SkinnedMeshRenderer hair;
    [SerializeField] SkinnedMeshRenderer teeth;
    [SerializeField] SkinnedMeshRenderer OutfitTop;
    [SerializeField] SkinnedMeshRenderer glasses;
    [SerializeField] SkinnedMeshRenderer body;
    [SerializeField] SkinnedMeshRenderer bottom;
    [SerializeField] SkinnedMeshRenderer foot;
    [SerializeField] SkinnedMeshRenderer beard;
    [SerializeField] SkinnedMeshRenderer facewear;
    [SerializeField] SkinnedMeshRenderer headwear;
    // [SerializeField] Transform Righteyebone, Lefteyebone;
    [SerializeField] Animator Animat;



    [Header("MaleTarget")]
    [SerializeField] GameObject malelefteye;
    [SerializeField] GameObject malerighteye;
    [SerializeField] GameObject malehead;
    [SerializeField] GameObject malehair;
    [SerializeField] GameObject maleteeth;
    [SerializeField] GameObject maleOutfitTop;
    [SerializeField] GameObject maleglasses;
    [SerializeField] GameObject malebody;
    [SerializeField] GameObject malebottom;
    [SerializeField] GameObject malefoot;
    [SerializeField] GameObject malebeard;
    [SerializeField] GameObject malefacewear;
    // [SerializeField] GameObject maleheadwear;


    [Header("FemaleTarget")]
    [SerializeField] GameObject femalelefteye;
    [SerializeField] GameObject femalerighteye;
    [SerializeField] GameObject femalehead;
    [SerializeField] GameObject femalehair;
    [SerializeField] GameObject femaleteeth;
    [SerializeField] GameObject femaleOutfitTop;
    [SerializeField] GameObject femaleglasses;
    [SerializeField] GameObject femalebody;
    [SerializeField] GameObject femalebottom;
    [SerializeField] GameObject femalefoot;
    [SerializeField] GameObject femalefacewear;
    //[SerializeField] GameObject femaleheadwear;

    [SerializeField] Transform Maleparent;
    [SerializeField] Transform femaleparent;
    [SerializeField] Transform Mainskeleton;
    [SerializeField] AnimatorOverrideController animoveride;
    [SerializeField] RuntimeAnimatorController maleanim;
    GameObject currentAvatar;
    GameObject otherAvatar = null;
    //AvatarObjectLoader avatarLoader;
    Vector3 defaultlefteye;
    Vector3 defaultrighteye;
    Displayname display;
    [SerializeField] Image speakericon;
    //[SerializeField] PhotonVoiceView voiceView;

    // Start is called before the first frame update

    Camera mainCam;

    void Start()
    {
        //playerlefteye = FindinTransform(Mainskeleton, "Renderer_EyeLeft");
        //playerrighteye = FindinTransform(Mainskeleton, "Renderer_EyeRight");
        //playerhair = FindinTransform(Mainskeleton, "Renderer_Hair");
        //playerhead = FindinTransform(Mainskeleton, "Renderer_Head");
        //playerteeth = FindinTransform(Mainskeleton, "Renderer_Teeth");
        //playerOutfitTop = FindinTransform(Mainskeleton, "Renderer_Outfit_Top");
        //playerbody = FindinTransform(Mainskeleton, "Renderer_Body");
        //playerbottom = FindinTransform(Mainskeleton, "Renderer_Outfit_Bottom");
        //playerfoot = FindinTransform(Mainskeleton, "Renderer_Outfit_Footwear");
        //playerglasses = FindinTransform(Mainskeleton, "Renderer_Glasses");
        // defaultlefteye = Lefteyebone.transform.localPosition;
        // defaultrighteye = Righteyebone.transform.localPosition;

        mainCam = UserManager.Instance.mainCamera;

        if (photonView.IsMine)
        {
            //VuplexHandler.OnAvatarLoaded += SpawnPlayer;
        }

        //StartCoroutine(sendemptyrpc());

    }



    private void Update()
    {
        //speakericon.enabled = voiceView.IsSpeaking;

    }
    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            //VuplexHandler.OnAvatarLoaded -= SpawnPlayer;
        }

    }


    public void togglespeakericon(bool toggle)
    {
        //photonView.RPC("togglespeaker",RpcTarget.AllBuffered,toggle);
    }

    IEnumerator sendemptyrpc()
    {
        while (true)
        {
            photonView.RPC("emptyrpc", RpcTarget.All);
            yield return new WaitForSeconds(30);
        }
    }
    public void Changedisplaynamerpc(string name)
    {
        photonView.RPC("changename", RpcTarget.AllBuffered, name);
    }

    public void Changedisplaylean(bool val)
    {
        photonView.RPC("toggledisplaylean", RpcTarget.AllBuffered, val);
    }
    void SpawnPlayer(GameObject avatar, string url)
    {


        if (currentAvatar != null)
        {
            Destroy(currentAvatar);
        }

        //if (photonView.IsMine)
        //{


        // }
        SetCustomAvatar(avatar, transform.parent.gameObject);
        photonView.RPC("DownloadAvatarForOtherClients", RpcTarget.OthersBuffered, url);
    }



    void SetCustomAvatar(GameObject avatar, GameObject playerparent)
    {
        //if(!photonView.IsMine)
        //{
        //    return;
        //}
        currentAvatar = avatar;
        avatar.transform.SetParent(playerparent.transform);

        //Debug.Log("avatar");
        //Debug.Log(avatar.name);
        //var child = RecursiveFindChild(avatar.transform, "Renderer_Head");
        Animat.avatar = avatar.GetComponent<Animator>().avatar;




        head = findskinrendererinteransform(avatar, "Renderer_Head");
        hair = findskinrendererinteransform(avatar, "Renderer_Hair");
        lefteye = findskinrendererinteransform(avatar, "Renderer_EyeLeft");
        righteye = findskinrendererinteransform(avatar, "Renderer_EyeRight");
        teeth = findskinrendererinteransform(avatar, "Renderer_Teeth");
        OutfitTop = findskinrendererinteransform(avatar, "Renderer_Outfit_Top");
        body = findskinrendererinteransform(avatar, "Renderer_Body");
        bottom = findskinrendererinteransform(avatar, "Renderer_Outfit_Bottom");
        foot = findskinrendererinteransform(avatar, "Renderer_Outfit_Footwear");
        glasses = findskinrendererinteransform(avatar, "Renderer_Glasses");
        beard = findskinrendererinteransform(avatar, "Renderer_Beard");
        facewear = findskinrendererinteransform(avatar, "Renderer_Facewear");
        headwear = findskinrendererinteransform(avatar, "Renderer_Headwear");



        if (avatar.GetComponent<Animator>().avatar.name.Contains("Feminine"))
        {

            femaleparent.gameObject.SetActive(true);
            Maleparent.gameObject.SetActive(false);
            Animat.runtimeAnimatorController = animoveride;
            femaleparent.SetAsFirstSibling();
            Animat.Rebind();

            //Animat.runtimeAnimatorController = maleanim;
            //Lefteyebone.transform.localPosition = new Vector3(-0.0341f, 0.0873f, 0.0659f);
            //Righteyebone.transform.localPosition = new Vector3(0.0341f, 0.0873f, 0.0659f);
            AssignActor(femalehead, head);
            AssignActor(femalehair, hair);
            AssignActor(femalebody, body);
            AssignActor(femaleteeth, teeth);
            AssignActor(femalelefteye, lefteye);
            AssignActor(femalerighteye, righteye);
            AssignActor(femaleOutfitTop, OutfitTop);
            AssignActor(femalebottom, bottom);
            AssignActor(femalefoot, foot);
            AssignActor(femaleglasses, glasses);
            AssignActor(femalefacewear, facewear);
            AssignActor(femalehair, headwear);


            femalelefteye.SetActive(false);
            femalerighteye.SetActive(false);
            femalehair.SetActive(false);
            femalehead.SetActive(false);
            femaleteeth.SetActive(false);
            femaleOutfitTop.SetActive(false);
            femalebody.SetActive(false);
            femalebottom.SetActive(false);
            femalefoot.SetActive(false);
            femaleglasses.SetActive(false);
        }
        else
        {
            Animat.runtimeAnimatorController = maleanim;
            Maleparent.SetAsFirstSibling();
            Animat.Rebind();
            femaleparent.gameObject.SetActive(false);
            Maleparent.gameObject.SetActive(true);
            //Lefteyebone.transform.localPosition = defaultlefteye;
            //Righteyebone.transform.localPosition = defaultrighteye;
            AssignActor(malehead, head);
            AssignActor(malehair, hair);
            AssignActor(malebody, body);
            AssignActor(maleteeth, teeth);
            AssignActor(malelefteye, lefteye);
            AssignActor(malerighteye, righteye);
            AssignActor(maleOutfitTop, OutfitTop);
            AssignActor(malebottom, bottom);
            AssignActor(malefoot, foot);
            AssignActor(maleglasses, glasses);
            AssignActor(malefacewear, beard);
            AssignActor(malefacewear, facewear);
            AssignActor(malehair, headwear);


            malelefteye.SetActive(false);
            malerighteye.SetActive(false);
            malehair.SetActive(false);
            malehead.SetActive(false);
            maleteeth.SetActive(false);
            maleOutfitTop.SetActive(false);
            malebody.SetActive(false);
            malebottom.SetActive(false);
            malefoot.SetActive(false);
            maleglasses.SetActive(false);
        }

        Debug.Log("Assigned");
        //GameManager.Instance.TryRemovingLoadingScreen();

    }

    public void AssignToActor(GameObject target, SkinnedMeshRenderer thisRenderer)
    {
        SkinnedMeshRenderer targetRenderer = target.GetComponent<SkinnedMeshRenderer>();
        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in targetRenderer.bones)
        {
            boneMap[bone.name] = bone;
            // Debug.Log(bone.name + "bone" + boneMap.Count + boneMap.ContainsKey(bone.name));
        }

        var boneArray = thisRenderer.bones;

        for (int i = 0; i < boneArray.Length; i++)
        {

            string boneName = boneArray[i].name;

            if (boneMap.TryGetValue(boneName, out boneArray[i]) == false)
            {
                Debug.LogError("failed to get bone: " + i + " - " + boneName);
            }
        }
        thisRenderer.bones = boneArray; //take effect
        thisRenderer.gameObject.SetActive(true);
        if (target.transform.childCount > 0)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                AssignToActor(target.transform.GetChild(i).gameObject, thisRenderer.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>());
            }
        }
    }

    GameObject FindinTransform(GameObject parent, string name)
    {
        var gameobj = parent.transform.Find(name);
        if (gameobj != null)
            return gameobj.gameObject;
        else
            return null;
    }

    void AssignActor(GameObject target, SkinnedMeshRenderer renderer)
    {
        if (target == null || renderer == null)
        {
            return;
        }
        AssignToActor(target, renderer);
    }

    SkinnedMeshRenderer findskinrendererinteransform(GameObject parent, string name)
    {
        SkinnedMeshRenderer meshrenderer;
        var Skinnedmesh = parent.transform.Find(name);
        if (Skinnedmesh != null)
        {
            meshrenderer = Skinnedmesh.GetComponent<SkinnedMeshRenderer>();
            return meshrenderer;
        }
        else
        {
            return null;
        }

    }

    Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("child : " + child.name);
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
    private void loadavatar(string avatarUrl)
    {

        //avatarLoader = new AvatarObjectLoader();
        //avatarLoader.OnCompleted += onavatarloadfinished;

        //avatarLoader.LoadAvatar(avatarUrl);
        Debug.Log("Avatar Url : " + avatarUrl);
    }

    private void onavatarloadfinished(object sender)
    {
        if (otherAvatar != null)
        {
            Destroy(otherAvatar);
        }
        //otherAvatar = args.Avatar;
        otherAvatar.name = "sample";
        //otherAvatar.name = PhotonNetwork.NickName;
        otherAvatar.transform.Rotate(Vector3.up, 180);
        otherAvatar.transform.position = new Vector3(0.75f, 0, 0.75f);
        otherAvatar.gameObject.SetActive(true);
        SetCustomAvatar(otherAvatar, transform.parent.gameObject);
        Debug.Log("Avatar Load Completed");
    }

    [PunRPC]
    void DownloadAvatarForOtherClients(string url)
    {
        Debug.Log("url: " + url);
        Debug.Log("rpc called");
        loadavatar(url);
        Debug.Log("rpc avatar loaded");
        // SetCustomAvatar(avatar, gameObject);

    }


    [PunRPC]

    void changename(string name)
    {
        display = GetComponent<Displayname>();
        display.Setname(name);
        //Debug.Log("name set as" + name);
    }

    [PunRPC]
    void toggledisplaylean(bool val)
    {
        display = GetComponent<Displayname>();

        if (Camera.main != null)
            if (Camera.main.transform != null)
                display.SetTarget(Camera.main.transform);
            else if (mainCam != null)
                if (mainCam.transform != null)
                    display.SetTarget(mainCam.transform);
        if (val && !photonView.IsMine)
        {
            display.Displaylean.TurnOn();
        }
        else
        {
            display.Displaylean.TurnOff();
        }
    }


    [PunRPC]
    void emptyrpc()
    {
        Debug.Log("lol");
    }

    [PunRPC]
    void togglespeaker(bool val)
    {
        speakericon.enabled = val;
    }
}