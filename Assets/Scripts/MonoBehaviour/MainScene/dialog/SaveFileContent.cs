using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TaisEngine;
using UnityEngine.UI;
using TaisEngine.Serialize;

public class SaveFileContent : MonoBehaviour
{
    public GameObject saveItemPrefabs;
    public bool canSelectd;
    public string seleced = "";

    List<GameObject> saveItems = new List<GameObject>();

    void OnEnable()
    {
        RefreshSave();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshSave()
    {
        foreach (var elem in saveItems)
        {
            Destroy(elem);
        }
        saveItems.Clear();

        foreach (var elem in Directory.EnumerateFiles(GMSerialize.savePath, "*.save"))
        {
            var saveItem = Instantiate(saveItemPrefabs, transform) as GameObject;

            if(!canSelectd)
            {
                saveItem.transform.Find("btnSelect").gameObject.SetActive(false);
            }

            var saveFileName = Path.GetFileNameWithoutExtension(elem);
            saveItem.GetComponentInChildren<Text>().text = saveFileName;
            saveItem.transform.Find("btnDelete").GetComponentInChildren<Button>().onClick.AddListener(() => { onClickDelete(saveFileName); });
            saveItem.transform.Find("btnSelect").GetComponentInChildren<Button>().onClick.AddListener(() => { seleced = saveFileName; });

            saveItems.Add(saveItem);
        }
    }

    private void onClickDelete(string saveFileName)
    {
        File.Delete($"{GMSerialize.savePath}/{saveFileName}.save");
        if(saveFileName == seleced)
        {
            seleced = "";
        }
        RefreshSave();
    }
}
