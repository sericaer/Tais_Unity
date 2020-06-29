using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class DepartPop : MonoBehaviour
{
    public LocalText type;
    public LocalText family;
    public Text num;
    public GameObject popPrefabs;

    public TaisEngine.Run.Pop gmPop;

    public void onClick()
    {
        var popObj = Instantiate(popPrefabs, this.GetComponentInParent<Depart>().transform);
        popObj.GetComponentInChildren<Pop>().gmPop = gmPop;
    }

    // Update is called once per frame
    void Update()
    {
        num.text = gmPop.num.ToString("N0");
    }

    // Use this for initialization
    void Start()
    {
        type.format = gmPop.name;

        family.gameObject.SetActive(false);
        //if (gmPop.family != null)
        //{
        //    family.format = gmPop.family.name;
        //    family.gameObject.SetActive(true);
        //}
    }
}
