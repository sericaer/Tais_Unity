﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaisEngine;
using TaisEngine.ModManager;
using TaisEngine.Run;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public GameObject Center;

    public GameObject dialogCommon;
    public GameObject reportCollectTax;
    public GameObject dialogException;

    public MsgPanel msgPanel;

    //public FamilyTop familyTop;
    //public GameObject familyContent;

    // Use this for initialization
    void Start()
    {
        Visitor.SetObj("common", RunData.inst);

        //foreach(var family in TaisEngine.GMData.inst.familys)
        //{
        //    var familyTopObj = Instantiate(familyTop, familyContent.transform);
        //    familyTopObj.family = family;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (TaisEngine.GMData.inst.gmEnd)
        //{
        //    SceneManager.LoadScene("sceneEnd");
        //}
    }

    //internal void CreatErrorDialog(string error)
    //{
    //    var panelDialog = Instantiate(dialogException, this.transform) as GameObject;
    //    panelDialog.GetComponentInChildren<Text>().text = error;
    //}

    internal async UniTask CreateEventDialogAsync(EventInterface eventobj)
    {
        object[] eventTitleParams = eventobj.GetTitle();
        msgPanel.AddMessage(LocalString.Get(eventTitleParams[0] as string, eventTitleParams.Skip(1).ToArray()));

        //if (eventobj.hide.Result())
        //{
        //    var opt = eventobj.options[0];
        //    opt.selected.Run();

        //    var next = opt.next.Get();
        //    if (next != "" && next != null)
        //    {
        //        await CreateEventDialogAsync(EventDef.find(next));
        //    }
        //    return;
        //}

        var panelDialog = Instantiate(dialogCommon, this.transform) as GameObject;
        panelDialog.GetComponentInChildren<DialogCommon>().gEvent = eventobj;
    }

    internal async UniTask CreateInterDialogAsync(string interEventName)
    {
        switch(interEventName)
        {
            case "ECONOMY_NOT_SUPPORT_CURR_DEFICILT":
                {
                    var panelDialog = Instantiate(Resources.Load(MessageBox.prefabsName), this.transform) as GameObject;
                    var msgbox = panelDialog.GetComponent<MessageBox>();
                    msgbox.desc.text = LocalString.Get("ECONOMY_NOT_SUPPORT_CURR_DEFICILT");
                    msgbox.Act = () =>
                    {
                        Instantiate(Resources.Load(EconomyDetail.prefabsName), this.transform);
                    };
                }
                break;
            default:
                throw new Exception("can not find inter event:" + interEventName);
        }
    }


    //internal void CreateTaskCollectTaxReport()
    //{
    //    var panelDialog = Instantiate(reportCollectTax, this.transform) as GameObject;
    //}
}
