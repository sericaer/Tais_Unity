using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TaisEngine;
using TaisEngine.Run;
using TaisEngine.Serialize;

public class SaveSelectPanel : MonoBehaviour
{
    public SaveFileContent saveFileContent;

    public Text selectText;
    public Button btnConfirm;

    // Use this for initialization
    void OnEnable()
    {
        saveFileContent.canSelectd = true;
        saveFileContent.RefreshSave();
    }
    // Update is called once per frame
    void Update()
    {
        selectText.text = saveFileContent.seleced;
        if(selectText.text == "")
        {
            selectText.text = "----";
            btnConfirm.interactable = false;
        }
        else
        {
            btnConfirm.interactable = true;
        }
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }

    public void OnConfirm()
    {
        gameObject.SetActive(false);

        RunData.inst = GMSerialize.Load(selectText.text);
        SceneManager.LoadScene("MainScene");

    }
}
