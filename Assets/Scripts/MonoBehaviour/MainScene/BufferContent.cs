using System.Collections;
using System.Linq;

using UnityEngine;

using TaisEngine.Run;
using System.Collections.Generic;

public class BufferContent : MonoBehaviour
{
    public GameObject buffPrefabs;
    public BufferManager buffmgr;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var needDestroys = listBufferPanels.Where(x => buffmgr.All(y=> y!= x.gmBuffer)).ToArray();
        foreach (var elem in needDestroys)
        {
            Destroy(elem.gameObject);

            listBufferPanels.Remove(elem);
        }

        var needCreate = buffmgr.Where(x => listBufferPanels.All(y => y.gmBuffer != x)).ToArray();
        foreach (var elem in needCreate)
        {
            var taskObj = Instantiate(buffPrefabs, this.transform);

            taskObj.name = elem.name;
            taskObj.GetComponent<BufferPanel>().gmBuffer = elem;

            listBufferPanels.Add(taskObj.GetComponent<BufferPanel>());
        }
    }

    private List<BufferPanel> listBufferPanels = new List<BufferPanel>();
}
