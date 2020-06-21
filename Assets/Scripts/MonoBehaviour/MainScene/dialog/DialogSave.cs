using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TaisEngine;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI.Extensions;
using TaisEngine.Run;
using TaisEngine.Serialize;

public class DialogSave : MonoBehaviour
{
    public GameObject saveItemPrefabs;
    public SaveFileContent saveItemContent;
    public InputField saveFileNameInput;

    public GameObject saveErrorDialog;

    

    public void onClickSave()
    {
     
        try
        {
            GMSerialize.Save(saveFileNameInput.text, RunData.inst);
            saveItemContent.RefreshSave();
        }
        catch(Exception e)
        {
            saveErrorDialog.transform.Find("content/title").GetComponent<LocalText>().format = e.Message;
            saveErrorDialog.SetActive(true);
        }
    }

    public void onClickCancel()
    {
        gameObject.SetActive(false);
    }

    public void onSaveErrorDialogConfirm()
    {
        saveErrorDialog.SetActive(false);
    }

    void OnEnable()
    {
        saveItemContent.canSelectd = false;
        saveFileNameInput.text = DateTime.Now.ToString("yyyyMMddHHmmss");
    }
}
