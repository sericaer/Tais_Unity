using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class WarnContent : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Process(string warnName, bool value)
    {
        if(value)
        {
            if(list.Any(x=>x.name == warnName))
            {
                return;
            }

            var warnObj = Instantiate(Resources.Load(WarnPanel.prefabsName), this.transform) as GameObject;
            warnObj.name = warnName;
            list.Add(warnObj);
        }
        else
        {
            var warnObj = list.SingleOrDefault(x => x.name == warnName);
            if(warnObj == null)
            {
                return;
            }

            list.Remove(warnObj);
            Destroy(warnObj);
        }

    }

    List<GameObject> list = new List<GameObject>();
}
