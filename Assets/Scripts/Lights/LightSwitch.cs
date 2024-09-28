using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] Light lightToTrigger;

    public void Interact()
    {
        lightToTrigger.enabled = !lightToTrigger.enabled;
    }


}
