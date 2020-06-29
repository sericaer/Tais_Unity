using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaisEngine;
using UnityEngine.UI.Extensions;
using System;
using TaisEngine.Run;
using UnityEngine.SceneManagement;

public class MsgPanel : MonoBehaviour
{
    public GameObject msgElemtPrefabs;

    void OnEnable()
    {
        if (transform.childCount != RunData.inst.recordMsg.Count)
        {
            for (int i = 0; i < RunData.inst.recordMsg.Count; i++)
            {
                var gmObj = Instantiate(msgElemtPrefabs, this.transform) as GameObject;
                gmObj.GetComponent<LocalText>().format = RunData.inst.recordMsg[i];
                gmObj.transform.SetAsFirstSibling();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddMessage(string title)
    {
        if(this.gameObject.activeInHierarchy)
        {
            var item = RunData.inst.date + " " + title;
            RunData.inst.recordMsg.Add(item);

            var gmObj = Instantiate(msgElemtPrefabs, this.transform) as GameObject;
            gmObj.GetComponent<LocalText>().format = item;
            gmObj.transform.SetAsFirstSibling();
        }
    }
}