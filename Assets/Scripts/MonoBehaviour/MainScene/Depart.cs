
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System;
//using ModelShark;

public class Depart : MonoBehaviour
{
    public LocalText name;

    public GameObject popNum;
    //public Text cropGrowing;

    public GameObject cropGrowing;
    public GameObject popPrefabs;
    public GameObject popContent;

    public BufferContent bufferContent;

    internal TaisEngine.Run.Depart gmDepart;

    public void OnClickConfirm()
    {
        Destroy(transform.parent.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        name.format = gmDepart.def.name;

        foreach (var pop in gmDepart.pops)
        {
            var gameObj = Instantiate(popPrefabs, popContent.transform);

            var departPop = gameObj.GetComponent<DepartPop>();
            listDepartPops.Add(departPop);

            departPop.gmPop = pop;
            departPop.gameObject.SetActive(false);
        }

        bufferContent.buffmgr = gmDepart.bufferManager;

        //var cropGrowingToolTip = cropGrowing.transform.GetComponent<TooltipTrigger>();
        //cropGrowingToolTip.funcGetTooltipStr = () =>
        //{
        //    if (gmDepart.is_crop_growing)
        //    {
        //        return ("CROP_GROWING",
        //               string.Join("\n", gmDepart.growSpeedDetail.Select(x => $"<color={(x.value < 0 ? "red" : "green")}>{TaisEngine.Mod.GetLocalString(x.name)} {x.value.ToString("N2")} </color>")));
        //    }
        //    else
        //    {
        //        return ("CROP_GROWING", "NOT_CROP_GROW_SEASON");
        //    }
        //};

    }

    // Update is called once per frame
    void Update()
    {
        popNum.transform.Find("value").GetComponent<Text>().text = string.Format("{0:N0}/{1:N0}",
                                    gmDepart.pops.Where(x => x.def.is_tax).Sum(x => x.num),
                                    gmDepart.pops.Sum(x => x.num));

        cropGrowing.transform.Find("value").GetComponent<Text>().text = gmDepart.crop_grow_percent.ToString("N2");

        foreach (var pop in listDepartPops)
        {
            pop.gameObject.SetActive(pop.gmPop.num > 0);
        }

        //UpdateBuffers();
    }

    private void UpdateBuffers()
    {
        //var needDestroys = listBufferPanels.Where(x => gmDepart.buffers.All(y=> y!= x.gmBuffer)).ToArray();
        //foreach (var elem in needDestroys)
        //{
        //    Destroy(elem.gameObject);

        //    listBufferPanels.Remove(elem);
        //}

        //var needCreate = gmDepart.buffers.Where(x => listBufferPanels.All(y => y.gmBuffer != x)).ToArray();
        //foreach (var elem in needCreate)
        //{
        //    var taskObj = Instantiate(buffPrefabs, buffContent.transform);

        //    taskObj.name = elem.name;
        //    taskObj.GetComponent<BufferPanel>().gmBuffer = elem;

        //    listBufferPanels.Add(taskObj.GetComponent<BufferPanel>());
        //}
    }

    private List<DepartPop> listDepartPops = new List<DepartPop>();
    //private List<BufferPanel> listBufferPanels = new List<BufferPanel>();
}
