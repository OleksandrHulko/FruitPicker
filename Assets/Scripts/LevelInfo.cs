using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelInfo : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Text levelText = null;
    [SerializeField]
    private Text taskText = null;
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioWin = null;
    [SerializeField]
    private AudioClip audioLose = null;
    #endregion

    #region Private Fields
    private int collectedApples  = 0;
    private int collectedBananas = 0;
    private int collectedLemons  = 0;
    
    private int neededApples  = 0;
    private int neededBananas = 0;
    private int neededLemons  = 0;

    private int level = 0;
    
    private readonly string level_key = "level";
    #endregion

    #region Public Fields
    public static Action<bool> onWin  = null;
    #endregion


    #region Public Methods
    public void SetNewLevelInfo()
    {
        ResetCollectedFruit();
        GetLevel();
        GenerateTask();
        SetLevelText();
        Show();
    }

    public void SetOldLevelInfo()
    {
        ResetCollectedFruit();
        SetTaskText();
        Show();
    }
    
    public void UpdateLevelInfo( FruitType fruitType )
    {
        switch (fruitType)
        {
            case FruitType.APPLE : Increment(ref collectedApples); 
                break;
            case FruitType.BANANA : Increment(ref collectedBananas);
                break;
            case FruitType.LEMON : Increment(ref collectedLemons);
                break;
        }

        SetTaskText();

        if (isWin())
        {
            onWin?.Invoke(true);
            audioSource.PlayAudioClip(audioWin);
            level++;
            SaveLevel();
        }
        else if (isLose())
        {
            onWin?.Invoke(false);
            audioSource.PlayAudioClip(audioLose);
        }

        void Increment( ref int counter )
        {
            counter++;
        }

        bool isLose()
        {
            bool redundantApples  = collectedApples  > neededApples;
            bool redundantBananas = collectedBananas > neededBananas;
            bool redundantLemons  = collectedLemons  > neededLemons;
            
            return redundantApples || redundantBananas || redundantLemons;
        }

        bool isWin()
        {
            bool allApplesCollected  = collectedApples  == neededApples;
            bool allBananasCollected = collectedBananas == neededBananas;
            bool allLemonsCollected  = collectedLemons  == neededLemons;

            return allApplesCollected && allBananasCollected && allLemonsCollected;
        }
    }

    public void Show( bool show = true )
    {
        canvasGroup.Show(show);
    }
    #endregion
    
    #region Private Methods
    private void GetLevel()
    {
        if (level == 0)
            level = PlayerPrefs.GetInt(level_key, 1);
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt(level_key, level);
    }

    private void GenerateTask()
    {
        int minFruitCount  = 1;
        int maxFruitCount  = 5;
        int fruitTypeCount = 3;
        int randomValue    = getRandomInt(0, fruitTypeCount - 1);

        int[] fruit = { getRandomInt(minFruitCount, maxFruitCount), getRandomInt(minFruitCount, maxFruitCount), getRandomInt(minFruitCount, maxFruitCount)};

        for (int i = 0; i < randomValue; i++)
        {
            fruit[Random.Range(0, fruitTypeCount)] = 0;
        }

        neededApples  = fruit[0];
        neededBananas = fruit[1];
        neededLemons  = fruit[2];
        
        SetTaskText();

        int getRandomInt(int min, int max)
        {
            return Random.Range(min, max + 1);
        }
    }
    
    private void SetLevelText()
    {
        levelText.text = $"Level № {level}";
    }

    private void SetTaskText()
    {
        string apples  = neededApples  == 0 ? null : $"{collectedApples}/{neededApples} apples";
        string bananas = neededBananas == 0 ? null : $"{collectedBananas}/{neededBananas} bananas";
        string lemons  = neededLemons  == 0 ? null : $"{collectedLemons}/{neededLemons} lemons";

        List<string> fruitText = new List<string>();
        StringBuilder stringBuilder = new StringBuilder();

        initList();
        initStringBuilder();
        setText();

        void initList()
        {
            if (apples != null)
                fruitText.Add(apples);
            
            if (bananas != null)
                fruitText.Add(bananas);

            if (lemons != null)
                fruitText.Add(lemons);
        }

        void initStringBuilder()
        {
            for (int i = 0; i < fruitText.Count; i++)
            {
                bool is_last_text = i == fruitText.Count - 1;

                stringBuilder.Append(fruitText[i]);
            
                if (!is_last_text)
                    stringBuilder.Append(" | ");
            }
        }

        void setText()
        {
            taskText.text = stringBuilder.ToString();
        }
    }

    private void ResetCollectedFruit()
    {
        collectedApples  = 0;
        collectedBananas = 0;
        collectedLemons  = 0;
    }
    #endregion
}
