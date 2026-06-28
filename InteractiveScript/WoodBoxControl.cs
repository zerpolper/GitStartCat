using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WoodBoxState
{
    Moving,
    Stop,
}


public class WoodBoxControl : MonoBehaviour
{
    private PlayerTrigger _playerTrigger;

    [Header("箱子移动速度")]
    [SerializeField]
    private float smoothSpeed = 5.0f;
    //private float startTime;
    //public float moveDuration = 40;

    //[SerializeField]
    //private float durationTime;

    public GameObject woodBox = null;
    [SerializeField]
    private bool isMove = false;
    [SerializeField]
    private bool hasMove = false;

    [SerializeField]
    private bool canMove = false;

    [SerializeField]
    private bool touchWater = false;

    private bool hasDownWater = false;

    private Vector3 downPoint;
    private Vector3 upPoint;
    private Vector3 orginPoint;
    float distanceDown = 0f;
    float distanceUp = 0f;




    public event Action woodBovMoving;
    public event Action boxTouchWater;
    // Start is called before the first frame update
    private void Awake()
    {
        _playerTrigger = FindObjectOfType<PlayerTrigger>();
        _playerTrigger.boxCollide += StartWoodBoxMove;
    }
    void Start()
    {
        if (woodBox == null)
        {
            if (this.gameObject.name == "woodBoxr" || this.gameObject.tag == "woodBox")
            {
                woodBox = this.gameObject;
            }
            else
            {
                woodBox = GameObject.Find("woodBox");
            }
        }
        orginPoint = woodBox.transform.position;
        downPoint = woodBox.transform.position - new Vector3(0, 2, 0);
        upPoint = orginPoint;
    }

    // Update is called once per frame
    void Update()
    {
        distanceDown = woodBox.transform.position.y - downPoint.y;
        distanceUp = woodBox.transform.position.y - upPoint.y;
        //Debug.Log("向下距离：" + distanceDown +"向上距离：" + distanceUp);

        toMove();

        if (isMove)
        {
           
            if (hasMove)
            {
                WoodBoxMoveDown();
            }
            else if (!hasMove)
            {
                WoodBoxMoveUp();
            }
        }
        
    }
    public void toMove()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((distanceUp > -1.9f && distanceUp < -0.2f) || (distanceDown > 0.2f && distanceDown < 1.9f))
            {
                Debug.Log("向上时");
                woodBovMoving?.Invoke();
                return;
            }
            
            if (!touchWater)
            {
                if (canMove && !isMove && !hasMove)
                {
                    isMove = true;
                    hasMove = true;
                }
            }
            if (canMove && isMove && hasMove)
            {
                isMove = true;
                hasMove = false;
            }

            if (touchWater)
            {
                if (!hasDownWater)
                {
                    boxTouchWater?.Invoke();
                }
            }
        }
    }

    public void UIToMove()
    {
        if ((distanceUp > -1.9f && distanceUp < -0.2f) || (distanceDown > 0.2f && distanceDown < 1.9f))
        {
            Debug.Log("向上时");
            woodBovMoving?.Invoke();
            return;
        }

        if (!touchWater)
        {
            if (canMove && !isMove && !hasMove)
            {
                isMove = true;
                hasMove = true;
            }
        }
        else if (canMove && isMove && hasMove)
        {
            isMove = true;
            hasMove = false;
        }

        if (touchWater)
        {
            if (!hasDownWater)
            {
                boxTouchWater?.Invoke();
            }
        }
    }

    public void WoodBoxMoveDown()
    {
        if (isMove)
        {
            float step = smoothSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(woodBox.transform.position, downPoint, step);
            hasDownWater = true;
        }

    }

    public void WoodBoxMoveUp()
    {
        float distance = Vector3.Distance(woodBox.transform.position, orginPoint);
        if (isMove)
        {
            float step = smoothSpeed * 2 * Time.deltaTime;
            transform.position = Vector3.Lerp(woodBox.transform.position, upPoint, step);
        }
        if (distance <= 0.1f)
        {
            isMove = false;
        }
    }

    private void StartWoodBoxMove(bool getmove)
    {
        canMove = getmove;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            touchWater = true;
            //Debug.Log("接触水了");
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            touchWater = false;
            //Debug.Log("离开水了");
            
        }
    }
}
