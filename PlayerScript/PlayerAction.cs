using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAction 
{
    // 存储摄像机的当前Y轴旋转角度
    private static float camerangle = 0f;

    public static void SetCameraangle( ref float angle)
    {
        camerangle = angle;
    }

    public static bool MoveInput(ref PlayerMoveDirect playerMoveDirect, PlayerState playerState)
    {
        if (playerState == PlayerState.NormalMove)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerMoveDirect = PlayerMoveDirect.Left;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                playerMoveDirect = PlayerMoveDirect.Back;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                playerMoveDirect = PlayerMoveDirect.Right;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                playerMoveDirect = PlayerMoveDirect.Forward;
                return true;
            }

        }
        return false;
    }

    /// <summary>
    /// <param name="playerMoveDirect"><移动方向/param>
    /// <param name="playerState">当前的移动状态</param>
    /// <param name="speed">移动速度</param>
    /// <param name="player">移动对象</param>
    /// <param name="isBlock">是否锁定移动</param>
    /// </summary>

    public static void PlayerMove(PlayerMoveDirect playerMoveDirect, PlayerState playerState, float speed, GameObject player, bool isBlock)
    {
        Vector3 playerMoveForward = Vector3.zero;
        Vector3 playerRotateForward = Vector3.zero;
        switch (playerMoveDirect)
        {
            case PlayerMoveDirect.Forward:
                if (camerangle == 0f)
                {
                    playerMoveForward = Vector3.forward;
                    playerRotateForward = new Vector3(0, 0, 0);
                }
                else
                {
                    playerMoveForward = Quaternion.Euler(0, camerangle, 0) * Vector3.forward;
                    playerRotateForward = new Vector3(0, camerangle, 0);
                }
                break;

            case PlayerMoveDirect.Left:
                if (camerangle == 0f)
                {
                    playerMoveForward = Vector3.left;
                    playerRotateForward = new Vector3(0, -90, 0);
                }
                else
                {
                    playerMoveForward = Quaternion.Euler(0, camerangle, 0) * Vector3.left;
                    playerRotateForward = new Vector3(0, camerangle - 90, 0);
                }
                
                break;

            case PlayerMoveDirect.Back:
                if (camerangle == 0f)
                {
                    playerMoveForward = Vector3.back;
                    playerRotateForward = new Vector3(0, -180, 0);
                }
                else
                {
                    playerMoveForward = Quaternion.Euler(0, camerangle, 0) * Vector3.back;
                    playerRotateForward = new Vector3(0, camerangle-180, 0);
                }
                break;

            case PlayerMoveDirect.Right:
                if (camerangle == 0f)
                {
                    playerMoveForward = Vector3.right;
                    playerRotateForward = new Vector3(0, -270, 0);
                }
                else
                {
                    playerMoveForward = Quaternion.Euler(0, camerangle, 0) * Vector3.right;
                    playerRotateForward = new Vector3(0, camerangle - 270, 0);
                }
                break;
        }

        if (playerState == PlayerState.NormalMove)
        {
            player.transform.rotation = Quaternion.Euler(playerRotateForward);

            if (!isBlock)
            {
                Move(player.transform, playerMoveForward, speed);
            }
        }
    }
    

    public static bool MoveInterval(ref float time, float moveIntervalTime)
    {
        time += Time.deltaTime;
        if (time >= moveIntervalTime)
        {
            return true;
        }
        return false;
    }
    private static void Move(Transform transform, Vector3 forwardDirect, float speed)
    {
        transform.Translate(forwardDirect * speed, Space.World);
    }
}
