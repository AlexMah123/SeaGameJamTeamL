using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIEnemy : MonoBehaviour
{
    [Header("Waypoint Config")]
    [SerializeField] List<GameObject> waypointList = new();

    [Header("AI Config")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float interactRadius = 1f;
    [SerializeField] LayerMask interactableLayer;


    public NavMeshAgent agent;
    private GameObject lastChosenWaypoint = null;
    private GameObject currentWaypoint = null;

    Coroutine coroutine = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        if(waypointList.Count == 0)
        {
            Debug.LogError("No waypoint assigned");
        }
    }

    void Start()
    {
        StartRunning();
    }

    public void StartRunning()
    {
        if (waypointList.Count > 0)
        {
            agent.isStopped = false;
            coroutine = StartCoroutine(FindSwitch());

            SFXManager.Instance.PlaySoundFXClip("ChildLaughter", transform);
        }
    }

    public void StopRunning()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        agent.isStopped = true;
        agent.ResetPath();
    }

    IEnumerator FindSwitch()
    {
        while (true)
        {
            if(agent.isStopped)
            {
                yield break;
            }

            Vector3 nextLocation = GetNextWaypointLocation();
            agent.SetDestination(nextLocation);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.localPosition, interactRadius, transform.forward, interactRadius, interactableLayer, QueryTriggerInteraction.Collide);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    IInteractable interactableObj = hit.collider.gameObject.GetComponent<IInteractable>();

                    if (interactableObj != null)
                    {
                        interactableObj.Interact(this.gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(waitTime);
        }       
    }

    GameObject GetRandomWaypoint()
    {
        GameObject randomWaypoint = null;

        int randomInt = Random.Range(0, waypointList.Count);

        try
        {
            randomWaypoint = waypointList[randomInt];
        }
        catch
        {
            Debug.LogError("Waypoint index out of range");
        }

        if (randomWaypoint != lastChosenWaypoint)
        {
            currentWaypoint = randomWaypoint;
        }
        else
        {
            GetRandomWaypoint();
        }

        return currentWaypoint;
    }

    [ContextMenu("AIEnemy/GetRandomWaypointLocation")]
    Vector3 GetNextWaypointLocation()
    {
        GetRandomWaypoint();

        if (currentWaypoint == null) return Vector3.zero;

        RaycastHit ray;
        Physics.Raycast(currentWaypoint.transform.position, Vector3.down, out ray, Mathf.Infinity);

        return ray.point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

}