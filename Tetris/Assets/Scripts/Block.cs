using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int[,] shape{private set;get;} //形状快照
    public enum blockType{I,J,L,O,Z,S,T}
    public blockType type;
    public Color blockColor;
    public int blockState = 0;
    private Vector2 curPos = new Vector2(Mathf.Floor(Map.mapCol/2)-2,Map.mapRow-1); //方块当前位置

    void Awake() {
        InitShape();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //根据Block类型来实例化形状快照
    void InitShape()
    {
        switch (type)
        {
            case blockType.I:
                shape = new int[4,16]{
                    {0,1,0,0,
                     0,1,0,0,
                     0,1,0,0,
                     0,1,0,0},
                    {0,0,0,0,
                     1,1,1,1,
                     0,0,0,0,
                     0,0,0,0},
                    {0,1,0,0,
                     0,1,0,0,
                     0,1,0,0,
                     0,1,0,0},
                    {0,0,0,0,
                     1,1,1,1,
                     0,0,0,0,
                     0,0,0,0}
                };
                break;
            case blockType.J:
                shape = new int[4,16]{
                    {0,1,0,0,
                     0,1,0,0,
                     1,1,0,0,
                     0,0,0,0},
                    {1,0,0,0,
                     1,1,1,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,1,0,0,
                     1,0,0,0,
                     1,0,0,0,
                     0,0,0,0},
                    {1,1,1,0,
                     1,0,0,0,
                     0,0,0,0,
                     0,0,0,0}
                };
                break;
            case blockType.L:
                shape = new int[4,16]{
                    {1,0,0,0,
                     1,0,0,0,
                     1,1,0,0,
                     0,0,0,0},
                    {1,1,1,0,
                     1,0,0,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,1,0,0,
                     0,1,0,0,
                     0,1,0,0,
                     0,0,0,0},
                    {0,0,1,0,
                     1,1,1,0,
                     0,0,0,0,
                     0,0,0,0}
                };
                break;
            case blockType.O:
                shape = new int[4,16]{
                    {1,1,0,0,
                     1,1,0,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,1,0,0,
                     1,1,0,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,1,0,0,
                     1,1,0,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,1,0,0,
                     1,1,0,0,
                     0,0,0,0,
                     0,0,0,0}
                };
                break;
            case blockType.Z:
                shape = new int[4,16]{
                    {1,1,0,0,
                     0,1,1,0,
                     0,0,0,0,
                     0,0,0,0},
                    {0,1,0,0,
                     1,1,0,0,
                     1,0,0,0,
                     0,0,0,0},
                    {1,1,0,0,
                     0,1,1,0,
                     0,0,0,0,
                     0,0,0,0},
                    {0,1,0,0,
                     1,1,0,0,
                     1,0,0,0,
                     0,0,0,0}
                };
                break;
            case blockType.S:
                shape = new int[4,16]{
                    {1,0,0,0,
                     1,1,0,0,
                     0,1,0,0,
                     0,0,0,0},
                    {0,1,1,0,
                     1,1,0,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,0,0,0,
                     1,1,0,0,
                     0,1,0,0,
                     0,0,0,0},
                    {0,1,1,0,
                     1,1,0,0,
                     0,0,0,0,
                     0,0,0,0}
                };
                break;
            case blockType.T:
                shape = new int[4,16]{
                    {0,1,0,0,
                     1,1,1,0,
                     0,0,0,0,
                     0,0,0,0},
                    {1,0,0,0,
                     1,1,0,0,
                     1,0,0,0,
                     0,0,0,0},
                    {1,1,1,0,
                     0,1,0,0,
                     0,0,0,0,
                     0,0,0,0},
                    {0,1,0,0,
                     1,1,0,0,
                     0,1,0,0,
                     0,0,0,0}
                };
                break;
        }
    }

    //获取当前block位置
    public Vector2 GetCurBlockPos()
    {
        return curPos;
    }
    //获取当前block状态
    public int GetCurBlockState()
    {
        return blockState;
    }
    //方块左移
    public Vector2 MoveLeft()
    {
        curPos.x --;
        return curPos;
    }
    //方块右移
    public Vector2 MoveRight()
    {
        curPos.x ++;
        return curPos;
    }
    //方块下移
    public Vector2 MoveDown()
    {
        curPos.y --;
        return curPos;
    }
    //方块下落撞墙，返回上一状态
    public Vector2 InverseMoveDown()
    {
        curPos.y ++;
        return curPos;
    }
    //方块旋转
    public Vector2 RotateBlock()
    {
        blockState = (blockState + 1) % 4;
        return curPos;
    }
    //方块撞墙，返回旋转前的状态
    public Vector2 InverseRotateBlock()
    {
        blockState = (blockState + 3) % 4;
        return curPos;
    }
}
