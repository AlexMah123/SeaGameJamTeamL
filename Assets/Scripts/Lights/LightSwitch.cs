using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] Light lightToTrigger;

    public void Interact(GameObject interactor)
    {
        if(interactor.CompareTag("Player"))
        {
            lightToTrigger.enabled = !lightToTrigger.enabled;
        }
        else
        {
            lightToTrigger.enabled = true;
        }
    }


}
