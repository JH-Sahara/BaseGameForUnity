using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static int mapRow = 20;
    public static int mapCol = 12;
    public GameObject wall; //墙
    public GameObject blockFrame; //空位置
    private int[,] mapSnapShot; //地图快照
    private GameObject[,] mapObjs;
    public GameObject infoText; //3D文字预制体
    public GameObject infoRoot; //3D文字节点
    private TextMesh[,] mapInfoTexts; //显示地图快照
    void Awake()
    {
        InitMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowMapInfoText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化地图快照以及实例化地图数组
    void InitMap()
    {
        mapSnapShot = new int[mapRow,mapCol];
        mapObjs = new GameObject[mapRow,mapCol];

        //循环初始化数组
        for (int i=0; i<mapRow; i++)
        {
            for (int j=0; j<mapCol; j++)
            {
                if (i==0 || j==0 || j==mapCol-1)
                {
                    mapSnapShot[i,j] = -1;
                    //创建Vector2时，i和j对调是因为行列和GameObject的坐标位置是相反的
                    mapObjs[i,j] = Instantiate(wall,new Vector2(j,i),Quaternion.identity);
                    mapObjs[i,j].name = "Wall" + i + "_" + j;
                }else{
                    mapSnapShot[i,j] = 0;
                    mapObjs[i,j] = Instantiate(blockFrame,new Vector2(j,i),Quaternion.identity);
                    mapObjs[i,j].name = "BlockFrame" + i + "_" + j;
                }
                mapObjs[i,j].transform.SetParent(transform);
            }
        }
    }

    //清空地图快照
    public void ClearSnapShot()
    {
        for (int row=0; row<mapRow; row++)
        {
            for (int col=0; col<mapCol; col++)
            {
                if (mapSnapShot[row,col] != -1 && mapSnapShot[row,col] != 8)
                {
                    mapSnapShot[row,col] = 0;
                    mapObjs[row,col].GetComponent<SpriteRenderer>().color = new Color32(152,152,152,160);
                }
            }
        }
    }

    //设置地图信息
    public void SetMapInfo(int row,int col,Color color)
    {
        if (mapSnapShot == null)
        {
            Debug.Log("Map-SetMapInfo is null");
            return;
        }

        mapSnapShot[row,col] = 1;
        mapObjs[row,col].GetComponent<SpriteRenderer>().color = color;
    }

    //设置地图信息为固定信息
    public void SetFixedMapInfo(int row,int col)
    {
        Debug.Log("SetFixedMapInfo");
        mapSnapShot[row,col] = 8;
    }
    //获取地图快照信息
    public int GetMapInfo(int row,int col)
    {
        return mapSnapShot[row,col];
    }

    //给定行号，执行消行操作
    public void DeleteLine(int row)
    {
        for (int i=row; i<=mapRow-1; i++)
        {
            for (int j=1; j<=mapCol-2; j++)
            {
                if (i == mapRow-1)
                {
                    mapSnapShot[i,j] = 0;
                    mapObjs[i,j].GetComponent<SpriteRenderer>().color = new Color32(152,152,152,160);
                }else{
                    mapSnapShot[i,j] = mapSnapShot[i+1,j];
                    mapObjs[i,j].GetComponent<SpriteRenderer>().color = mapObjs[i+1,j].GetComponent<SpriteRenderer>().color;
                }
            }
        }
    }

    //将地图快照显示出来，方便调试
    public void ShowMapInfoText()
    {
        if (mapInfoTexts == null)
        {
            mapInfoTexts = new TextMesh[mapRow,mapCol];
            infoRoot = new GameObject("InfoRoot");
        }

        for (int i=0; i<mapRow; i++)
        {
            for (int j=0; j<mapCol; j++)
            {
                if (mapInfoTexts[i,j] == null)
                {
                    mapInfoTexts[i,j] = Instantiate(infoText,new Vector2(j,i),Quaternion.identity).GetComponent<TextMesh>();
                    mapInfoTexts[i,j].text = mapSnapShot[i,j].ToString();
                }else{
                    mapInfoTexts[i,j].text = mapSnapShot[i,j].ToString();
                }
                mapInfoTexts[i,j].transform.SetParent(infoRoot.transform);
            }
        }
    }
}
