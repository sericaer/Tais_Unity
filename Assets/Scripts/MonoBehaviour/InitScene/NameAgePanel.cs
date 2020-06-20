using System;
using System.Collections;
using System.Linq;
using TaisEngine.ModManager;
using UnityEngine;
using UnityEngine.UI;

public class NameAgePanel : MonoBehaviour
{

    public InputField taishouName;
    public Text age;


    public void onNameRandom()
    {
        taishouName.text = PersonName.EnumFamily().OrderBy(x => Guid.NewGuid()).First()
                  + PersonName.EnumGiven().OrderBy(x => Guid.NewGuid()).First();

        age.text = ageValue.ToString();
    }

    public void onAgeInc()
    {
        
        ageValue += 5;

        //if (ageValue > TaisEngine.InitDataTaishou.ageRange.max)
        //{
        //    ageValue = TaisEngine.InitDataTaishou.ageRange.max;
        //}

        age.text = ageValue.ToString();
    }

    public void onAgeDec()
    {
        ageValue -= 5;

        //if(ageValue < TaisEngine.InitDataTaishou.ageRange.min)
        //{
        //    ageValue = TaisEngine.InitDataTaishou.ageRange.min;
        //}

        age.text = ageValue.ToString();
    }

    public void onConfirm()
    {
        //TaisEngine.InitData.inst.taishou.name = name.text;
        //TaisEngine.InitData.inst.taishou.age = ageValue;

        gameObject.SetActive(false);

        var select1st = InitSelectDef.Enumerate().Single(x => x.is_first.Result());
        GetComponentInParent<InitScene>().CreateSelectPanel(select1st);
    }

    // Use this for initialization
    void Start()
    {
        onNameRandom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int ageValue = 35;
}
