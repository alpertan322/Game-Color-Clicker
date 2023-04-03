using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    /*
        This sript is for placing points onto the generated map. It scans through the random map (or the standard map), checks if it is possible to
        place a point (it uses dozen of raycasts shooting to surrounding of the point to check if it collides or not), then places the point accordingly.
        
        It also vaporizes a random point and respawns another point in certain time intervals.
    */

    [SerializeField] GameObject prefab;

    [SerializeField]
    private float startingPosX;
    [SerializeField]
    private float startingPosY;

    [SerializeField]
    private int amountX;
    [SerializeField]
    private int amountY;

    [SerializeField]
    private float pivotX;

    [SerializeField]
    private float pivotY;

    [SerializeField]
    private float pointChangeCooldown;

    [SerializeField]
    private float padding;

    private float timeAfterLastChange;

    [SerializeField] private float population;

    [SerializeField]
    private List<Color> colors;


    private List<int> activePointIndexes;

    private List<int> inactivePointIndexes;

    private List<GameObject> allPoints;

    private int pointCount;

    [SerializeField]
    private int difficulty;

    [SerializeField]
    private int maxDifficulty;

    private bool gameIsReady;

    [SerializeField]
    private LayerMask collidersMask;

    [SerializeField]
    private int pointsChangeCount;

    // Start is called before the first frame update
    void Start()
    {
        gameIsReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("red: " + gameIsReady);
        if (gameIsReady)
        {
                Debug.Log("inss3");
            timeAfterLastChange += Time.deltaTime;
            if (timeAfterLastChange > pointChangeCooldown)
            {
                Debug.Log("inss1");
                ChangePoints(pointsChangeCount);
                timeAfterLastChange = 0;
            }
        }
        
    }

    public void GeneratePoints()
    {
        // startingPosX = 8f;
        // startingPosY = 4.7f;
        // amountX = 25;
        // amountY = 18;
        activePointIndexes = new List<int>();
        inactivePointIndexes = new List<int>();
        allPoints = new List<GameObject>();
        colors = new List<Color>();
        pointCount = amountX * amountY;
        // activeCount = pointCount / maxDifficulty * difficulty;
        // inactiveCount = pointCount - activeCount;
        float paddingX = startingPosX * 2 / amountX;
        float paddingY = startingPosY * 2 / amountY;
        colors.Add(Color.blue);
        colors.Add(Color.red);
        colors.Add(Color.green);
        Debug.Log("padding x : " + paddingX);
        Debug.Log("padding x : " + paddingY);
        for (int i = 0; i < amountX; i++)
        {
            for (int x = 0; x < amountY; x++)
            {
                Vector3 currentPos = new Vector3(pivotX + startingPosX - paddingX * i, pivotY + startingPosY - paddingY * x, 0);
                if (CheckPosition(currentPos))
                {
                    GameObject currentPoint = Instantiate(prefab, currentPos, Quaternion.identity);
                    currentPoint.GetComponent<SpriteRenderer>().color = colors[(int) Random.Range(0,3)];
                    currentPoint.SetActive(false);
                    allPoints.Add(currentPoint);
                }
            }
        }

        for (int i = 0; i < (int) (pointCount / maxDifficulty * difficulty * population); i++)
        {
            int randomIndex = Random.Range(0, allPoints.Count);
            while (allPoints[randomIndex].activeSelf)
            {
                randomIndex = Random.Range(0, allPoints.Count);
            }

            GameObject pointToActivate = allPoints[randomIndex];
            pointToActivate.GetComponent<SpriteRenderer>().color = colors[(int) Random.Range(0,3)];
            pointToActivate.SetActive(true);

        }

        GetComponent<ClickController>().enabled = true;
        GetComponent<RectangleController>().enabled = true;

        Debug.Log("inss2");
        gameIsReady = true;


    }

    bool CheckPosition(Vector3 currentPos)
    {
        Vector3 rayIncrementX = new Vector3(padding, 0, 0);
        Vector3 rayIncrementY = new Vector3(0, padding, 0);
        int rayCountSide = 25;

        for (int x = 0; x < rayCountSide; x++)
        {
            for (int y = 0; y < rayCountSide; y++)
            {
                Vector3 shootPosition = currentPos + rayIncrementX * (x - rayCountSide / 2)
                                                   + rayIncrementY * (y - rayCountSide / 2);
                RaycastHit2D hit = Physics2D.Raycast(shootPosition, new Vector3(0, 0, 1f), 100f, collidersMask);
                if (hit)
                {
                    return false;
                }

            }
        }
        return true;

    }

    void ChangePoints(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, allPoints.Count);
            int randomIndexActive = -1;
            int randomIndexInactive = -1;
            int trials = 0;
            if (allPoints[randomIndex].activeSelf)
            {
                randomIndexActive = randomIndex;
                randomIndex = Random.Range(0, allPoints.Count);
                while (allPoints[randomIndex].activeSelf && trials < 10000)
                {
                    trials += 1;
                    randomIndex = Random.Range(0, allPoints.Count);
                }
                randomIndexInactive = randomIndex;
            }
            else
            {
                randomIndexInactive = randomIndex;
                randomIndex = Random.Range(0, allPoints.Count);
                trials = 0;
                while (!allPoints[randomIndex].activeSelf && trials < 10000)
                {
                    trials += 1;
                    randomIndex = Random.Range(0, allPoints.Count);
                }
                randomIndexActive = randomIndex;
            }

            allPoints[randomIndexActive].SetActive(false);
            allPoints[randomIndexInactive].GetComponent<SpriteRenderer>().color = colors[(int) Random.Range(0,colors.Count)];
            allPoints[randomIndexInactive].SetActive(true);

        }
    }

    public void IncreasePointCount()
    {
        Debug.Log("inrease");
        int randomIndex = (int) Random.Range(0, allPoints.Count);
        int trials = 0;
        while (allPoints[randomIndex].activeSelf && trials < 1000)
        {
            trials += 1;
            randomIndex = (int) Random.Range(0, allPoints.Count);
        }
        allPoints[randomIndex].SetActive(true);
    }

    public void SetPointsChangeCount(int count)
    {
        pointsChangeCount = count;
    }

    public void SetPointsChangeCooldown(float cooldown)
    {
        pointChangeCooldown = cooldown; 
    }

    public void SetColors(List<Color> colors)
    {
        this.colors = new List<Color>(colors);
    }

    public bool GetGameIsReady()
    {
        return gameIsReady;
    }

}
