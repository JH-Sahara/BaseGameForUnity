using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private string patchesFilePath = "Sprites/Patches";
    private Sprite[] patcheSprites;
    public GameObject patchePrefab;
    private int row = 3;
    private int col = 3;
    public Vector2 xPosRange;
    public Vector2 yPosRange;
    public int score{get;set;}
    public GameObject gameOverText;
    public GameObject gameOverButton;
    // Start is called before the first frame update
    void Start()
    {
        initGameController();
        LoadAllPatches(patchesFilePath);
        CreateAllPatches();
    }

    // Update is called once per frame
    void Update()
    {
        if (score == row*col)
        {
            GameOver();
        }
    }

    //初始化函数
    private void initGameController()
    {
        score = 0;
    }

    //批量读取图片
    private void LoadAllPatches(string patchesFilePath)
    {
        patcheSprites = Resources.LoadAll<Sprite>(patchesFilePath);
    }

    //生成切片对象
    private void CreateAllPatches()
    {
        Vector3 patchSize = patchePrefab.GetComponent<SpriteRenderer>().bounds.size;
        Debug.Log(patchSize);
        for (int i=0; i<row; i++)
        {
            for (int j=0; j<col; j++)
            {
                GameObject temp;
                temp = Instantiate(patchePrefab,
                                new Vector3(Random.Range(xPosRange.x,xPosRange.y),Random.Range(yPosRange.x,yPosRange.y),0),
                                Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().sprite = patcheSprites[i*col+j];
                temp.GetComponent<Patche>().targetPos = new Vector3((0.5f+j)*patchSize.x,(row-0.5f-i)*patchSize.y,0);
            }
        }
    }

    //匹配后增加score分数
    public void AddScore()
    {
        score ++;
    }

    //结束游戏
    private void GameOver()
    {
        gameOverText.SetActive(true);
        gameOverButton.SetActive(true);
    }
}
