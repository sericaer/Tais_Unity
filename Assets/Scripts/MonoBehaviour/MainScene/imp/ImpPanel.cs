using UnityEngine;

using TaisEngine.Run;

public class ImpPanel : MonoBehaviour
{
    public GameObject taisDetail;
    public GameObject chaotingDetail;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onTaisClick()
    {
        var taisDetailObj = Instantiate(taisDetail, GetComponentInParent<Canvas>().transform) as GameObject;
        taisDetailObj.GetComponent<TaishouDetail>().gmTaishou = RunData.inst.taishou;
    }

    //public void onChaotingClick()
    //{
    //    var detailObj = Instantiate(chaotingDetail, GetComponentInParent<Canvas>().transform) as GameObject;
    //    detailObj.GetComponent<ChaotingDetail>().gmChaoting = RunData.inst.chaoting;
    //}
}
