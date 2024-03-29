﻿using System.Collections;
using System.Dynamic;
using System.Linq;
using TaisEngine.Init;
using TaisEngine.ModManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    public GameObject selectPanel;
    public GameObject reportPanel;
    public GameObject nameAgePanel;

    void Start()
    {
        InitData.Generate();
        Visitor.SetObj("init", InitData.inst);

        CreateNameAgePanel();
    }

    internal void CreateSelectPanel(InitSelectDef selectDef)
    {
        var panelSelect = Instantiate(selectPanel, this.transform) as GameObject;
        panelSelect.GetComponentInChildren<SelectPanel>().initSelectDef = selectDef;
    }

    internal void CreateReportPanel()
    {
        var panelSelect = Instantiate(reportPanel, this.transform) as GameObject;
    }

    internal void CreateNameAgePanel()
    {
        var panelSelect = Instantiate(nameAgePanel, this.transform) as GameObject;
    }
}
