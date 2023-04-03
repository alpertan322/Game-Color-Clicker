using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerHit;

    private GameController gameController;
    private PointController pointController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        pointController = GetComponent<PointController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit1 = Physics2D.Raycast(ray.origin, ray.direction,100f, layerHit);
            if (hit1)
            {
                Debug.Log(hit1.collider.gameObject.name);
                hit1.collider.gameObject.SetActive(false);
                Debug.Log(ray.direction);
                RaycastHit2D hit2 = Physics2D.Raycast(ray.origin, ray.direction, 100f);
                RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 100f);
                if (hits.Length > 0)
                {
                    Vector3 curSize = hits[0].collider.gameObject.transform.localScale;
                    float minArea = curSize.x * curSize.y;
                    int minIndex = 0;
                    for (int i = 1; i < hits.Length; i++)
                    {
                        curSize = hits[i].collider.gameObject.transform.localScale;
                        float curArea = curSize.x * curSize.y;
                        if (curArea < minArea)
                        {
                            minArea = curArea;
                            minIndex = i;
                        }
                    }
                    if (hit1.collider.GetComponent<SpriteRenderer>().color == 
                        hits[minIndex].collider.GetComponent<SpriteRenderer>().color)
                    {
                        gameController.ScoreUp();
                    }
                    else
                    {
                        gameController.LifeDown();
                    }
                }
            }
        }
    }
}
