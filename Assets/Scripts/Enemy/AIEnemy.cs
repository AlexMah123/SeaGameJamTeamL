using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    [Header("Waypoint Config")]
    [SerializeField] List<GameObject> waypointList = new();

    [Header("AI Config")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float interactRadius = 1f;
    [SerializeField] LayerMask interactableLayer;


    private NavMeshAgent agent;
    private GameObject lastChosenWaypoint = null;
    private GameObject currentWaypoint = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        if(waypointList.Count == 0)
        {
            Debug.LogWarning("No waypoint assigned");
        }
    }

    void Start()
    {
        StartRunning();
    }

    public void StartRunning()
    {
        StartCoroutine(FindSwitch());
    }

    public void StopRunning()
    {
        StopCoroutine(FindSwitch());
    }

    IEnumerator FindSwitch()
    {
        while (true)
        {
            Vector3 nextLocation = GetNextWaypointLocation();
            agent.SetDestination(nextLocation);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            Debug.Log("Try Interact");

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.localPosition, interactRadius, transform.forward, interactRadius, interactableLayer, QueryTriggerInteraction.Collide);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    IInteractable interactableObj = hit.collider.gameObject.GetComponent<IInteractable>();

                    if (interactableObj != null)
                    {
                        Debug.Log("On Switch");
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

        randomWaypoint = waypointList[randomInt];

        if(randomWaypoint != lastChosenWaypoint)
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

        Debug.Log(ray.point);

        return ray.point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

}