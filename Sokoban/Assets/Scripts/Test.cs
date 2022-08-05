using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject textParfab;
    public MapBuilder mapBuilder;
    public Text[] mapText{get;set;}
    public int[] mapClone{get;set;}
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        MashMap();
    }

    //初始化
    private void Init()
    {
        mapBuilder = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapBuilder>();
    }

    //绘制数组地图
    private void MashMap()
    {
        // Debug.Log("mapClone == null:"+mapClone == null);
        // Debug.Log(mapClone.ToString());
        if (mapClone == null)
        {
            mapClone = mapBuilder.map.Clone() as int[];
            Debug.Log("mapClone"+mapClone.Length);
        }
        // Debug.Log("mapText == null:"+mapText == null);
        // Debug.Log(mapText.ToString());
        //如果测试数组没有实例化则实例化
        if (mapText == null)
        {
            mapText = new Text[mapBuilder.row*mapBuilder.col];
            Debug.Log("mapText"+mapText.Length);
        }
        //为测试数组赋值
        for (int i=0; i<mapBuilder.row; i++)
        {
            for (int j=0; j<mapBuilder.col; j++)
            {
                //对比两个数组中不同的部分，并将测试数组中对应部分清空
                // Debug.Log("mapClone"+mapClone.Length);
                // Debug.Log("map"+mapBuilder.map.Length);
                if (mapClone[i*mapBuilder.col+j] != mapBuilder.map[i*mapBuilder.col+j])
                {
                    //更新克隆数组和测试数组
                    mapClone[i*mapBuilder.col+j] = mapBuilder.map[i*mapBuilder.col+j];
                    Destroy(mapText[i*mapBuilder.col+j].gameObject);
                    mapText[i*mapBuilder.col+j] = null;
                }
                //实时绘制地图快照
                if (mapText[i*mapBuilder.col+j] == null)
                {
                    GameObject temp = Instantiate(textParfab,new Vector3((j+1)*20,(mapBuilder.col-i)*20,0),Quaternion.identity);
                    temp.GetComponent<Text>().text = mapBuilder.map[i*mapBuilder.col+j].ToString();
                    temp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform,false);
                    mapText[i*mapBuilder.col+j] = temp.GetComponent<Text>();
                }
            }
        }
    }
}
