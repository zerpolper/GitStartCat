using UnityEngine;


public class SmoothFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject  m_Camera;
    public Transform target;        // 要跟随的目标物体
    public float smoothSpeed = 0.5f;  // 平滑速度
    public Vector3 offset = new Vector3(2, 2, -2);  // 摄像机与目标的偏移量

    public float rotateDuration = 0.5f;  // 旋转持续时间
    private float currentYaw = 0f;      // 当前世界Y轴角度
    private bool isRotating = false;
    private bool isLookAt = false;
    private float targetYaw = 0f;
    private float rotateStartTime = 0f;
    private float startYaw = 0f;
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
    }
    void LateUpdate()
    {

        Vector3 desiredPosition = target.position + offset;
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
            targetYaw = currentYaw - 90f;
            StartRotate();
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            isLookAt = true;
            targetYaw = currentYaw + 90f;
            StartRotate();
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