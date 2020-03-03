using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    private int iScore;
    public bool bRespawning = false;

    private const string HIGHSCORE_PREF = "HIGHSCORE";
    private int iHighScore;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public GameObject restartButton;

    public static GameManager instance;

    private void Awake() {
        if (GameManager.instance == null) {
            GameManager.instance = this;
        } else if (GameManager.instance != this) {
            Destroy(GameManager.instance.gameObject);
            GameManager.instance = this;
        }
    }

    private void Start() {
        if (PlayerPrefs.HasKey(HIGHSCORE_PREF)) {
            iHighScore = GetiPref(HIGHSCORE_PREF);
        } else {
            iHighScore = 0;
        }
        iScore = 0;
    }

    public void GameOver() {
        //Check if iScore > Highscore if so update.
        if (iScore > iHighScore) {
            iHighScore = iScore;
            SetPref(HIGHSCORE_PREF, iHighScore);
        }
        highScoreText.text = ("High score: " + iHighScore);
        if (PipeManager.instance) {
            PipeManager.instance.bSpawning = false;
            PipeManager.instance.StopSpawning();
        }
        highScoreText.gameObject.SetActive(true);
        restartButton.SetActive(true);
    }

    public void RestartScene() {
        iScore = 0;
        UpdateScore();
    }

    public void IncrementScore() {
        iScore++;
        UpdateScore();
    }

    private void UpdateScore() {
        scoreText.text = "Score: " + iScore;
    }

    private void SetPref(string _strKey, int _iValue) {
        PlayerPrefs.SetInt(_strKey, _iValue);
    }

    private int GetiPref(string _strKey, int _iDefault = 0) {
        return PlayerPrefs.GetInt(_strKey, Convert.ToInt32(_iDefault));
    }
}
