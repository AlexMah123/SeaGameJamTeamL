using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

[RequireComponent(typeof(SphereCollider))]
public class InteractComponent : MonoBehaviour
{
    [Header("Interact Configs")]
    [SerializeField] GameObject interactPromptUI;
    [SerializeField] SphereCollider interactCollider;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask obstacleLayer;

    private GameObject closestInteractableObj = null;
    private List<GameObject> interactables = new();

    private void Awake()
    {
        if (interactPromptUI == null)
        {
            throw new MissingReferenceException("InteractUIPrompt not assigned");
        }

        interactCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        UpdateInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleOnInteract();
        }
    }

    private void UpdateInteractable()
    {
        if (closestInteractableObj)
        {
            if (!interactPromptUI.activeSelf)
            {
                interactPromptUI.gameObject.SetActive(true);
            }
        }
        else
        {
            if (interactPromptUI.activeSelf)
            {
                interactPromptUI.gameObject.SetActive(false);
            }
        }
    }

    private void HandleOnInteract()
    {
        if (closestInteractableObj != null)
        {
            closestInteractableObj.GetComponent<IInteractable>().Interact();
        }
    }

    private void SetClosestInteractable()
    {
        //early return
        if (interactables.Count <= 0)
        {
            closestInteractableObj = null;
            return;
        }

        //cache
        float closestDistance = Mathf.Infinity;

        foreach (GameObject interactableObj in interactables)
        {
            RaycastHit obstacleHit;
            Vector3 directionToInteractable = interactableObj.transform.position - transform.position;

            //check if any obstacle between them.
            if (Physics.Raycast(transform.position, directionToInteractable, out obstacleHit, directionToInteractable.magnitude, obstacleLayer))
            {
                //if hit an obstacle, skip
                continue;
            }

            float distance = Vector3.Distance(transform.position, interactableObj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractableObj = interactableObj;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactables.Add(collision.gameObject);
            SetClosestInteractable();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactables.Remove(collision.gameObject);
            SetClosestInteractable();
        }
    }
}
