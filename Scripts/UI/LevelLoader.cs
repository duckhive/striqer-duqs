using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1;
    public GameObject loadingScreen;
    public Slider slider;
    
    [SerializeField] private int mainMenuIndex = 0;
    [SerializeField] private int playNowIndex = 1;
    [SerializeField] private int quackPlayIndex = 2;
    [SerializeField] private int gameplayIndex = 3;
    private static readonly int Start = Animator.StringToHash("Start");

    private void Awake()
    {
        transition = GetComponentInChildren<Animator>();
    }

    private IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }

    private IEnumerator LoadLevelAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        loadingScreen.SetActive(true);
        
        while (!operation.isDone )
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(mainMenuIndex));
    }
    
    public void LoadMainMenuAsync()
    {
        StartCoroutine(LoadLevelAsync(mainMenuIndex));
    }
    
    public void LoadPlayNow()
    {
        StartCoroutine(LoadLevel(playNowIndex));
    }
    
    public void LoadPlayNowAsync()
    {
        StartCoroutine(LoadLevelAsync(playNowIndex));
    }

    public void LoadQuackPlay()
    {
        StartCoroutine(LoadLevel(quackPlayIndex));
    }
    
    public void LoadQuackPlayAsync()
    {
        StartCoroutine(LoadLevelAsync(quackPlayIndex));
    }
    
    public void LoadGameplay()
    {
        StartCoroutine(LoadLevel(gameplayIndex));
    }
    
    public void LoadGameplayAsync()
    {
        StartCoroutine(LoadLevelAsync(gameplayIndex));
    }
}
