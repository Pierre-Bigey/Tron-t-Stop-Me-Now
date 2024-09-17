using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction continueAction;

    [SerializeField] private GameObject MainCamera;
    
    public static int redScore = 0;
    public static int blueScore = 0;

    private bool isInGame = false;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    

    private void Update()
    {
        if (!isInGame && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartGame();
        }

        if (!isInGame && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Quit();
        }
    }

    private void OnEnable()
    {
        playerInputActions.Tron.Start.performed += OnStart;
        GlobalEvents.PlayerLost += OnPlayerLost;
        GlobalEvents.ShowScores += OnShowScore;
    }

    private void OnDisable()
    {
        playerInputActions.Tron.Start.performed -= OnStart;
        GlobalEvents.PlayerLost -= OnPlayerLost;
        GlobalEvents.ShowScores -= OnShowScore;
    }

    void OnStart(InputAction.CallbackContext ctx)
    {
        Debug.Log("Game Start");
        if(!isInGame) StartGame();
    }
    
    public void StartGame()
    {
        isInGame = true;
        MainCamera.SetActive(false);
        GlobalEvents.RoundStart.Invoke();
    }
    
    private void OnPlayerLost(Team team)
    {
        if (team == Team.BLUE)
        {
            redScore++;
        }
        else
        {
            blueScore++;
        }
    }

    void OnShowScore()
    {
        MainCamera.SetActive(true);
        isInGame = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
