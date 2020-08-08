using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Economy : MonoBehaviour
{
    public Text value;

    public GameObject detail;

    public void onDetailClick()
    {
        var panelDialog = Instantiate(detail, GetComponentInParent<MainScene>().Center.transform) as GameObject;
        //detail.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        value.text = TaisEngine.Run.RunData.inst.economy.value.ToString("N1");
    }
}
