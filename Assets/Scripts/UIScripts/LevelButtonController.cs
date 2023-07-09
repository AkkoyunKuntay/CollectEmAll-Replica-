using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    [Header("Level Settings")]
    public int level;
    
    [Header("Debug")]
    [SerializeField] bool isLevelCompleted;
    [SerializeField] bool isPlayable;
    public int highestScore;

    private int completedKey;
    private int playableKey;

    [Header("References")]
    public Button playbutton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI HighestScoreText;

    private void Awake()
    {
        highestScore = PlayerPrefs.GetInt(gameObject.name);
        completedKey = PlayerPrefs.GetInt(gameObject.name + "isCompleted");
        playableKey =  PlayerPrefs.GetInt(gameObject.name + "isPlayable");

        if (completedKey == 0) isLevelCompleted = false;
        else isLevelCompleted = true;

        if (playableKey == 0) SetIsPlayable(false); 
        else SetIsPlayable(true);

        if(level == 0 ) SetIsPlayable(true);
    
    }

    private void Start()
    {
        LevelHandler.instance.LevelIsEndedEvent += OnLevelEnded;

        levelText.text = "Level" + (level+1);
        HighestScoreText.text = "Highest Score :" + highestScore;
    }

    private void OnLevelEnded(bool state)
    {
        if (state)
        {
            highestScore = PlayerPrefs.GetInt(gameObject.name);
            HighestScoreText.text = "Highest Score :" + highestScore;
        }
    }

    public void OnPlayButtonClicked(int levelIndex)
    {
        levelIndex = level;
        CanvasManager.instance.OnPlayButtonClicked(levelIndex);
    }
    public void SetLevelAsCompleted()
    {
        isLevelCompleted = true;
        PlayerPrefs.SetInt(gameObject.name +"isCompleted", 1);
    }
    public void SetIsPlayable(bool state)
    {
        if(state) 
        {
            isPlayable = true;
            playbutton.interactable = true;
            PlayerPrefs.SetInt(gameObject.name + "isPlayable", 1);
        }
        else
        {
            isPlayable = false;
            playbutton.interactable = false;
            PlayerPrefs.SetInt(gameObject.name + "isPlayable", 0);
        }
    }
    public bool IsLevelCompleted()
    {
        return isLevelCompleted;
    }
}
