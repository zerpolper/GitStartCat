using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 30f;

    public GameObject star;
    private Vector3 rotationDirection = new Vector3(0, 1, 0); // (0,1,0) 表示绕Y轴旋转

    void Update()
    {
        transform.Rotate(rotationSpeed * rotationDirection * Time.deltaTime, Space.Self);
    }
}
