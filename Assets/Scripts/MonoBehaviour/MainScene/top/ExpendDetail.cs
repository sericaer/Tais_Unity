using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TaisEngine.ModManager;
using TaisEngine.Run;
using System.Collections.Generic;
using System;

//using ModelShark;

public class ExpendDetail : MonoBehaviour
{
    public Slider slider;
    public Text value;
    public Expend gmExpend;

    public double Num;

    public bool isChanged
    {
        get
        {
            return newLevel != gmExpend.GetCurrLevel();
        }
    }

    internal int newLevel;

    private void Start()
    {
        Num = gmExpend.CalcCurrValue();
        slider.value = gmExpend.GetCurrLevel();

        newLevel = (int)slider.value;

        slider.onValueChanged.AddListener((curr) =>
        {
            newLevel = (int)curr;

            Num = gmExpend.CalcExpandValue(newLevel);
        });
    }

    private void Update()
    {
        value.text = Num.ToString("N1");
    }

    internal void Confirm()
    {
        gmExpend.SetLevel(newLevel);
    }
}
