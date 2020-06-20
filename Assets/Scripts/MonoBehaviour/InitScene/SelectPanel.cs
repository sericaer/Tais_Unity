using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;
using TaisEngine.ModManager;

public class SelectPanel : MonoBehaviour
{
    public LocalText textDesc;
    public List<Button> btns;

    internal InitSelectDef.Element initSelectDef;

    // Use this for initialization
    void Start()
    {
        textDesc.format = initSelectDef.desc.Result();

        //foreach (var elem in initSelectDef.options)
        //{
        //    var index = int.Parse(elem.Key.Replace("OPTION_", ""));
        //    if (index > btns.Count())
        //    {
        //        throw new Exception($"{elem.Key} not match select button");
        //    }

        //    var btn = btns[index];
        //    btn.gameObject.SetActive(true);

        //    btn.GetComponentInChildren<LocalText>().format = elem.Value.desc();

        //    btn.onClick.AddListener(() =>
        //    {
        //        elem.Value.selected();

        //        Destroy(this.gameObject);

        //        var next = elem.Value.next_select();
        //        if (next != "")
        //        {
        //            //GetComponentInParent<sceneInit>().CreateSelectPanel(next);
        //        }
        //        else
        //        {
        //            GetComponentInParent<InitScene>().CreateReportPanel();
        //        }
        //    });
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
