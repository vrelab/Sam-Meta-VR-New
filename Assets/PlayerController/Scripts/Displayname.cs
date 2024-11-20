using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Displayname : MonoBehaviour
{
    public GameObject Display;
    public LeanToggle Displaylean;
    [SerializeField] TMP_Text displayName;
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
       // displayName = Display.GetComponentInChildren<TMP_Text>();
       // displayName.text = "GUEST";
    }


    public void SetTarget(Transform target)
    {
        Target = target;
    }
    public void Setname(string name)
    {
        displayName.text = name;
    }
    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {

            Display.transform.LookAt(Target);
        }
    }
}
