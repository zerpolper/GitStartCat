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
public class SimpleMove : MonoBehaviour
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
    public float PlayerMoveInterval = 0.25f;
    //private Vector3 BasicDistance = new Vector3(0, 0, 0.8f);

    private GameObject player = null;

    private bool isPlayerInput = false;
    private bool isMoveInterval = false;
    //判断是否阻挡
    private bool isBlock = false;

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

        player.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    void Update()
    {
        isPlayerInput = PlayerAction.MoveInput(ref playerMoveDirect, playerState);
        isMoveInterval = PlayerAction.MoveInterval(ref currentInterTime, PlayerMoveInterval);
        if (isPlayerInput == true && isMoveInterval == true)
        {
            PlayerAction.PlayerMove(playerMoveDirect, playerState, Speed, player, isBlock);
            currentInterTime = 0;
            isPlayerInput = false;
            isMoveInterval = false;
        }
    }
}