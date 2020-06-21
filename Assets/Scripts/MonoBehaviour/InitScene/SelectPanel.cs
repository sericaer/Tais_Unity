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

        for(int i= 0; i< initSelectDef.options.Count(); i++)
        {
            var opt = initSelectDef.options[i];

            var btn = btns[i];
            btn.gameObject.SetActive(true);

            btn.GetComponentInChildren<LocalText>().format = opt.desc.Result();

            btn.onClick.AddListener(() =>
            {
                opt.selected.Run();

                //Destroy(this.gameObject);

                //var next = opt.next_select.Result();
                //if (next != "")
                //{
                //    //GetComponentInParent<sceneInit>().CreateSelectPanel(next);
                //}
                //else
                //{
                //    GetComponentInParent<InitScene>().CreateReportPanel();
                //}
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
