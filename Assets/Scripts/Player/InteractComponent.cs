using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);
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
        // Update the closest interactable object every frame
        SetClosestInteractable();
        UpdateInteractPromptUI();

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleOnInteract();
        }
    }

    private void UpdateInteractPromptUI()
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
            var interactable = closestInteractableObj.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this.gameObject);
            }
        }
    }

    private void SetClosestInteractable()
    {
        if (interactables.Count == 0)
        {
            closestInteractableObj = null;
            return;
        }

        float closestDistance = Mathf.Infinity;
        GameObject newClosest = null;

        foreach (GameObject interactableObj in interactables)
        {
            Vector3 directionToInteractable = interactableObj.transform.position - transform.position;

            // Raycast to check for obstacles between the player and interactable
            if (Physics.Raycast(transform.position, directionToInteractable, out RaycastHit obstacleHit, directionToInteractable.magnitude, obstacleLayer))
            {
                continue; // Skip if an obstacle is hit
            }

            float distance = directionToInteractable.magnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                newClosest = interactableObj;
            }
        }

        // Only update the closest interactable if it's changed
        if (newClosest != closestInteractableObj)
        {
            closestInteractableObj = newClosest;
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

    private void OnTriggerStay(Collider other)
    {
        SetClosestInteractable();
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

