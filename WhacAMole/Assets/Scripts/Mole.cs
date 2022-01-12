using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mole : MonoBehaviour
{
    public int id;
    public GameObject hitMolePrefab;
    public GameController gameController;
    public static int score = 0;
    public Text scoreText;
    public Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();

        if (timer.time > 10f)
        {
            Destroy(gameObject,2);
        }else{
            Destroy(gameObject,0.8f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.time <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown() {
        gameController.holes[id].moleStateObj = Instantiate(hitMolePrefab,gameObject.transform.position,Quaternion.identity);
        Destroy(gameObject);
        score ++;
        scoreText.text = "Score：" + score;
    }
}
