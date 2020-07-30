using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TaisEngine.ModManager;
using TaisEngine.Run;
using System.Collections.Generic;
using System;

//using ModelShark;

public class InComeDetail : MonoBehaviour
{
    public List<Toggle> toggles;
    public Text value;
    public InCome gmIncome;

    public double Num;

    public bool isChanged
    {
        get
        {
            return newLevel != gmIncome.GetCurrLevel();
        }
    }

    internal int newLevel;

    private void Start()
    {
        Num = gmIncome.CalcCurrValue();
        value.text = Num.ToString();

        var level = gmIncome.GetCurrLevel();
        if (level < 1 || level > toggles.Count)
        {
            throw new System.Exception();
        }

        newLevel = level;

        toggles[level - 1].isOn = true;

        for(int i=0; i<toggles.Count; i++)
        {
            toggles[i].onValueChanged.AddListener((curr) =>
            {
                if(curr)
                {
                    newLevel = toggles.FindIndex(x => x.isOn) + 1;

                    Num = gmIncome.CalcExpandValue(newLevel);
                    value.text = Num.ToString();
                }
            });
        }
    }

    internal void Confirm()
    {
        gmIncome.SetLevel(newLevel);
    }
}
