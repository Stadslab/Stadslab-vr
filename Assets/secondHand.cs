using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class secondHand : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        foreach( var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(onSecondHandGrab);
            item.onSelectExited.AddListener(onSecondHandGrab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("grab");
    }

    public void onSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("release");
    }

   
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor);
    }
}
