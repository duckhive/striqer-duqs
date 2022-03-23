using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data_Scripts;
using Gameplay;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Util;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    #region GAMEPLAY
    
    [BoxGroup("Gameplay UI")] public Canvas gameplayCanvas;
    
    [BoxGroup("Gameplay UI")] public TMP_Text timeText;
    [BoxGroup("Gameplay UI")] public TMP_Text awayScoreText;
    [BoxGroup("Gameplay UI")] public TMP_Text homeScoreText;
        
    #endregion

    #region PAUSE MENU

    [BoxGroup("Pause Menu UI")] public Canvas pauseMenuCanvas;

    #endregion

    #region GAME OVER

    [BoxGroup("Game Over UI")] public Canvas gameOverCanvas;

    #endregion
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
    }

    /*private void Update()
    {
        // gameplay:
        if (GameManager.instance is not null)
        {
            if (GameManager.instance.timer > 0 && GameManager.instance.gameActive)
                GameManager.instance.timer -= Time.deltaTime;

            DisplayTime(GameManager.instance.timer);

            
                if(awayScoreText is not null)
                    awayScoreText.text = "" + AwayTeamData.instance.score;
                else
                    Debug.LogWarning("Away Score Text game object not set.");


                if(homeScoreText is not null)
                    homeScoreText.text = "" + HomeTeamData.instance.score;
                else
                    Debug.LogWarning("Home Score Text game object not set.");
        }
    }*/

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
