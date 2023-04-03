using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> rectangles;

    [SerializeField]
    private Vector3 upperLeftCornerPos;

    [SerializeField]
    private float sideX;

    [SerializeField]
    private float sideY;

    [SerializeField]
    private int currentIndex;
    private bool initialPlacing = true;

    [SerializeField]
    private float cooldown;
    private float timeAfterLastChange;

    private bool generationFinished;

    [SerializeField]
    private Vector3 previousPos;

    private int trial;

    [SerializeField]
    private GameObject mainRectangle;

    private bool tryInside;

    [SerializeField]
    private float normalize;

    private Vector3 previousScale;


    [SerializeField]
    private float wiggleRoom;

    [SerializeField]
    private float initialWait;

    private float timeWaited;

    // Start is called before the first frame update
    void Start()
    {
        trial = 0;
        timeAfterLastChange = 0;     
        generationFinished = false;
        tryInside = false;
        initialPlacing = true;
        currentIndex = 0;
        initialPlacing = true;
        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int x = 0; x< 2; x++)
            {
                Vector3 size = rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size;
                rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size = new Vector3(size.x, 2 * size.y, 0);
            }
            for (int x = 2; x < 4; x++)
            {
                Vector3 size = rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size;
                rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size = new Vector3(2 * size.x,  size.y, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!generationFinished && timeWaited > initialWait)
        {
            if (timeAfterLastChange > cooldown)
            {
                timeAfterLastChange = 0;
                Rectangle rect = rectangles[currentIndex].GetComponent<Rectangle>();
                if (initialPlacing || rect.IsColliding())
                {
                    initialPlacing = false;
                    previousPos = new Vector3(sideX * Random.Range(0, sideX) / sideX,  -sideY * Random.Range(0, sideY) / sideY, 0);
                    previousScale = rect.transform.localScale;
                    //previousPos = previousPos * rect.transform.localScale.x * rect.transform.localScale.y * normalize;
                    previousPos += upperLeftCornerPos;
                    RandomizeCurRectangle();
                    rectangles[currentIndex].GetComponent<Rectangle>().SetIsColliding(false);
                    rectangles[currentIndex].GetComponent<Rectangle>().chosen = true;
                    rectangles[currentIndex].GetComponent<Rigidbody2D>().WakeUp();
                    rectangles[currentIndex].transform.position = previousPos;
                }
                else if (tryInside)
                {
                   // previousPos = CalculateNewPos(trial);
                    if (trial >= 3)
                    {
                        initialPlacing = true;
                        tryInside = false;
                        trial = 0;
                    }
                    else if (trial == 2 && !rect.IsColliding())
                    {
                        tryInside = false;
                        trial = 0;
                        return;
                    }
                    trial += 1;
                    RandomizeCurRectangle();
                  //  rect.transform.localScale = previousScale * 0.75f;
                    rectangles[currentIndex].GetComponent<Rectangle>().SetIsColliding(false);
                    rectangles[currentIndex].GetComponent<Rectangle>().chosen = true;
                    rectangles[currentIndex].GetComponent<Rigidbody2D>().WakeUp();
                    rectangles[currentIndex].transform.position = previousPos;
                }
                else if (!rect.IsColliding())
                {
                    tryInside = true;
                    initialPlacing = false;
                    timeAfterLastChange = 0;
                    trial = 0;
                    //Vector3 pos = upperLeftCornerPos + new Vector3(Random.Range(0, sideX) / sideX, Random.Range(0, sideY) / sideY, 0);
                    if (currentIndex >= rectangles.Count - 1)
                    {
                        generationFinished = true;

                        for (int i = 0; i < rectangles.Count; i++)
                        {
                            for (int x = 0; x < 2; x++)
                            {
                                Vector3 size = rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size;
                                rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size = new Vector3(size.x, 0.5f * size.y, 0);
                            }

                            for (int x = 2; x < 4; x++)
                            {
                                Vector3 size = rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size;
                                rectangles[i].transform.GetChild(x).GetComponent<BoxCollider2D>().size = new Vector3(0.5f * size.x,  size.y, 0);
                            }
                            rectangles[i].GetComponent<BoxCollider2D>().enabled = true;
                        }
                            mainRectangle.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    else
                    {
                        currentIndex += 1;
                    }
                }

            }
                timeAfterLastChange += Time.deltaTime;
        }
        timeWaited += Time.deltaTime;
    }

    private Vector3 CalculateNewPos(int trial)
    {
        Vector3 result = Vector3.zero;
        switch (trial)
        {
            case 0:
                result = previousPos - new Vector3(0, 1f, 0);
                break;
            case 1:
                result = previousPos + new Vector3(0, 1f, 0);
                break;
            case 2:
                result = previousPos + new Vector3(1f, 0, 0);
                break;
            case 3:
                result = previousPos - new Vector3(1f, 1, 0);
                break;
        }
        return result;
    }

    private void RandomizeCurRectangle()
    {
        GameObject rect = rectangles[currentIndex];
        rect.GetComponent<BoxCollider2D>().enabled = false;
        // Vector3 scale = new Vector3(rect.transform.localScale.x * Random.Range(0.8f, 1.2f),
                                    // rect.transform.localScale.y * Random.Range(0.8f, 1.2f), 0);
        int randomInt = (int) Random.Range(1, 180f);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        if (randomInt % 2 == 0)
        {
            rotation = Quaternion.Euler(0, 0, 90f);
        }
        // rect.transform.localScale = scale;
        rect.transform.rotation = rotation;
    }

    public bool GetGenerationFinished()
    {
        return generationFinished;
    }

}
