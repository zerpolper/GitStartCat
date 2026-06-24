using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private GameObject player = null;
    public GameObject Star;
    private GameObject [] childStars;
    //private GameObject destroyChild;

    public bool bonk = false;

    [SerializeField] private int childCount = 0;
    [SerializeField] private int collectCount = 0;
    void Start()
    {
        if (player == null)
        {
            if (this.gameObject.name == "Player" || this.gameObject.tag == "Player")
            {
                player = this.gameObject;
            }
            else
            {
                player = GameObject.Find("Player");
            }
        }

        if (Star == null)
        {
            Star = GameObject.Find("Star");
        }

        // 先获取父物体的子物体数量
        childCount = Star.transform.childCount;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            //bonk = true;
            if (childCount >= 0)
            {
                collectCount += 1;
                GameObject destroyChild = other.gameObject;
                Debug.Log("已收集" + destroyChild.name + ";" + collectCount + "/" + childCount);
                Destroy(destroyChild);
            }
        }
    }
}
