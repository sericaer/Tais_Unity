using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TaisEngine.Run;

public class EndScene : MonoBehaviour
{
    public Text histroyRecord;

    // Use this for initialization
    void Start()
    {
        histroyRecord.text = string.Join("\n", RunData.inst.recordMsg);
        RunData.inst = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onConfirm()
    {
        SceneManager.LoadScene("StartScene");
    }
}
