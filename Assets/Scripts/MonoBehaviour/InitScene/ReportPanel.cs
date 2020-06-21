using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI.Extensions;
using TaisEngine.Init;
using TaisEngine.Run;

public class ReportPanel : MonoBehaviour
{
    public LocalText name;
    public LocalText age;
    public LocalText background;

    public void OnConfirm()
    {
        Destroy(this.gameObject);

        RunData.New(InitData.inst);

        SceneManager.LoadScene("MainScene");
    }

    public void OnCancel()
    {
        Destroy(this.gameObject);

        GetComponentInParent<InitScene>().CreateNameAgePanel();
    }


    // Use this for initialization
    void Start()
    {
        name.format = InitData.inst.taishou.name;
        age.format =InitData.inst.taishou.age.ToString();
        background.format = InitData.inst.taishou.background;
    }
}
