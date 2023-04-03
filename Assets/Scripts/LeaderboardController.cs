using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LeaderboardController : MonoBehaviour
{
    /*
        This script is for a simple leaderboard ranking in the game.
    */
    [SerializeField]
    private TextMeshProUGUI[] nameTexts;
    [SerializeField]
    private TextMeshProUGUI[] scoreTexts;

    [SerializeField]
    private TextMeshProUGUI inputText;

    [SerializeField]
    private GameObject inputField;

    [SerializeField]
    private GameObject inputButton;

    [SerializeField]
    private GameObject leaderBoard;

    [SerializeField]
    private GameObject replayButton;

    private string input;

    private Dictionary<string, int> highScores = new Dictionary<string, int>();

    private const string HighScoreKey = "HighScores";
    private const int NumScoresToShow = 4;

    private bool firstPlay;
 
    void Awake ()
    {
    }
   private void Start()
    {
        if (Application.isEditor == false) 
        {
             if (PlayerPrefs.GetInt("FirstPlay", 1) != 1) 
             {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt("FirstPlay", 1);
                PlayerPrefs.Save();
             } 
        }
        LoadHighScores();
        UpdateUI();
        
    }

    private void LoadHighScores()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            string[] entries = PlayerPrefs.GetString(HighScoreKey).Split(';');
            foreach (string entry in entries)
            {
                if (string.IsNullOrEmpty(entry))
                    continue;

                string[] entryData = entry.Split(':');
                if (entryData.Length != 2)
                    continue;

                string name = entryData[0];
                int score = int.Parse(entryData[1]);

                if (!highScores.ContainsKey(name))
                {
                    highScores.Add(name, score);
                }
                else if (highScores[name] < score)
                {
                    highScores[name] = score;
                }
            }
        }
    }

    private void SaveHighScores()
    {
        List<string> entries = new List<string>();
        foreach (KeyValuePair<string, int> highScore in highScores)
        {
            entries.Add(highScore.Key + ":" + highScore.Value);
        }

        string data = string.Join(";", entries.ToArray());
        PlayerPrefs.SetString(HighScoreKey, data);
        PlayerPrefs.Save();
    }

    public void AddScore(string name, int score)
    {
        if (highScores.ContainsKey(name))
        {
            if (score > highScores[name])
            {
                highScores[name] = score;
            }
        }
        else
        {
            highScores.Add(name, score);
        }

        SaveHighScores();
        UpdateUI();
    }

    private void UpdateUI()
    {
        List<KeyValuePair<string, int>> sortedScores = new List<KeyValuePair<string, int>>(highScores);
        sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

        for (int i = 0; i < NumScoresToShow; i++)
        {
            if (i < sortedScores.Count)
            {
                nameTexts[i].text = sortedScores[i].Key;
                scoreTexts[i].text = sortedScores[i].Value.ToString();
            }
            else
            {
                nameTexts[i].text = "";
                scoreTexts[i].text = "";
            }
        }
    }

    public void ShowLeaderBoard()
    {
        leaderBoard.SetActive(true);
    }

    public void SubmitScore()
    {
        AddScore(inputText.text, 2);
        inputButton.SetActive(false);
        inputField.SetActive(false);
        replayButton.SetActive(true);

    }
}
