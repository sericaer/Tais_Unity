using System;
using ModelShark;
using UnityEngine;

public class DynamicToolTip : MonoBehaviour
{
    public void AddDesc(Func<(string desc, string detail)> _funcDetail)
    {
        this.funcDetail = _funcDetail;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add the TooltipTrigger component to the button.
        tooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
        TooltipStyle tooltipStyle = Resources.Load<TooltipStyle>("MetroSimple");
        tooltipTrigger.tooltipStyle = tooltipStyle;

        // Set the tooltip text.
        //tooltipTrigger.SetText("BodyText", string.Format("This object was created at <b><color=#F3B200>runtime</color></b> from a prefab that <b><color=#F3B200>did not</color></b> already have a tooltip on it. The tooltip was added programmatically and the message and other parameters modified through code. This \"metro\" tooltip style was also added dynamically to the scene.\n\nObject created and tooltip text assigned at {0}.", DateTime.Now));

        // Set some extra style properties on the tooltip
        tooltipTrigger.maxTextWidth = 250;
        tooltipTrigger.backgroundTint = Color.white;
        tooltipTrigger.tipPosition = TipPosition.BottomRightCorner;

        tooltipTrigger.evtShow = () =>
        {
            var desc = funcDetail();
            tooltipTrigger.SetTextDetail(desc.desc, desc.detail);
        };
    }

    void Update()
    {
    }

    void OnDisable()
    {
        if(TooltipManager.Instance == null)
        {
            return;
        }

        if(tooltipTrigger != null)
        {
            tooltipTrigger.HidePopup();
        }
    }

    private TooltipTrigger tooltipTrigger;
    private Func<(string desc, string detail)> funcDetail;
}
