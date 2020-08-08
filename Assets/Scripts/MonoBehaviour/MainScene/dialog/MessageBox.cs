using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MessageBox : MonoBehaviour
{
    public static string prefabsName = "Prefabs/MessageBox";

    public Text desc;
    public Action Act;

    public void OnConfirm()
    {
        Act?.Invoke();
        Act = null;
        Destroy(this.gameObject);
    }

}
