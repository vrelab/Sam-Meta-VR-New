using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform VRTarget;
    public Transform rigTarget;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Vector3 handsRotationOffset;

    public void Map()
    {
        rigTarget.position = VRTarget.TransformPoint(positionOffset);
        //rigTarget.position = Vector3.Lerp(rigTarget.position, VRTarget.TransformPoint(positionOffset), 20 * Time.deltaTime);

        var currentRotationOffset = OVRInput.IsControllerConnected(OVRInput.Controller.Hands) ? handsRotationOffset : rotationOffset;

        rigTarget.rotation = VRTarget.rotation * Quaternion.Euler(currentRotationOffset);
        //rigTarget.rotation = Quaternion.Lerp(rigTarget.rotation, VRTarget.rotation * Quaternion.Euler(currentRotationOffset), 20 * Time.deltaTime);
    }
}

public class HeadBodyRig : MonoBehaviour
{
    [SerializeField]
    PhotonView photonView;

    public VRMap head;
    public VRMap rightHand;
    public VRMap leftHand;

    public Transform headConstraint;
    Vector3 offset;

    public float turnFactor = 1f;
    public ForwardAxis forwardAxis;

    public enum ForwardAxis
    {
        blue,
        green,
        red
    }

    void Start()
    {
        if (photonView)
            if (!photonView.IsMine)
            {
                this.enabled = false;
                return;
            }

        offset = transform.position - headConstraint.position;

        var cam =UserManager.Instance.mainCamera;
        var cameraParent = cam.transform.parent;
        head.VRTarget = cam.transform;
        rightHand.VRTarget = UserManager.Instance.right.transform;
        leftHand.VRTarget = UserManager.Instance.left.transform;
    }

    void FixedUpdate()
    {
        transform.position = headConstraint.position + offset;
        Vector3 projectionVector = headConstraint.up;
        switch (forwardAxis)
        {
            case ForwardAxis.green:
                projectionVector = headConstraint.up;
                break;
            case ForwardAxis.blue:
                projectionVector = headConstraint.forward;
                break;
            case ForwardAxis.red:
                projectionVector = headConstraint.right;
                break;
        }
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(projectionVector, Vector3.up).normalized, Time.deltaTime * turnFactor);

        head.Map();
        rightHand.Map();
        leftHand.Map();
    }


    public Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform child = null;
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            child = parent.GetChild(i);
            if (child.name == childName)
            {
                break;
            }
            else
            {
                child = RecursiveFindChild(child, childName);
                if (child != null)
                {
                    break;
                }
            }
        }

        return child;
    }
}
