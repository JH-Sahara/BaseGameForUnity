using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Block[] blocks; //方块的所有可能形状
    private Block curBlock; //当前block
    private Block nextBlock; //下一个方块
    private bool isFirstCreateBlock = true; //是否为第一次创建方块
    public Map mapSnapShot;
    private bool isGameOver = false;
    public float timeSpeed = 1f;
    private float timeCount = 0f;
    public GameObject blockFrame;
    private GameObject[,] nextBlocks;
    // Start is called before the first frame update
    void Start()
    {
        CreateBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            MoveBlock();
        }
    }

    //创建方块形状
    void CreateBlock()
    {
        if (isFirstCreateBlock)
        {
            //创建当前block
            int indexCur = Random.Range(0,blocks.Length);
            curBlock = Instantiate(blocks[indexCur]);
            isFirstCreateBlock = false;

            //创建下一个block
            int indexNext = Random.Range(0,blocks.Length);
            nextBlock = Instantiate(blocks[indexNext]);
        }else{
            Destroy(curBlock.gameObject);
            curBlock = nextBlock;
            nextBlock = Instantiate(blocks[Random.Range(0,blocks.Length)]);
        }
        //Debug.Log(curBlock.name + ":" + nextBlock.name);
        ShowNextBlock();
        SetBlockStateToMapSnapShot();
        mapSnapShot.ShowMapInfoText();
    }

    //方块的移动
    void MoveBlock()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //左移一个单位
            CurBlockMoveLeft();
        }else if (Input.GetKeyDown(KeyCode.D)){
            //右移一个单位
            CurBlockMoveRight();
        }else if (Input.GetKeyDown(KeyCode.W)){
            //旋转
            CurBlockRotate();
        }else if (Input.GetKeyDown(KeyCode.S)){
            //加速下降
            CurBlockMoveDown();
        }

        if (TimeCount())
        {
            CurBlockMoveDown();
        }
    }

    //判断移动后的block是否撞墙
    bool CanMoveBlock(Vector2 curPos)
    {
        for (int row=0; row<4; row++)
        {
            for (int col=0; col<4; col++)
            {
                if (curBlock.shape[curBlock.GetCurBlockState(),row*4+col] != 0)
                {
                    if (HitWall(row,col) || HitOtherBlock(row,col))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    //是否变为触底固定状态
    void SetChangeFixedBlock()
    {
        Vector2 pos = curBlock.GetCurBlockPos();
        for (int row=0; row<4; row++)
        {
            for (int col=0;col<4;col++)
            {
                if (curBlock.shape[curBlock.GetCurBlockState(),row*4+col] != 0)
                {
                    mapSnapShot.SetFixedMapInfo((int)pos.y-row,(int)pos.x+col);
                }
            }
        }
        mapSnapShot.ShowMapInfoText();
    }

    //左移函数
    void CurBlockMoveLeft()
    {
        //向左移动一个单位
        Vector2 curPos = curBlock.MoveLeft();
        //判断是否撞墙、撞到其他固定方块
        if (!CanMoveBlock(curPos))
        {
            curBlock.MoveRight(); //撞墙返回上一状态
        }
        //刷新地图快照 1.清空快照 2.设置新状态到快照中
        mapSnapShot.ClearSnapShot();
        SetBlockStateToMapSnapShot();
        mapSnapShot.ShowMapInfoText();
    }
    //右移函数
    void CurBlockMoveRight()
    {
        Vector2 curPos = curBlock.MoveRight();
        if (!CanMoveBlock(curPos))
        {
            curBlock.MoveLeft();
        }
        mapSnapShot.ClearSnapShot();
        SetBlockStateToMapSnapShot();
        mapSnapShot.ShowMapInfoText();
    }
    //下移函数
    void CurBlockMoveDown()
    {
        Vector2 curPos = curBlock.MoveDown();
        if (!CanMoveBlock(curPos))
        {
            curBlock.InverseMoveDown();
            //碰到底部或者其他方块，变为固定的方块
            SetChangeFixedBlock();
            //消行操作
            DisappearLine();
            //判断是否结束游戏
            if (GameOver())
            {
                Debug.Log("game over!");
                return;
            }
            CreateBlock();
            return;
        }
        mapSnapShot.ClearSnapShot();
        SetBlockStateToMapSnapShot();
        mapSnapShot.ShowMapInfoText();
    }
    //旋转函数
    void CurBlockRotate()
    {
        Vector2 curPos = curBlock.RotateBlock();
        if (!CanMoveBlock(curPos))
        {
            curBlock.InverseRotateBlock();
        }
        mapSnapShot.ClearSnapShot();
        SetBlockStateToMapSnapShot();
        mapSnapShot.ShowMapInfoText();
    }

    //设置方块的状态到新的地图快照中
    public void SetBlockStateToMapSnapShot()
    {
        Vector2 pos = curBlock.GetCurBlockPos();
        for (int row=0; row<4; row++)
        {
            for (int col=0; col<4; col++)
            {
                if (curBlock.shape[curBlock.GetCurBlockState(),row*4+col] != 0)
                {
                    mapSnapShot.SetMapInfo((int)pos.y-row,(int)pos.x+col,curBlock.blockColor);
                }
            }
        }
    }

    //撞到左右墙、底部墙
    bool HitWall(int row,int col)
    {
        Vector2 pos = curBlock.GetCurBlockPos();
        if (mapSnapShot.GetMapInfo((int)pos.y-row,(int)pos.x+col) == -1)
        {
            return true;
        }
        return false;
    }

    //撞到其他固定的方块
    bool HitOtherBlock(int row,int col)
    {
        Vector2 pos = curBlock.GetCurBlockPos();
        if (mapSnapShot.GetMapInfo((int)pos.y-row,(int)pos.x+col) == 8)
        {
            return true;
        }
        return false;
    }

    //判断是否有需要被消行的方块
    void DisappearLine()
    {
        int count = 0;
        for (int row=1; row<=Map.mapRow-1; row++)
        {
            count = 0;
            for (int col=1; col<=Map.mapCol-2; col++)
            {
                if (mapSnapShot.GetMapInfo(row,col) == 8)
                {
                    count ++;
                }
            }

            //如果一行中8的个数为mapCol-2,则为满行
            if (count == Map.mapCol-2)
            {
                mapSnapShot.DeleteLine(row);
                row --;
            }
        }
    }

    //计时器功能
    private bool TimeCount()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= timeSpeed)
        {
            timeCount = 0f;
            return true;
        }

        return false;
    }

    //显示下一个出现的方块
    private void ShowNextBlock()
    {
        if (nextBlocks == null)
        {
            nextBlocks = new GameObject[4,4];
        }

        //循环清空
        for (int i=0; i<4; i++)
        {
            for (int j=0; j<4; j++)
            {
                if (nextBlocks[i,j] == null)
                {
                    nextBlocks[i,j] = Instantiate(blockFrame,new Vector2(15+j,15-i),Quaternion.identity);
                }
                //清空边框颜色
                nextBlocks[i,j].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                //判断nextblock该位置是否有值，有值则赋值颜色
                if (nextBlock.shape[nextBlock.GetCurBlockState(),i*4+j] != 0)
                {
                    nextBlocks[i,j].GetComponent<SpriteRenderer>().color = nextBlock.blockColor;
                }
            }
        }
    }

    //判断是否结束游戏
    bool GameOver()
    {
        int count = 0;
        for (int j=1; j<=Map.mapCol-2; j++)
        {
            if (mapSnapShot.GetMapInfo(Map.mapRow-1,j) == 8)
            {
                count ++;
            }
        }

        //当最顶层出现固定的8时，即为游戏结束
        if (count > 0)
        {
            isGameOver = true;
            return true;
        }

        return false;
    }
}
