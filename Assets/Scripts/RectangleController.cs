using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleController : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> rectangles;

    [SerializeField]
    private float rectangleCollapseCooldown;
    private float timeAfterLastCollapse;

    [SerializeField]
    private float collapseSpeed;

    [SerializeField]
    private GameObject movingRectangle;

    void Start()
    {
        timeAfterLastCollapse = 0;
    }

    void Update()
    {
        if (timeAfterLastCollapse > rectangleCollapseCooldown && (movingRectangle == null || !movingRectangle.GetComponent<Rectangle>().IsMoving()))
        {
            int trials = 0;
            timeAfterLastCollapse = 0;
            movingRectangle = rectangles[Random.Range(0, rectangles.Count)];
            while (!movingRectangle.activeSelf && trials < rectangles.Count)
            {
                trials += 1;
                movingRectangle = rectangles[Random.Range(0, rectangles.Count)];
            }
            movingRectangle.GetComponent<Rectangle>().Collapse(collapseSpeed / 1000);
        }
        timeAfterLastCollapse += Time.deltaTime;


    }

    public void SetCollapseSpeed(float collapseSpeed)
    {
        this.collapseSpeed = collapseSpeed;
    }

    public void SetCollapseCooldown(float collapseCooldown)
    {
        this.rectangleCollapseCooldown = collapseCooldown;
    }

    public void SetRectangleColors(List<Color> colors)
    {
        for (int i = 0; i < rectangles.Count; i++)
        {
            rectangles[i].GetComponent<Rectangle>().SetColors(colors);
        }
    }

    public void SetRectangles(List<GameObject> rectangles)
    {
        this.rectangles = new List<GameObject>(rectangles);
    }

    public void RandomizeColors()
    {
        for (int i = 0; i < rectangles.Count; i++)
        {
            rectangles[i].GetComponent<Rectangle>().RandomizeColor();
        }
    }

}
