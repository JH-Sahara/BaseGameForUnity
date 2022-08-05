using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Direction{Null=0,Up=-9,Down=9,Left=-1,Right=1}
    public Direction direction;
    public MapBuilder mapBuilder;
    private Vector3 playerAddPos;//角色移动的增量
    private bool canMove;//角色能否移动
    private bool canBoxMove;//箱子能否移动
    public GameController gameController;
    private Animator playerAni;
    // Start is called before the first frame update
    void Start()
    {
        InitPlayerController();
    }

    // Update is called once per frame
    void Update()
    {
        DetectDirection();
        DetectMove();
        Move();
    }

    //初始化
    private void InitPlayerController()
    {
        mapBuilder = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapBuilder>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerAni = gameObject.GetComponent<Animator>();
        canMove = false;
        canBoxMove = false;
    }

    //获取玩家键盘的输入
    private void DetectDirection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Direction.Up;
            playerAni.Play("PlayerUp");
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Direction.Left;
            playerAni.Play("PlayerLeft");
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Direction.Down;
            playerAni.Play("PlayerDown");
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Direction.Right;
            playerAni.Play("PlayerRight");
        }else
        {
            direction = Direction.Null;
        }
        //Debug.Log(direction);
    }

    //获取玩家移动方向上的Tile类型
    private void DetectMove()
    {
        switch (direction)
        {
            case Direction.Up:
                playerAddPos = Vector3.up;
                break;
            case Direction.Left:
                playerAddPos = Vector3.left;
                break;
            case Direction.Right:
                playerAddPos = Vector3.right;
                break;
            case Direction.Down:
                playerAddPos = Vector3.down;
                break;
            case Direction.Null:
                playerAddPos = Vector3.zero;
                break;
        }
    }

    //玩家移动函数
    private void Move()
    {
        switch (mapBuilder.GetTileType(transform.position,direction))
        {
            //角色移动的下一个位置为空
            case MapBuilder.TileType.Null:
                Debug.Log("前面为空");
                mapBuilder.SetTileType(transform.position,direction,MapBuilder.TileType.Player);
                if (mapBuilder.GetTileType(transform.position,Direction.Null) == MapBuilder.TileType.Player)
                {
                    mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Null);
                }else{
                    mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Point);
                }
                canMove = true;
                break;
            //角色移动的下一个位置为箱子
            case MapBuilder.TileType.Box:
            //角色移动的下一个位置为箱子，箱子在Point上
            case MapBuilder.TileType.BoxWithPoint:
                switch (mapBuilder.GetTileType(transform.position,(Direction)((int)direction*2)))
                {
                    //箱子的下一个位置为空
                    case MapBuilder.TileType.Null:
                        //先修改箱子和角色移动后的位置
                        mapBuilder.SetTileType(transform.position,(Direction)((int)direction*2),MapBuilder.TileType.Box);
                        if (mapBuilder.GetTileType(transform.position,direction) == MapBuilder.TileType.Box)
                        {
                            mapBuilder.SetTileType(transform.position,direction,MapBuilder.TileType.Player);
                        }else{
                            mapBuilder.SetTileType(transform.position,direction,MapBuilder.TileType.PlayerWithPoint);
                        }
                        //再修改角色原先所在的位置
                        if (mapBuilder.GetTileType(transform.position,Direction.Null) == MapBuilder.TileType.Player)
                        {
                            mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Null);
                        }else{
                            mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Point);
                        }
                        canBoxMove = true;
                        canMove = true;
                        break;
                    //箱子的下一个位置为Point
                    case MapBuilder.TileType.Point:
                        //先修改箱子和角色的位置
                        mapBuilder.SetTileType(transform.position,(Direction)((int)direction*2),MapBuilder.TileType.BoxWithPoint);
                        if (mapBuilder.GetTileType(transform.position,direction) == MapBuilder.TileType.Box)
                        {
                            mapBuilder.SetTileType(transform.position,direction,MapBuilder.TileType.Player);
                        }else{
                            mapBuilder.SetTileType(transform.position,direction,MapBuilder.TileType.PlayerWithPoint);
                        }
                        if (mapBuilder.GetTileType(transform.position,Direction.Null) == MapBuilder.TileType.Player)
                        {
                            mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Null);
                        }else{
                            mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Point);
                        }
                        canBoxMove = true;
                        canMove = true;
                        break;
                    default:
                        canBoxMove = false;
                        canMove = false;
                        break;
                }
                break;
            //角色移动的下一个位置为Point
            case MapBuilder.TileType.Point:
                mapBuilder.SetTileType(transform.position,direction,MapBuilder.TileType.PlayerWithPoint);
                if (mapBuilder.GetTileType(transform.position,Direction.Null) == MapBuilder.TileType.Player)
                {
                    mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Null);
                }else{
                    mapBuilder.SetTileType(transform.position,Direction.Null,MapBuilder.TileType.Point);
                }
                canMove = true;
                break;
            default:
                canBoxMove = false;
                canMove = false;
                break;
        }
        //判断游戏是否结束，如果结束的话阻止角色移动
        if (gameController.isGameOver == true)
        {
            return;
        }

        if (canBoxMove == true)
        {
            mapBuilder.SetBoxToMove(transform.position,direction,playerAddPos);
        }
        if (canMove == true)
        {
            transform.position += playerAddPos;
            gameController.AddStep();
        }
    }
}
