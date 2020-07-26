using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DialogNextChangedConfirm : MonoBehaviour
{
    public string desc;

    public Text text;

    public Action act;

    public void OnConfirm()
    {
        Destroy(this.gameObject);
        act();
    }

    public void OnCancel()
    {
        Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        text.text = desc;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
