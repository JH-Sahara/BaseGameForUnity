using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCursor : MonoBehaviour
{
    public Sprite hammer;
    public Sprite hitHammer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GetComponent<Image>().sprite = hitHammer;
        }else{
            GetComponent<Image>().sprite = hammer;
        }

        transform.position = Input.mousePosition;
    }
}
