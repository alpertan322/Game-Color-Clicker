using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private RectangleController rectController;

    [SerializeField]
    private PointController pointController;

    private List<Color> allColors;

    private int curDifficulty;

    [SerializeField]
    private TextMeshProUGUI difficultyText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private MapGenerator mapGenerator;

    private int score;

    private int livesLeft;

    private bool gameOpened;

    [SerializeField]
    private TextMeshProUGUI lifeText;

    [SerializeField]
    private List<GameObject> randomMapRects;

    [SerializeField]
    private GameObject loadingText;

    [SerializeField]
    private List<GameObject> premadeMapRects;

    private bool randomMap;
    private bool preMap;




    // Start is called before the first frame update
    void Start()
    {
        gameOpened = false;
        curDifficulty = 0;
        score = 0;
        livesLeft = 3;
        lifeText.text = livesLeft.ToString();
        allColors = new List<Color>();
        allColors.Add(Color.blue);
        allColors.Add(Color.red);
        allColors.Add(Color.green);
        allColors.Add(Color.cyan);
        allColors.Add(Color.yellow);
        allColors.Add(Color.magenta);

        SetDifficultyText();
        scoreText.text = score.ToString();
    }

    void Update()
    {
        if (randomMap && !gameOpened && mapGenerator.GetGenerationFinished())
        {
            StartRandomMap();
            gameOpened = true;
        }

        if (preMap && !gameOpened && pointController.GetGameIsReady())
        {
            gameOpened = true;
            loadingText.SetActive(false);
            mainMenu.SetActive(false);
        }
    }

    public void StartRandomMap()
    {
        Debug.Log("random");
        pointController.enabled = true;
        pointController.GeneratePoints();
        rectController.enabled = true;
        GetComponent<ClickController>().enabled = true;
        loadingText.SetActive(false);
        rectController.SetRectangles(randomMapRects);
    }

    public void SetLoadingScreen()
    {
        randomMap = true;
        loadingText.SetActive(true);
        mainMenu.SetActive(false);

    }

    public void StartPremadeMap()
    {
        for (int i = 0; i < premadeMapRects.Count; i++)
        {
            premadeMapRects[i].SetActive(true);
        }

        for (int i = 0; i < randomMapRects.Count; i++)
        {
            randomMapRects[i].SetActive(false);
        }
        mapGenerator.enabled = false;
        pointController.enabled = true;
        pointController.GeneratePoints();
        loadingText.SetActive(true);
        rectController.enabled = true;
        GetComponent<ClickController>().enabled = true;
        preMap = true;
    }

    // Update is called once per frame
    public void LifeDown()
    {
        livesLeft -= 1;
        lifeText.text = livesLeft.ToString();
        if (livesLeft == 0)
        {
            GameOver();
        }
    }

    public void ScoreUp()
    {
        score += 1;
        SetGameDifficulty();
        SetDifficultyText();
        scoreText.text = score.ToString();
    }

    private void GameOver()
    {
        GetComponent<ClickController>().enabled = false;
        GetComponent<LeaderboardController>().ShowLeaderBoard();
    }

    private void SetGameDifficulty()
    {
        int prevDifficulty = curDifficulty;
        curDifficulty = score / 5;
        if (prevDifficulty != curDifficulty)
        {
            for (int i = 0 ; i < 7; i++)
            {
                pointController.IncreasePointCount();
            }
                pointController.SetPointsChangeCount(6 * curDifficulty);
                pointController.SetPointsChangeCooldown(0.3f - 0.03f * curDifficulty);

                rectController.SetCollapseCooldown(3f - 0.8f * curDifficulty);
                rectController.SetCollapseSpeed(1f - 0.17f * curDifficulty);

                List<Color> temp = new List<Color>(allColors);
                for (int i = curDifficulty; i < 3; i++)
                {
                    Debug.Log("temp: " + temp.Count + "cur idff:  " + curDifficulty);
                    temp.RemoveAt(temp.Count - 1);
                }
                pointController.SetColors(temp);
                rectController.RandomizeColors();
                rectController.SetRectangleColors(temp);


        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    private void SetDifficultyText()
    {
        string difText = "";
        switch(curDifficulty)
        {
            case (1):
                difText = "Medium";
                break;
            case (2):
                difText = "Hard";
                break;
            case (3):
                difText = "Extreme";
                break;
            default:
                difText = "Easy";
                break;
        }
        difficultyText.text = difText;

    }
}
