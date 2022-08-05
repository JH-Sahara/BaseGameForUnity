using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRender : MonoBehaviour
{
    //记录箱子的状态
    public int boxStat{get;set;}
    public Sprite box;
    public Sprite boxWithPoint;
    public GameController gameController;
    private Sprite nowBoxStatSprite;
    private Sprite LastBoxStatSprite;
    // Start is called before the first frame update
    void Start()
    {
        InitBoxRender();
    }

    // Update is called once per frame
    void Update()
    {
        nowBoxStatSprite = ChangeBoxSprite();
    }

    void LateUpdate() {
        if (nowBoxStatSprite == LastBoxStatSprite)
        {
            return;
        }else{
            if (nowBoxStatSprite == boxWithPoint)
            {
                gameController.AddNowPoint();
            }else{
                gameController.SubNowPoint();
            }
            LastBoxStatSprite = nowBoxStatSprite;
        }
    }

    //初始化
    private void InitBoxRender()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        LastBoxStatSprite = box;
    }

    //修改箱子sprite 
    private Sprite ChangeBoxSprite()
    {
        if (boxStat == (int)MapBuilder.TileType.Box)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = box;
        }else{
            gameObject.GetComponent<SpriteRenderer>().sprite = boxWithPoint;
        }

        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}
