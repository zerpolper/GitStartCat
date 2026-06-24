using System;
using UnityEngine;

public enum PlayerState
{
    NormalMove,
    StopMove,
}
public enum PlayerMoveDirect
{
    Forward,
    Back,
    Left,
    Right,
}
public class PlayerMove : MonoBehaviour
{
    [Header("玩家运动方向")]
    [SerializeField]
    private PlayerMoveDirect playerMoveDirect = PlayerMoveDirect.Forward;

    [Header("玩家运动状态")]
    [SerializeField]
    private PlayerState playerState = PlayerState.NormalMove;

    [Space]
    [Header("玩家移动速度")]
    public float Speed = 1.0f;

    [Header("玩家移动间隔")]
    public float PlayerMoveInterval = 0.1f;

    private GameObject player = null;
    public GameObject Star;

    private bool isPlayerInput = false;
    private bool isMoveInterval = false;

    public LayerMask layerMask;//检测层
    public Vector3 boxCenter = new Vector3(0, -1f, 0);

    [SerializeField]
    private float currentInterTime = 0;

    private void Start()
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

        player.transform.rotation = Quaternion.Euler(Vector3.zero);
        //Debug.Log("数值长度"+isWithBlock.Length);
    }
    void Update()
    {
        if (Star.transform.childCount == 0)
        {
            playerState = PlayerState.StopMove;
            Debug.Log("完成通过条件");
        }

        isPlayerInput = PlayerAction.MoveInput(ref playerMoveDirect, playerState);
        isMoveInterval = PlayerAction.MoveInterval(ref currentInterTime, PlayerMoveInterval);


        if (isPlayerInput == true && isMoveInterval == true)
        {
            PlayerAction.PlayerMove(playerMoveDirect, playerState, Speed, player, layerMask);
            currentInterTime = 0;
            isPlayerInput = false;
            isMoveInterval = false;

        }
    }

    void OnDrawGizmos()
    {
        // 计算和检测时一模一样的中心点和半长宽高
        float detectDistance = 1f;
        Vector3 center = transform.position + transform.forward * detectDistance + boxCenter;
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);

        Gizmos.color = Color.green;
        // 绘制线框盒：中心点、半长宽高、旋转（必须和检测时的旋转一致）
        Gizmos.DrawWireCube(center, halfExtents * 2);

        float detectUpDistance = 1f;
        Vector3 Upcenter = transform.position + transform.forward * detectUpDistance;
        Vector3 UphalfExtents = new Vector3(0.25f, 0.25f, 0.25f);

        Gizmos.color = Color.green;
        // 绘制线框盒：中心点、半长宽高、旋转（必须和检测时的旋转一致）
        Gizmos.DrawWireCube(Upcenter, UphalfExtents * 2);
    }
}