using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject ScreenSeparator;

    
    [SerializeField] private GameObject MainPannel;
    
    [Header("Scores")]
    [SerializeField] private GameObject ScorePannel;
    [SerializeField] private TMP_Text RedScoreText;
    [SerializeField] private TMP_Text BlueScoreText;
    
    
    [SerializeField] private TMP_Text countdownText;
    
    
    [Header("Winner")]
    [SerializeField] private float pre_winner_cooldown = 1;
    [SerializeField] private float post_winner_cooldown = 1;
    [SerializeField] private GameObject WinnerPannel;
    [SerializeField] private GameObject RedWinner;
    [SerializeField] private GameObject BlueWinner;

    private void Start()
    {
        HideWinner();
        MainPannel.SetActive(true);
        ScreenSeparator.SetActive(false);
        countdownText.text = "";
        PrintScores();
    }

    private void OnEnable()
    {
        GlobalEvents.PlayerLost += OnPlayerLost;
        GlobalEvents.RoundStart += OnRoundStart;
    }

    private void OnDisable()
    {
        GlobalEvents.PlayerLost -= OnPlayerLost;
        GlobalEvents.RoundStart -= OnRoundStart;
    }

    void OnPlayerLost(Team team)
    {
        Team winner = team == Team.RED ? Team.BLUE : Team.RED; 
        StartCoroutine(ShowWinnerAfterCoolDown(winner));
        ScreenSeparator.SetActive(false);
    }

    void OnRoundStart()
    {
        HideWinner();
        HideScore();
        MainPannel.SetActive(false);
        StartCoroutine(CountDown());
    }

    private IEnumerator ShowWinnerAfterCoolDown(Team team)
    {
        yield return new WaitForSecondsRealtime(pre_winner_cooldown);
        ShowWinner(team);
        yield return new WaitForSecondsRealtime(post_winner_cooldown);
        HideWinner();
        GlobalEvents.ShowScores.Invoke();
        ShowScore();
    }

    private void HideWinner()
    {
        WinnerPannel.SetActive(false);
    }

    private void HideScore()
    {
        ScorePannel.SetActive(false);
    }

    private void ShowScore()
    {
        PrintScores();
        ScorePannel.SetActive(true);
    }

    private void ShowWinner(Team team)
    {
        WinnerPannel.SetActive(true);
        BlueWinner.SetActive(team == Team.BLUE);
        RedWinner.SetActive(team == Team.RED);
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "";
        GlobalEvents.CountdownEnd.Invoke();
        ScreenSeparator.SetActive(true);
    }

    private void PrintScores()
    {
        RedScoreText.text = GameManager.redScore.ToString();
        BlueScoreText.text = GameManager.blueScore.ToString();
    }
    
}