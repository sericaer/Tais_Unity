using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TaisEngine;
using Tools;
using TaisEngine.ModManager;
using TaisEngine.ConfigManager;

public class ModSelectPanel : MonoBehaviour
{
    public GameObject modTogglePrefabs;
    public GameObject modToggleContent;

    public GameObject restartPanel;

    void Start()
    {
        foreach (var mod in Mod.listMod)
        {
            var panel = Instantiate(modTogglePrefabs, modToggleContent.transform);
            panel.GetComponentInChildren<Text>().text = mod.info.name;
            panel.name = mod.info.name;

            panel.GetComponentInChildren<Toggle>().isOn = Config.inst.select_mods.Contains(mod.info.name) || mod.info.master;
            panel.GetComponentInChildren<Toggle>().interactable = !mod.info.master;
        }
    }

    public void OnConfirm()
    {
        var selected = modToggleContent.GetComponentsInChildren<Toggle>().Where(x => x.isOn).Select(x => x.GetComponentInChildren<Text>().text).ToList();
        Log.INFO($"select mod {string.Join(",", selected)}");

        if (Enumerable.SequenceEqual(Config.inst.select_mods.OrderBy(t => t), selected.OrderBy(t => t)))
        {
            this.gameObject.SetActive(false);
            return;
        }

        Config.inst.select_mods = selected;
        Config.Save();

        restartPanel.SetActive(true);
    }

    public void OnCanel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnReStart()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
