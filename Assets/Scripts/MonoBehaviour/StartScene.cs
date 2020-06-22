using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TaisEngine.Run;
using TaisEngine.Init;

public class StartScene : MonoBehaviour
{
    public GameObject modSelectPanl;
    public GameObject saveSelectPanl;

    public void onNew()
    {

#if UNITY_EDITOR_OSX
        RunData.New(InitData.Random());
        SceneManager.LoadScene("MainScene");
#else
        SceneManager.LoadScene("InitScene");
#endif

    }

    public void onLoad()
    {
        saveSelectPanl.SetActive(true);
    }

    public void OnMod()
    {
        modSelectPanl.SetActive(true);
    }

    public void onQuit()
    {
        Application.Quit();
    }
}
