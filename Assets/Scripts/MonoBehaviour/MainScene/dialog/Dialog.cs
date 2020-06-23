using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Dialog : MonoBehaviour
{
    public virtual bool isQueue { get { return true; } }

    void Awake()
    {
        Timer.Pause();

        if(isQueue)
        {
            if (listDialog.Count != 0)
            {
                this.gameObject.SetActive(false);
            }

            listDialog.Enqueue(this);
        }

    }

    void OnDestroy()
    {
        Timer.unPause();

        if (isQueue)
        {
            listDialog.Dequeue();
        }

        if (listDialog.Count != 0)
        {
            var panel = listDialog.Peek();
            panel.gameObject.SetActive(true);
        }
    }

    private static Queue<Dialog> listDialog = new Queue<Dialog>();
}
