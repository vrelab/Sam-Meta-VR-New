using Lean.Transition;
using Oculus.Platform;
using Photon.Pun;
//using ReadyPlayerMe.AvatarLoader;
using System.Collections;
using System.Collections.Generic;
//using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Voice.PUN;

public class CustomAvatarVR : MonoBehaviour
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
    [SerializeField] Transform Righteyebone, Lefteyebone;
    [SerializeField] Animator anim;


    [Header("Target")]
    [SerializeField] GameObject playerlefteye;
    [SerializeField] GameObject playerrighteye;
    [SerializeField] GameObject playerhead;
    [SerializeField] GameObject playerhair;
    [SerializeField] GameObject playerteeth;
    [SerializeField] GameObject playerOutfitTop;
    [SerializeField] GameObject playerglasses;
    [SerializeField] GameObject playerbody;
    [SerializeField] GameObject playerbottom;
    [SerializeField] GameObject playerfoot;
    [SerializeField] GameObject playerbeard;
    [SerializeField] GameObject playerfacewear;
    [SerializeField] GameObject playerheadwear;
    [SerializeField] GameObject Mainskeleton;
    Vector3 defaultlefteye;
    Vector3 defaultrighteye;
    Displayname display;

    [SerializeField] Image speakerIcon;
    [SerializeField] PhotonVoiceView photonVoiceView;

    GameObject currentAvatar;
    GameObject otherAvatar = null;
    bool secondTime;

    [SerializeField] Material planeclipping;
    // Start is called before the first frame update

    public void Changedisplaynamerpc(string name)
    {
        photonView.RPC("changename", RpcTarget.AllBuffered, name);
    }

    public void Changedisplaylean(bool val)
    {
        photonView.RPC("toggledisplaylean", RpcTarget.AllBuffered, val);
    }
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

        defaultlefteye = Lefteyebone.transform.localPosition;
        defaultrighteye = Righteyebone.transform.localPosition;
        if (photonView.IsMine)
        {
            playerlefteye.SetActive(false);
            playerrighteye.SetActive(false);
            playerhair.SetActive(false);
            playerhead.SetActive(false);
            playerteeth.SetActive(false);
            playerglasses.SetActive(false);
            //VuplexHandler.OnAvatarLoaded += SpawnPlayer;
        }

        //transform.EventTransition(() =>
        //{
        //    Debug.Log("loading duplicate");
        //    secondTime = true;
        //    loadavatar("https://models.readyplayer.me/651c2b25edf5b8a88212b89b.glb");
        //}, 15);

    }
    private void Update()
    {
        speakerIcon.enabled = photonVoiceView.IsSpeaking;
    }
    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
           // VuplexHandler.OnAvatarLoaded -= SpawnPlayer;
        }

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
        SetCustomAvatar(avatar, gameObject);
        photonView.RPC("DownloadAvatarForOtherClients", RpcTarget.OthersBuffered,
            url);
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
        anim.avatar = avatar.GetComponent<Animator>().avatar;
        anim.Rebind();
        if (avatar.GetComponent<Animator>().avatar.name.Contains("Masculine"))
        {
            Lefteyebone.transform.localPosition = new Vector3(-0.030352f, 0.087839f, 0.082406f);
            Righteyebone.transform.localPosition = new Vector3(0.029882f, 0.087790f, 0.082406f);
        }
        else
        {
            Lefteyebone.transform.localPosition = defaultlefteye;
            Righteyebone.transform.localPosition = defaultrighteye;
        }

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
        if (bottom != null)
            bottom.gameObject.SetActive(false);
        if (foot != null)
            foot.gameObject.SetActive(false);


        AssignActor(playerhead, head);
        AssignActor(playerhair, hair);
        AssignActor(playerbody, body);
        AssignActor(playerteeth, teeth);
        AssignActor(playerlefteye, lefteye);
        AssignActor(playerrighteye, righteye);
        AssignActor(playerOutfitTop, OutfitTop);
        AssignActor(playerfacewear, beard);
        AssignActor(playerfacewear, facewear);
        AssignActor(playerhair, headwear);
        // AssignActor(playerbottom, bottom);
        // AssignActor(playerfoot, foot);
        AssignActor(playerglasses, glasses);
        playerlefteye.SetActive(false);
        playerrighteye.SetActive(false);
        playerhair.SetActive(false);
        playerhead.SetActive(false);
        playerteeth.SetActive(false);
        playerOutfitTop.SetActive(false);
        playerbody.SetActive(false);
        playerbottom.SetActive(false);
        playerfoot.SetActive(false);
        playerglasses.SetActive(false);

        var mat = body.material;
        planeclipping.SetColor("col", mat.GetColor("baseColorFactor"));
        planeclipping.SetTexture("Tex", mat.GetTexture("baseColorTexture"));
        body.material = planeclipping;

        if (photonView.IsMine)
        {
            head.gameObject.SetActive(false);
            hair.gameObject.SetActive(false);
            lefteye.gameObject.SetActive(false);
            righteye.gameObject.SetActive(false);
            teeth.gameObject.SetActive(false);
            if (glasses != null)
            {
                glasses.gameObject.SetActive(false);
            }
            if (beard != null)
            {
                beard.gameObject.SetActive(false);
            }
            if (facewear != null)
            {
                facewear.gameObject.SetActive(false);
            }
            if (headwear != null)
            {
                headwear.gameObject.SetActive(false);
            }

        }

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
        int count = boneArray.Length;
        for (int i = 0; i < count; i++)
        {

            string boneName = boneArray[i].name;

            if (boneMap.TryGetValue(boneName, out boneArray[i]) == false)
            {
                //Debug.LogError("failed to get bone: " + i + " - " + boneName);
            }
        }
        thisRenderer.bones = boneArray; //take effect
        thisRenderer.quality = SkinQuality.Bone4;
        //targetRenderer.quality = SkinQuality.Bone4;
        thisRenderer.gameObject.SetActive(true);
        if (target.transform.childCount > 0)
        {
            int childcount = target.transform.childCount;
            for (int i = 0; i < childcount; i++)
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
            //Debug.Log("child : " + child.name);
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

        //AvatarObjectLoader avatarLoader = new AvatarObjectLoader();
        //avatarLoader.OnCompleted += onavatarloadfinished;

        //avatarLoader.LoadAvatar(avatarUrl);
        //Debug.Log("Avatar Url : " + avatarUrl);
    }

    private void onavatarloadfinished(object sender)
    {
        if (secondTime)
            return;

        if (otherAvatar != null)
        {
            Destroy(otherAvatar);
        }

        //otherAvatar = args.Avatar;
        otherAvatar.name = "sample";
        otherAvatar.transform.Rotate(Vector3.up, 180);
        otherAvatar.transform.position = new Vector3(0.75f, 0, 0.75f);
        otherAvatar.gameObject.SetActive(true);
        SetCustomAvatar(otherAvatar, gameObject);
        //Debug.Log("Avatar Load Completed");
    }

    [PunRPC]
    void DownloadAvatarForOtherClients(string url)
    {
        //Debug.Log("url: " + url);
        //Debug.Log("rpc called");

        loadavatar(url);
        //Debug.Log("rpc avatar loaded");
    }


    [PunRPC]

    void changename(string name)
    {
        display = GetComponent<Displayname>();
        display.Setname(name);
    }

    [PunRPC]
    void toggledisplaylean(bool val)
    {
        display = GetComponent<Displayname>();
        if (Camera.main.transform != null)
            display.SetTarget(Camera.main.transform);
        if (val && !photonView.IsMine)
        {
            display.Displaylean.TurnOn();
        }
        else
        {
            display.Displaylean.TurnOff();
        }
    }

}