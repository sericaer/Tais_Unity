﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TaisEngine;
//using ModelShark;
using Tools;
using TaisEngine.ModManager;

public class DialogCommon : Dialog
{
    public LocalText title;
    public LocalText content;

    public List<Button> btns;

    internal EventDef.Element gEvent;

    void Start ()
    {
        object[] eventTitleParams = gEvent.title.Result();
        title.format = LocalString.Get(eventTitleParams[0] as string, eventTitleParams.Skip(1).ToArray());

        object[] eventDescParams = gEvent.desc.Result();
        content.format = LocalString.Get(eventDescParams[0] as string, eventDescParams.Skip(1).ToArray());

        for (int i = 0; i < gEvent.options.Count(); i++)
        {
            var btn = btns[i];

            var opt = gEvent.options[i];
            
            btn.gameObject.SetActive(true);
            //btn.interactable = opt.isVaild();

            object[] optDescParams = opt.desc.Result();
            btn.GetComponentInChildren<Text>().text = LocalString.Get(optDescParams[0] as string, optDescParams.Skip(1).ToArray());
            //btn.GetComponent<TooltipTrigger>().funcGetTooltipStr = () =>
            //{
            //    List<List<object>> toolTipParams = opt.tooltip();

            //    return (string.Join("\n", toolTipParams.Select(x =>
            //    {
            //        return Mod.GetLocalString(x[0] as string, x.Skip(1).ToArray());
            //    })), "");
            //};

            btn.onClick.AddListener(async () =>
            {

                Log.INFO($"EVENT:{eventTitleParams[0]}, SELECT:OPTION_{i}");

                //opt.Do();
                //var resumeRslt = opt.MakeResume();
                //if(resumeRslt != null)
                //{
                //    foreach(var elem in resumeRslt)
                //    {
                //        elem.owner.getResumeManager().Add(elem.resume);
                //    }
                //}

                //var nextEvent = opt.getNext();
                //if (nextEvent != null)
                //{
                //    GetComponentInParent<sceneMain>().CreateEventDialog(nextEvent);
                //}
                opt.selected.Run();

                Destroy(this.gameObject);

                string next_event = opt.next.Get();

                //if(next_event != "" && next_event != null)
                //{
                //    await GetComponentInParent<Timer>().CreateDialog(EventDef.find(next_event));
                //}

                //gEvent.DestroyAction?.Invoke();


            });
        }
    }
}