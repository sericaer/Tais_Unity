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
        value.text = gmExpend.CalcCurrValue().ToString();
        slider.value = gmExpend.GetCurrLevel();

        newLevel = (int)slider.value;

        slider.onValueChanged.AddListener((curr) =>
        {
            newLevel = (int)curr;
            value.text = gmExpend.CalcExpandValue(newLevel).ToString();
        });
    }

    internal void Confirm()
    {
        gmExpend.SetLevel(newLevel);
    }
}
