using System;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TaisEngine.ModManager;
using TaisEngine.ConfigManager;

using Tools;
using TaisEngine.Init;
using TaisEngine.Serialize;
using TaisEngine.Run;
using System.Reflection;

public class LoadScene : MonoBehaviour
{
    public GameObject loadErrorPanel;

    // Use this for initialization
    void Start()
    {
        Application.logMessageReceivedThreaded += Log.exceptionLogCallback;

        Log.INFO("!!!!!APPLICATION START!!!!!");

        Config.Load();

        try
        {
            Directory.CreateDirectory(GMSerialize.savePath);

            Visitor.InitReflect("init", typeof(InitData));
            Visitor.InitReflect("common", typeof(RunData));
            Visitor.InitReflect("depart", typeof(TaisEngine.Run.Depart));

            //InitData.AssocVisitor();
            //RunData.AssocVisitor();

            Mod.Load();
        }
        catch(Exception e) 
        {
            var errTitle = e.Message;
            var errDetail = "";

            while (e.InnerException != null)
            {
                if (!(e.InnerException is TargetInvocationException))
                {
                    errDetail += "\n" + e.InnerException.Message;
                }

                e = e.InnerException;
            }

            Log.ERRO(errTitle + "\n" + errDetail + "\n" + e.StackTrace);


            loadErrorPanel.SetActive(true);

            loadErrorPanel.transform.Find("title").GetComponent<Text>().text = errTitle;
            loadErrorPanel.transform.Find("detail").GetComponent<Text>().text = errDetail;

            return;
        }

        LocalText.getLocalString = (format) => { return LocalString.Get(format); };

        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLoadErrorConfirm()
    {
        loadErrorPanel.SetActive(false);
        Config.Reset();
        Config.Save();

        SceneManager.LoadScene("sceneLoad");
    }

}
