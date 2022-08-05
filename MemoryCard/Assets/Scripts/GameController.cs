using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject MemoryCardPrefab;
    public float originalX = -4.5f;
    public float originalY = -2.5f;
    public int gridRows = 3;
    public int gridCols = 4;
    public float offsetX = 3;
    public float offsetY = 2.5f;
    public Sprite[] cardImages;
    public int[] numbers = {0,0,1,1,2,2,3,3,4,4,5,5};
    public MemoryCard firstMemoryCard;
    public MemoryCard secondMemoryCard;
    private int score = 0;//判断游戏胜利条件的参数
    public GameObject victoryImage;
    public GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        numbers = ShuffleCards(numbers);
        initMap();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(firstMemoryCard);
        Debug.Log(secondMemoryCard);
        //当两张卡牌同时不为null时开启协程
        if (firstMemoryCard != null && secondMemoryCard != null)
        {
            Debug.Log("即将开启协程：");
            StartCoroutine("Matched");
        }
    }

    // 初始化场景函数
    public void initMap()
    {
        for (int i=0; i<gridRows; i++)
        {
            for (int j=0; j<gridCols; j++)
            {
                GameObject temp;
                temp = Instantiate(MemoryCardPrefab,new Vector3(originalX + j * offsetX, originalY + i * offsetY, 0),Quaternion.identity);
                temp.GetComponent<MemoryCard>().cardImage = cardImages[numbers[i*gridCols+j]];
                temp.GetComponent<MemoryCard>().setID(numbers[i*gridCols+j]);
            }
        }
    }

    //洗牌算法
    public int[] ShuffleCards(int[] numbers)
    {
        int[] newNumbers = numbers.Clone() as int[];
        int temp;
        int rander;

        for (int i=0; i<newNumbers.Length; i++)
        {
            temp = newNumbers[i];
            rander = Random.Range(i,newNumbers.Length);
            newNumbers[i] = newNumbers[rander];
            newNumbers[rander] = temp;
        }

        return newNumbers;
    }

    //协程处理函数
    public IEnumerator Matched()
    {
        Debug.Log("协程开启");
        if (firstMemoryCard.getID() == secondMemoryCard.getID())
        {
            firstMemoryCard.CardMatched();
            secondMemoryCard.CardMatched();
            score ++;
            if (score == cardImages.Length)
            {
                GameOver();
            }
        }else {
            yield return new WaitForSeconds(1.5f);
            firstMemoryCard.CardUnReveal();
            secondMemoryCard.CardUnReveal();
        }

        firstMemoryCard = null;
        secondMemoryCard = null;
        StopCoroutine("Matched");
    }

    //游戏结束函数
    public void GameOver()
    {
        victoryImage.SetActive(true);
        startButton.SetActive(true);
    }
}
