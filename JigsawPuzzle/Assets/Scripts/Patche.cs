using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patche : MonoBehaviour
{
    public Vector3 originalPos{get;set;}
    public Vector3 targetPos{get;set;}
    public bool isMatch{get;set;}
    private Camera mainCamera;
    private Vector3 mouseWorld;
    private float threshold = 0.2f;
    private int maxSortingOrder = 10;
    private int minsortingOrder = 0;
    public GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        initPatch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化时保存一些关键数值
    private void initPatch()
    {
        isMatch = false;
        originalPos = gameObject.transform.position;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnMouseDrag() {
        if (isMatch == true)
        {
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = maxSortingOrder;
        mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(mouseWorld.x,mouseWorld.y,0);
    }

    private void OnMouseUp() {
        if (Mathf.Abs(mouseWorld.x-targetPos.x)<threshold && Mathf.Abs(mouseWorld.y-targetPos.y)<threshold)
        {
            gameObject.transform.position = targetPos;
            isMatch = true;
            gameController.AddScore();
        }else{
            gameObject.transform.position = originalPos;
        }

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = minsortingOrder;
    }
}
