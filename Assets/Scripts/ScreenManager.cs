using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Outage()
    {
        Debug.Log("Outage");
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject curChild = transform.GetChild(i).gameObject;
            Debug.Log(curChild);
            Color curColor = curChild.GetComponent<SpriteRenderer>().color;
            curColor.a = 0.5f;
            curChild.GetComponent<SpriteRenderer>().color = curColor;
        }
    }

    public void Recovery()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject curChild = transform.GetChild(i).gameObject;
            Color curColor = curChild.GetComponent<SpriteRenderer>().color;
            curColor.a = 1f;
            curChild.GetComponent<SpriteRenderer>().color = curColor;
        }
    }
}
