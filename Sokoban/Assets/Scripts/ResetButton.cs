using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    void Start()
    {
       
    }
    //重新开始加载游戏
    public void AgainGame()
    {
        SceneManager.LoadScene("Sokoban");
    }
}
