using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour
{
    private int ID;
    public Sprite backCardImage;
    public Sprite cardImage;
    public enum CardState{
        Reveal,UnReveal,Matched//展示，隐藏，匹配
    }
    public CardState cardState;
    public GameController gameController;
    public static int step = 0;
    public Text stepText;
    // Start is called before the first frame update
    void Start()
    {
        initCard(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //卡牌初始化函数
    public void initCard()
    {
        step = 0;
        gameObject.GetComponent<SpriteRenderer>().sprite = backCardImage;
        cardState = CardState.UnReveal;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        stepText = GameObject.FindGameObjectWithTag("StepText").GetComponent<Text>();
    }

    //展示卡牌
    public void CardReveal()
    {
        if (cardState == CardState.UnReveal)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cardImage;
            cardState = CardState.Reveal;
        }
    }

    //隐藏卡牌
    public void CardUnReveal()
    {
        if (cardState == CardState.Reveal)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = backCardImage;
            cardState = CardState.UnReveal;
        }
    }

    //设置卡牌匹配状态
    public void CardMatched()
    {
        if (cardState != CardState.Matched && cardState == CardState.Reveal)
        {
            cardState = CardState.Matched;
        }
    }

    //增加步数的方法
    public void AddStep()
    {
        step ++;
        stepText.text = "Step：" + step;
    }

    private void OnMouseDown() {
        if (cardState == CardState.Matched || cardState == CardState.Reveal)
            return;
        if (gameController.firstMemoryCard != null && gameController.secondMemoryCard != null)
            return;

        if (gameController.firstMemoryCard == null)
        {
            gameController.firstMemoryCard = this;
            CardReveal();
            AddStep();
        }else {
            gameController.secondMemoryCard = this;
            CardReveal();
            AddStep();
        }
    }

    //获取id的方法
    public int getID()
    {
        return ID;
    }
    public void setID(int id)
    {
        ID = id;
    }
}
