using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TimerManager timerManager;

    [Header("EndingImage")]
    [SerializeField] GameObject endingImageObj;
    [SerializeField] TextMeshProUGUI billText;

    [Header("Lights Data")]
    [SerializeField] List<GameObject> lightsObjList;
    [SerializeField] int lightsCount;

    [Header("CaughtKid Config")]
    [SerializeField] GameObject caughtPrompt;

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

    private void Start()
    {
        InvokeRepeating(nameof(HandleBillCalculation), 0.1f, 1f);
    }

    private void Update()
    {
        HandleTimer();
    }

    private void HandleBillCalculation()
    {
        foreach(var lights in lightsObjList)
        {
            LightSwitch lightSwitch = lights.GetComponent<LightSwitch>();

            if (lightSwitch)
            {
                if (!lightSwitch.lightToTrigger.enabled)
                    continue;
                
                lightsCount++;
            }
        }
    }

    private void HandleTimer()
    {
        if (!TimerManager.timerIsRunning)
        {
            if (!endingImageObj.activeSelf)
            {
                kidObj.StopRunning();

                //prompt ending image
                endingImageObj.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


                CancelInvoke(nameof(HandleBillCalculation));

                var endCalculation = lightsCount * 2.50f;
                billText.text = $"RM {endCalculation.ToString("0.00")}";
            }
        }
    }

    [ContextMenu("GameManager/SimulateCaughtKid")]
    public IEnumerator CaughtKid()
    {
        //stop kid running, reset and invoke a timer to continue running
        kidObj.agent.Warp(kidSpawnPoint.transform.position);
        kidObj.StopRunning();

        //reset player
        CharacterController characterController = playerObj.GetComponent<CharacterController>();

        characterController.enabled = false;
        playerObj.transform.position = playerSpawnPoint.transform.position;
        characterController.enabled = true;

        //increment hour
        timerManager.IncrementHour();
        caughtPrompt.gameObject.SetActive(true);

        yield return new WaitForSeconds(cooldownTimer);

        kidObj.StartRunning();
        caughtPrompt.gameObject.SetActive(false);
    }

}
