using System;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TaisEngine.ModManager;
using TaisEngine.ConfigManager;

using Tools;

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
            //Directory.CreateDirectory(GMSerialize.savePath);

            Mod.Load();
        }
        catch(Exception e) 
        {
            var errTitle = e.Message;
            var errDetail = "";

            while (e.InnerException != null)
            {
                errDetail += e.InnerException.Message;
                e = e.InnerException;
            }

            Log.ERRO(errTitle);
            Log.ERRO(errDetail + e.StackTrace);


            loadErrorPanel.SetActive(true);

            loadErrorPanel.transform.Find("title").GetComponent<Text>().text = errTitle;
            loadErrorPanel.transform.Find("detail").GetComponent<Text>().text = errDetail;

            return;
        }

        LocalText.getLocalString = LocalString.Get;

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
