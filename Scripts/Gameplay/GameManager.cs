using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using Util;
using Cinemachine;
using Data_Scripts;
using Sirenix.OdinInspector;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        [BoxGroup("Switches")] public bool sideView;
        [BoxGroup("Switches")] public bool freeplay;

        public GameData gameData;
        public HomeTeamData homeTeamData;
        public AwayTeamData awayTeamData;
        
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Possessing Agent")] public List<Agent> lastAgentWithPoss;
        
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current States")] public bool goalScored;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current States")] public bool gameActive;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current States")] public bool ballShot;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current States")] public bool ballPassed;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current States")] public bool gamePaused;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current States")] public bool shooting;

        [HideInInspector] public float timer;
        
        [BoxGroup("Gameplay UI")] public Canvas pauseCanvas;
        [BoxGroup("Gameplay UI")] public TMP_Text timerText;
        [BoxGroup("Gameplay UI")] public TMP_Text awayScoreText;
        [BoxGroup("Gameplay UI")] public TMP_Text homeScoreText;

        
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            
            Time.timeScale = 1;
            gameActive = true;

            timer = gameData.matchLength;
            gameData.crowdIdleSfx.Play();
        }

        private void OnEnable()
        {
            if(PlaylistManager.instance is not null)
                Destroy(PlaylistManager.instance.gameObject);
        }

        private void Update()
        {
            if (timer > 0 && gameActive)
                timer -= Time.deltaTime;

            DisplayTime(timer);

            
            if(awayScoreText is not null)
                awayScoreText.text = "" + awayTeamData.score;
            else
                Debug.LogWarning("Away Score Text game object not set.");


            if(homeScoreText is not null)
                homeScoreText.text = "" + homeTeamData.score;
            else
                Debug.LogWarning("Home Score Text game object not set.");
        }

        public void DisplayTime(float timeToDisplay)
        {
            if (timeToDisplay < 0)
                timeToDisplay = 0;

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timerText.text = $"{minutes:0}:{seconds:00}";
        }
        
        public void PauseGame()
        {
            gamePaused = true;
            pauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        public void ResumeGame()
        {
            Time.timeScale = 1;
            pauseCanvas.gameObject.SetActive(false);
            StartCoroutine(GamePauseFalse());
        }

        private IEnumerator GamePauseFalse()
        {
            yield return new WaitForSeconds(0.25f);
            gamePaused = false;
        }

        public IEnumerator ResetScene()
        {
            Util.Game.ResetBall(Ball.instance);
            //Util.Game.ResetAllAgents(allAgents);
            //Util.Game.FreezeAllAgents(allAgents);
            yield return new WaitForSeconds(1);
            //Util.Game.UnfreezeAllAgents(allAgents);
        }

        public IEnumerator TeamScoreEvent(Team team)
        {
            switch (team.teamEnum)
            {
                case Game.TeamEnum.Away:
                    awayTeamData.score += 1;
                    break;
                case Game.TeamEnum.Home:
                    homeTeamData.score += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            gameData.crowdCheerOnScoreSfx.Play();

            gameActive = false;
            
            yield return new WaitForSeconds(1);

            //StartCoroutine(ResetScene());
            Game.ResetBall(Ball.instance);
            gameActive = true;
        }
        
        public void LoadMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}
