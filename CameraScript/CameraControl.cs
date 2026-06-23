using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject  m_Camera;
    public Transform target;        // 要跟随的目标物体
    public float smoothSpeed = 0.5f;  // 平滑速度
    public Vector3 offset = Vector3.zero;  // 摄像机与目标的偏移量
    private Vector3 desiredPosition = Vector3.zero;
    public float rotateDuration = 0.5f;  // 旋转持续时间
    private float currentYaw = 0f;      // 当前世界Y轴角度
    private bool isRotating = false;
    private bool isLookAt = false; //只在旋转时注视

    private float targetYaw = 0f;
    private float rotateStartTime = 0f;
    private float startYaw = 0f;
    private float rotateAngle = 0f;

    private bool hasRatated = false;//是否发生旋转

    //不同方向的偏移量预设
   [Header("Camera Offset Presets")]
    public Vector3 forwardOffset = new Vector3(2, 2, -2);   // 前方视角
    public Vector3 leftOffset = new Vector3(2, 2, 2);       // 左方视角
    public Vector3 backOffset = new Vector3(-2, 2, 2);      // 后方视角
    public Vector3 rightOffset = new Vector3(-2, 2, -2);    // 右方视角
    //当前偏移量索引
    [SerializeField]
    private int currentOffsetIndex = 0;
    private Vector3[] offsetPresets;

    private void Start()
    {
        if (m_Camera == null)
        {
            m_Camera = GameObject.Find("MainCamera");
        }

        if (target == null)
        {
            if (this.gameObject.name == "Player" || this.gameObject.tag == "Player")
            {
                target = this.gameObject.transform;
            }
            else
            {
                target = GameObject.Find("Player").transform;
            }
        }

        offsetPresets = new Vector3[]
        {
            forwardOffset,   // 索引0: 前方
            leftOffset,      // 索引1: 左方
            backOffset,      // 索引2: 后方
            rightOffset,     // 索引3: 右方
        };

        offset = forwardOffset;
    }
    void LateUpdate()
    {
        if (hasRatated)
        {
            desiredPosition = target.position + offsetPresets[currentOffsetIndex];
        }
        else
        {
            desiredPosition = target.position + offset;
        }
        // 使用Lerp平滑移动
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        CameraRotate();
    }

    private void CameraRotate()
    {
        // 检测Q/E键
        if (Input.GetKeyDown(KeyCode.Q) && !isRotating)
        {
            isLookAt = true;
            // 向左旋转：切换到左方偏移
            currentOffsetIndex = (currentOffsetIndex + 1) % offsetPresets.Length;
            
            targetYaw = currentYaw - 90f;
            StartRotate();
            rotateAngle -= 90f;
            
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            isLookAt = true;
            // 向右旋转：切换到右方偏移
            currentOffsetIndex = (currentOffsetIndex - 1 + offsetPresets.Length) % offsetPresets.Length;

            targetYaw = currentYaw + 90f;
            StartRotate();
            rotateAngle += 90f;
        }
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            isLookAt = false;
            
            return;
        }

        // 平滑旋转
        if (isRotating)
        {
            float t = (Time.time - rotateStartTime) / rotateDuration;
            if (t >= 0.8f)
            {
                currentYaw = targetYaw;
                isRotating = false;

                hasRatated = true;
            }
            else
            {
                // 使用平滑曲线
                t = Mathf.SmoothStep(0, 1, t);
                currentYaw = Mathf.Lerp(startYaw, targetYaw, t);
            }
        }

        // 更新摄像机位置（世界坐标旋转）
        if(isLookAt)
        {
            UpdateCameraPosition();
        }

        if (rotateAngle >= 360f || rotateAngle <= -360)
        {
            rotateAngle = 0f;
        }
        PlayerAction.SetCameraangle(ref rotateAngle);
    }

    void StartRotate()
    {
        isRotating = true;
        rotateStartTime = Time.time;
        startYaw = currentYaw;
    }

    void UpdateCameraPosition()
    {
        // 围绕世界Y轴旋转偏移向量
        Quaternion worldRotation = Quaternion.Euler(0, currentYaw, 0);
        Vector3 rotatedOffset = worldRotation * offset;

        // 摄像机位置 = 目标位置 + 旋转后的偏移
        transform.position = target.position + rotatedOffset;

        // 摄像机始终看向目标
        transform.LookAt(target);
    }
}