using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public GameObject playerParfab;
    public GameObject WallParfab;
    public GameObject BoxParfab;
    public GameObject PointParfab;
    public int row = 9;
    public int col = 9;
    public int[] map;
    public GameObject[] boxMap;
    public enum TileType{Null=0,Wall=1,Player=2,Box=3,Point=9,PlayerWithPoint=10,BoxWithPoint=11}
    public GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        InitMapBuilder();
        InitMap();
        CreateMap();
    }

    //初始化函数
    private void InitMapBuilder()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    //初始化地图数组
    private void InitMap()
    {
        map = new int[]  
        {
            1,1,1,1,1,0,0,0,0,
            1,2,0,0,1,0,0,0,0,
            1,0,3,0,1,0,1,1,1,
            1,0,3,0,1,0,1,9,1,
            1,1,1,3,1,1,1,9,1,
            0,1,1,0,0,0,0,9,1,
            0,1,0,0,0,1,0,0,1,
            0,1,0,0,0,1,1,1,1,
            0,1,1,1,1,1,0,0,0
        };
    }

    //根据map数组创建地图
    private void CreateMap()
    {
        //实例化boxMap数组
        boxMap = new GameObject[row*col];
        Debug.Log("boxMap is Length"+boxMap.Length);
        //创建游戏物体
        for (int i=0; i<row; i++)
        {
            for (int j=0; j<col; j++)
            {
                switch(map[i*col+j])
                {
                    case (int)TileType.Null:
                        break;
                    case (int)TileType.Wall:
                        CreateParfabs(WallParfab,new Vector3(j+1,row-i,0));
                        break;
                    case (int)TileType.Player:
                        CreateParfabs(playerParfab,new Vector3(j+1,row-i,0));
                        break;
                    case (int)TileType.Box:
                        boxMap[i*col+j] = CreateParfabs(BoxParfab,new Vector3(j+1,row-i,0));
                        boxMap[i*col+j].GetComponent<BoxRender>().boxStat = map[i*col+j];
                        break;
                    case (int)TileType.Point:
                        CreateParfabs(PointParfab,new Vector3(j+1,row-i,0));
                        gameController.AddAllPoint();
                        //Debug.Log("addAllPoint增加");
                        break;
                }
            }
        }
    }

    //根据参数实例化不同的游戏物体
    private GameObject CreateParfabs(GameObject Parfab,Vector3 pos)
    {
        GameObject temp = Instantiate(Parfab,pos,Quaternion.identity);
        temp.transform.SetParent(GameObject.FindGameObjectWithTag("Map").transform,false);
        return temp;
    }

    //获取Tile类型
    public TileType GetTileType(Vector2 pos,PlayerController.Direction direction)
    {
        float x = row - pos.y;
        float y = pos.x - 1;
        int temp = map[(int)(x * col + y + (int)direction)];
        return (TileType)temp;
    }

    //设置移动位置上的Tile类型
    public void SetTileType(Vector2 pos,PlayerController.Direction direction,TileType tileType)
    {
        float x = row - pos.y;
        float y = pos.x - 1;
        map[(int)(x * col + y +(int)direction)] = (int)tileType;
    }

    //修改移动后的Box位置
    public void SetBoxToMove(Vector2 pos,PlayerController.Direction direction,Vector3 boxNextPos)
    {
        float x = row - pos.y;
        float y = pos.x - 1;
        GameObject temp = boxMap[(int)(x*col+y+(int)direction)];
        if (temp == null)
        {
            Debug.Log("查找Box出现错误");
        }else{
            temp.transform.position += boxNextPos;
            temp.GetComponent<BoxRender>().boxStat = map[(int)(x*col+y+(int)direction*2)];
            boxMap[(int)(x*col+y+(int)direction)] = null;
            boxMap[(int)(x*col+y+(int)direction*2)] = temp;
        }
    }
}
