using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour
{

    /*
        This scripts handles collision of rectangles with points or other rectangles, it also handles it collapsing mechanic
    */
    
    private bool isMoving;
    private bool isColliding;
    private float collapseSpeed;

    private Vector3 scaleSaved;

    public bool chosen;

    private Vector3 colliderPos;

    private List<Color> colors;

    [SerializeField]
    private PointController pointController;


    // Start is called before the first frame update
    void Start()
    {
        colliderPos = Vector3.zero;
        isMoving = false;
        isColliding = false;
        colors = new List<Color>();
        colors.Add(Color.blue);
        colors.Add(Color.red);
        colors.Add(Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        if (chosen)
         Debug.Log("colliding: " + isColliding);
        if (isMoving)
        {
            transform.localScale -= new Vector3(collapseSpeed, collapseSpeed, 0);
            Debug.Log(transform.localScale.magnitude);
            if (transform.localScale.x <= 0.01f
                || transform.localScale.y <= 0.01f)
            {
                isMoving = false;

                List<Color> temp = new List<Color>(colors);
                SpriteRenderer thisSprite = GetComponent<SpriteRenderer>();
                temp.Remove(thisSprite.color);
                thisSprite.color = temp[(int) Random.Range(0, temp.Count)];
                gameObject.transform.localScale = scaleSaved;
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (isMoving)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Points"))
            {
                other.gameObject.SetActive(false);
                pointController.IncreasePointCount();
            }
            else
            {
                SpriteRenderer otherSprite = other.gameObject.transform.parent.GetComponent<SpriteRenderer>();
                SpriteRenderer thisSprite = GetComponent<SpriteRenderer>();
                List<Color> temp = new List<Color>(colors);
                temp.Remove(otherSprite.color);
                if (otherSprite.color != thisSprite.color)
                    temp.Remove(thisSprite.color);
                otherSprite.color = thisSprite.color;
                isMoving = false;

                int random = (int) Random.Range(0, temp.Count); 
                thisSprite.color = temp[random];
                gameObject.transform.localScale = scaleSaved;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        colliderPos = other.transform.position;
        isColliding = true;    
    }

    public void Collapse(float collapseSpeed)
    {
        this.collapseSpeed = collapseSpeed;
        scaleSaved = transform.localScale;
        isMoving = true;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    public void SetIsColliding(bool isColliding)
    {
        this.isColliding = isColliding;
    }

    public Vector3 GetColliderPos()
    {
        return colliderPos;
    }

    public void SetColors(List<Color> colors)
    {
        this.colors = new List<Color>(colors);
    }

    public void RandomizeColor()
    {
        GetComponent<SpriteRenderer>().color = colors[(int) Random.Range(0, colors.Count)];
    }

}
