using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI.Extensions;

using ModelShark;
using Tools;
using UnityUITable;
using System;

public class Pop : MonoBehaviour
{
    public LocalText name;
    public LocalText depart;
    public LocalText num;
    public LocalText farm;

    //public LocalText expcetTax;
    //public LocalText attitude;
    //public LocalText background;

    public GameObject consume;
    public GameObject familyPanel;

    public GameObject buffContent;
    public GameObject buffPrefabs;

    //public Buffer bufferPrefabs;

    public TaisEngine.Pop gmPop;

    //public class PERSON_INFO
    //{
    //    public string name;
    //    public int age;
    //    public int attitude;
    //    public string weight;
    //}

    //public List<PERSON_INFO> tableList = new List<PERSON_INFO>();
    //{
    //    get
    //    {
    //        return pop.family.persons.Select(x => new POP_INFO() {name = x.fullName, age = GRandom.getNum(20, 60)}).ToList();
    //    }
    //}

    public void onConfirm()
    {
        Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        name.format = gmPop.name;
        depart.format = gmPop.depart.name;

        if (gmPop.is_consume)
        {
            consume.GetComponent<TooltipTrigger>().funcGetTooltipStr = () =>
            {
                var detail = string.Join("\n", gmPop.consumeDetail.Select(x => $"<color={(x.value < 0 ? "red" : "green")}>{TaisEngine.Mod.GetLocalString(x.name)} {x.value.ToString("N2")} </color>"));
                return ("consume", detail);
            };
        }

        if (gmPop.family != null)
        {
            familyPanel.SetActive(true);
            familyPanel.GetComponent<FamilyPanel>().gmFamily = gmPop.family;
        }

        //personTable.SetActive(pop.);
        //if (pop.familyValid)
        //{
        //    background.localString = pop.family.background.getName();
        //    attitude.format = pop.family.attitude.ToString();

        //    personTable.GetComponent<PersonTable>().getPersonsFunc = () => pop.family.persons;
        //    personTable.GetComponent<PersonTable>().tableObj.HideColumn("isSelected", true);
        //}
        //else
        //{
        //    background.transform.parent.gameObject.SetActive(false);
        //    attitude.transform.parent.gameObject.SetActive(false);
        //}
    }

    void Update()
    {
        num.format = gmPop.num.ToString("N0");

        if(gmPop.is_consume)
        {
            consume.transform.Find("value").GetComponent<LocalText>().format = gmPop.consume.ToString();
        }
        else
        {
            consume.SetActive(false);
        }

        //if(pop.farmVaild)
        //{
        //    farm.format = Math.Round(pop.farm, 2).ToString();
        //}
        //else
        //{
        //    farm.transform.parent.gameObject.SetActive(false);
        //}

        //if (pop.def.is_tax)
        //{
        //    expcetTax.format = Math.Round(pop.expectTax, 2).ToString();
        //}
        //else
        //{
        //    expcetTax.transform.parent.gameObject.SetActive(false);
        //}



        //var expcetTaxDetail = pop.expectTaxDetail;
        //if (expcetTaxDetail != null)
        //{
        //    var expcetTaxDetailStr = expcetTaxDetail.Select(x => $"<color={(x.value > 0 ? LocalString.getColorStr(LocalString.COLOR.green) : LocalString.getColorStr(LocalString.COLOR.red))}>{x.value.ToString("+00;-00")}    {new LocalString(x.cause).ToString()}</color>").ToList();
        //    expcetTaxDetailStr.AddRange(pop.collectRatioDetail.Select(x => $"<color={(x.value > 0 ? LocalString.getColorStr(LocalString.COLOR.green) : LocalString.getColorStr(LocalString.COLOR.red))}>{x.value.ToString("+00;-00")}%    {new LocalString(x.cause).ToString()}</color>"));
        //    expectTaxTooltipTrigger.SetTextDetail("POP_EXPECT_TAX_DETAIL", string.Join("\n", expcetTaxDetailStr));
        //}

        //var needRemove = listBufferObj.FindAll(x => !pop.buffers.Contains(x.buffer));
        //needRemove.ForEach(x =>
        //{
        //    listBufferObj.Remove(x);
        //    Destroy(x.gameObject);
        //});

        //var needAdd = pop.buffers.Where(x => listBufferObj.All(y => y.buffer != x));
        //foreach (var buffer in needAdd)
        //{
        //    var bufferObj = Instantiate(bufferPrefabs, bufferContuent.transform);
        //    bufferObj.buffer = buffer;
        //    bufferObj.name = buffer.getName();

        //    listBufferObj.Add(bufferObj);
        //}

        UpdateBuffers();
    }

    private void UpdateBuffers()
    {
        var needDestroys = listBufferPanels.Where(x => gmPop.buffers.All(y=>y!= x.gmBuffer)).ToArray();
        foreach (var elem in needDestroys)
        {
            Destroy(elem.gameObject);

            listBufferPanels.Remove(elem);
        }

        var needCreate = gmPop.buffers.Where(x => listBufferPanels.All(y => y.gmBuffer != x)).ToArray();
        foreach (var elem in needCreate)
        {
            var taskObj = Instantiate(buffPrefabs, buffContent.transform);

            taskObj.name = elem.name;
            taskObj.GetComponent<BufferPanel>().gmBuffer = elem;

            listBufferPanels.Add(taskObj.GetComponent<BufferPanel>());
        }
    }

    private List<BufferPanel> listBufferPanels = new List<BufferPanel>();

    //private List<Buffer> listBufferObj = new List<Buffer>();
}
