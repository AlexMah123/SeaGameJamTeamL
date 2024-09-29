using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light lightToTrigger;
    [SerializeField] AudioSource lightBuzzSource;
    [SerializeField] bool isActiveOnStart = true;

    private void Start()
    {
        lightToTrigger.enabled = isActiveOnStart;
    }

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

        lightBuzzSource.enabled = lightToTrigger.enabled;

        SFXManager.Instance.PlaySoundFXClip("LightSwitch", transform);
    }

}
