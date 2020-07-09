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

    internal InitSelectDef initSelectDef;

    // Use this for initialization
    void Start()
    {
        textDesc.format = initSelectDef.desc.Result()[0].ToString();

        for(int i= 0; i< initSelectDef.options.Count(); i++)
        {
            var opt = initSelectDef.options[i];

            var btn = btns[i];
            btn.gameObject.SetActive(true);

            btn.GetComponentInChildren<LocalText>().format = opt.desc.Result()[0].ToString();

            btn.onClick.AddListener(() =>
            {
                opt.selected.Run();

                Destroy(this.gameObject);

                //var next = opt.next.Get();
                //if (next != "")
                //{
                //    GetComponentInParent<InitScene>().CreateSelectPanel(InitSelectDef.Find(next));
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
