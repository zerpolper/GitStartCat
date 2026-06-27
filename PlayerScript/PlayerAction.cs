using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PlayerAction 
{
    // 存储摄像机的当前Y轴旋转角度
    private static float camerangle = 0f;

    private static bool isBlock = false;
    private static bool isUpBlock = false;
    public static float detectDistance = 1f; // 检测距离
    private static Vector3 boxCenter = new Vector3(0, -1f, 0);
    public static Vector3 boxSize = new Vector3(0.25f, 0.25f, 0.25f); // 检测盒大小

    public static void SetCameraangle( ref float angle)
    {
        camerangle = angle;
        //Debug.Log("角度：" + camerangle);
        
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

    public static void PlayerMove(PlayerMoveDirect playerMoveDirect, PlayerState playerState, float speed, GameObject player, LayerMask layerMask)
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

            isBlock = CheckForwardBlocked(playerMoveForward, player,layerMask);
            isUpBlock = CheckForwardUpBlocked(playerMoveForward, player, layerMask);
            Debug.Log("CanPass:" + isBlock);

            if (isBlock && !isUpBlock)
            {
                Move(player.transform, playerMoveForward, speed);
            }
        }
    }
    private static bool CheckForwardBlocked(Vector3 playerMoveForward ,GameObject player, LayerMask layerMask)
    {

        // 计算检测的中心点（在玩家前下方 detectDistance 处）
        Vector3 center = player.transform.position + playerMoveForward * detectDistance + boxCenter;

        // 进行盒体检测，不检测玩家自己的碰撞体（通过LayerMask过滤）
        Collider[] colliders = Physics.OverlapBox(center, boxSize / 2, player.transform.rotation, layerMask);
        Debug.Log("检测碰撞的个数：" + colliders.Length);
        // 如果检测到任何碰撞体，返回 true（表示被阻挡）
        return colliders.Length > 0;
    }
    private static bool CheckForwardUpBlocked(Vector3 playerMoveForward, GameObject player, LayerMask layerMask)
    {

        // 计算检测的中心点（在玩家前方 detectDistance 处）
        Vector3 center = player.transform.position + playerMoveForward * detectDistance;

        // 进行盒体检测，不检测玩家自己的碰撞体（通过LayerMask过滤）
        Collider[] colliders = Physics.OverlapBox(center, boxSize / 2, player.transform.rotation, layerMask);
        Debug.Log("检测碰撞的个数：" + colliders.Length);
        // 如果检测到任何碰撞体，返回 true（表示被阻挡）
        return colliders.Length > 0;
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
