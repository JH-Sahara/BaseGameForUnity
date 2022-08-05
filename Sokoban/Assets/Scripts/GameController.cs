using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text stepText;
    public GameObject winImage;
    public GameObject resetButton;
    public GameObject startGameButton;
    private int step;
    private int allPoint;
    private int nowPoint;
    public bool isGameOver{get;set;}
    // Start is called before the first frame update
    void Start()
    {
        InitGameController();
    }

    // Update is called once per frame
    void Update()
    {
        //修改文字
        stepText.text = "Step：" + step;
        //Debug.Log("step:"+step+",allPoint:"+allPoint+",nowPoint:"+nowPoint);
        //判断游戏是否结束
        //Debug.Log("over"+nowPoint+","+allPoint);
        if (nowPoint == allPoint && allPoint != 0)
        {
            GameOver();
        }
    }

    //初始化函数
    public void InitGameController()
    {
        //step = 0;
        //allPoint = 0;
        //nowPoint = 0;
        isGameOver = false;
        resetButton.SetActive(true);
        winImage.SetActive(false);
        startGameButton.SetActive(false);
    }

    //结束游戏
    private void GameOver()
    {
        resetButton.SetActive(false);
        winImage.SetActive(true);
        startGameButton.SetActive(true);
        isGameOver = true;
    }

    //增加步数
    public void AddStep()
    {
        step ++;
    }

    //根据实例化阶段计算所有Point
    public void AddAllPoint()
    {
        allPoint ++;
        //Debug.Log("函数内部");
    }

    //增加箱子到达的关键点个数
    public void AddNowPoint()
    {
        nowPoint ++;
    }

    //减少箱子到达关键点的个数
    public void SubNowPoint()
    {
        nowPoint --;
    }
}
