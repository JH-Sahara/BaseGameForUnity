using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //用于存放洞口信息的结构体
    public struct hole{
        public float posX;
        public float posY;
        public bool isAppear;
        public GameObject moleStateObj;
    }
    public hole[] holes;
    public GameObject holePrefab;
    public GameObject molePrefab;
    public Timer timer;
    public Text timeText;
    public GameObject gameOver;
    public GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        initMap(); 
        InvokeRepeating("MoleAppear",0.2f,1);
    }

    // Update is called once per frame
    void Update()
    {
        CleanHoleState();

        if (timer.time <= 0)
        {
            timeText.text = "Time：0";
            GameOver();
        }
    }

    private void initMap() 
    {
        holes = new hole[9];
        Vector2 original = new Vector2(-1.8f,-2);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                holes[i*3+j].posX = original.x + 2f*j;
                holes[i*3+j].posY = original.y + 1f*i;
                holes[i*3+j].isAppear = false;
                Instantiate(holePrefab,new Vector3(holes[i*3+j].posX,holes[i*3+j].posY,0),Quaternion.identity);
            }
        }
    }

    private void MoleAppear()
    {
        int id = Random.Range(0,9);
        while(holes[id].isAppear == true)
        {
            id = Random.Range(0,9);
        }

        holes[id].isAppear = true;
        holes[id].moleStateObj = Instantiate(molePrefab,new Vector3(holes[id].posX,holes[id].posY + 0.4f,0),Quaternion.identity);
        holes[id].moleStateObj.GetComponent<Mole>().id = id;
    }

    //清空isAppear状态
    private void CleanHoleState()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (holes[i*3+j].moleStateObj == null)
                {
                    holes[i*3+j].isAppear = false;
                }
            }
        }
    }

    //游戏结束程序
    private void GameOver()
    { 
        Mole.score = 0;
        CancelInvoke();
        Destroy(timer);
        gameOver.SetActive(true);
        startButton.SetActive(true);
    }
}
