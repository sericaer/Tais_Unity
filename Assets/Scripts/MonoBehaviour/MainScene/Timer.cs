using System;
using System.Collections;
using TaisEngine;
using TaisEngine.ModManager;
using TaisEngine.Run;
using Tools;
using UniRx.Async;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Timer : MonoBehaviour
{
    public Button btnInc;
    public Button btnDec;
    public Toggle togPause;

    public const int MaxSpeed = 4;
    public const int MinSpeed = 1;

    // AfterAssembliesLoaded is called before BeforeSceneLoad
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void InitUniTaskLoop()
    {
        var loop = PlayerLoop.GetCurrentPlayerLoop();
        PlayerLoopHelper.Initialize(ref loop);
    }

    public int currSpeed
    {
        get
        {
            return _currSpeed;
        }
        set
        {
            if(value >= MaxSpeed)
            {
                _currSpeed = MaxSpeed;
                btnInc.interactable = false;
            }
            else if(value <= MinSpeed)
            {
                _currSpeed = MinSpeed;
                btnDec.interactable = false;
            }
            else
            {
                btnInc.interactable = true;
                btnDec.interactable = true;

                _currSpeed = value;
            }
        }
    }

    public static void Pause()
    {
        pauseCount++;
    }

    public static void unPause()
    {
        pauseCount--;
    }

    async void Start()
    {
#if UNITY_EDITOR
        _currSpeed = 10;
#else
        _currSpeed = MinSpeed;
#endif
        btnInc.onClick.AddListener(() => currSpeed++);
        btnDec.onClick.AddListener(() => currSpeed--);
        togPause.onValueChanged.AddListener((isOn) => isUserPause = isOn);

        await UniTask.Run(async () =>
        {
            try
            {
                while (!RunData.inst.end_flag)
                {
                    await UniTask.WaitUntil(() => !isPaused);

                    await RunData.inst.DaysInc(CreateDialog);

                    await UniTask.Delay(1000/currSpeed, true);

                }

                await SceneManager.LoadSceneAsync("EndScene");
            }
            catch(Exception e)
            {
                //await UniTask.SwitchToMainThread();
                //GetComponentInParent<MainScene>().CreatErrorDialog(e.Message);
                Log.ERRO(e.Message);
            }
        });
    }

    internal async UniTask CreateDialog(EventDef.Element gevent)
    {
        if (gevent != null)
        {

            {
                await UniTask.SwitchToMainThread();
                GetComponentInParent<MainScene>().CreateEventDialogAsync(gevent);
            }

            await UniTask.WaitUntil(() => !isPaused);
        }
    }

    private bool isPaused
    {
        get
        {
            return (isUserPause || pauseCount != 0);
        }
    }

    //private float m_fWaitTime;

    public bool isUserPause = false;
    //public static bool isSysPause = false;


    private static int pauseCount = 0;

    private static int _currSpeed;
}
