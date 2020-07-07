using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TaisEngine;

public class DialogEsc : MonoBehaviour
{
    public Toggle toggle;
    public GameObject saveFileDialog;

    public void onClickSave()
    {
        saveFileDialog.SetActive(true);
    }

    public void onClickQuit()
    {
        //GMData.inst.end_flag = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync("StartScene");
    }

    public void onClickCancel()
    {
        toggle.isOn = false;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        this.transform.SetAsLastSibling();

        Timer.Pause();
    }

    void OnDisable()
    {
        Timer.unPause();
    }
}
