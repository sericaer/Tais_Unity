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
    public Slider slider;
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
        if (level > slider.maxValue)
        {
            throw new System.Exception();
        }

        slider.value = level;
        newLevel = level;

        slider.onValueChanged.AddListener((curr) =>
        {
            newLevel = (int)curr;

            Num = gmIncome.CalcExpandValue(newLevel);
            value.text = Num.ToString();
        });
    }

    internal void Confirm()
    {
        gmIncome.SetLevel(newLevel);
    }
}
