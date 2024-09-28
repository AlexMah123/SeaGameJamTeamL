using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Config")]
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject playerSpawnPoint;

    [Header("Kid Config")]
    [SerializeField] AIEnemy kidObj;
    [SerializeField] GameObject kidSpawnPoint;
    [SerializeField] float cooldownTimer;

    [Header("Timer Ref")]
    [SerializeField] Timer timerManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(timerManager == null)
        {
            Debug.LogError("TimerManager not referenced");
        }
    }

    private void Update()
    {
        
    }

    void CaughtKid()
    {
        //stop kid running, reset and invoke a timer to continue running
        kidObj.StopRunning();
        kidObj.agent.Warp(kidSpawnPoint.transform.position);
        Invoke(nameof(kidObj.StartRunning), cooldownTimer);

        //reset player
        playerObj.transform.position = playerSpawnPoint.transform.position;

        //increment hour
        timerManager.IncrementHour();
    }

}
