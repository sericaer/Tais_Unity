using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text desc;

    public void OnConfirm()
    {
        Destroy(this.gameObject);
    }

}
