using UnityEngine;
using System.Collections;
using UnityEngine.UI.Extensions;
using ModelShark;

//using ModelShark;

public class BufferPanel : MonoBehaviour
{
    public LocalText title;

    internal TaisEngine.Run.Buffer gmBuffer;

    internal TooltipTrigger tooltipTrigger;

    // Use this for initialization
    void Start()
    {
        title.format = gmBuffer.name;

        tooltipTrigger = GetComponent<TooltipTrigger>();
        tooltipTrigger.funcGetTooltipStr = getBuffText;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private (string title, string desc) getBuffText()
    {
        string title = TaisEngine.ModManager.LocalString.Get(gmBuffer.name);

        string desc = "";
        if (gmBuffer.def.effect_crop_growing_speed != null)
        {
            var effect = gmBuffer.def.effect_crop_growing_speed;

            desc += $"<color={(effect.Item2 < 0 ? "red" : "green")}>" + TaisEngine.ModManager.LocalString.Get("CROP_GROWING_EFFECT", effect.Item2.ToString("N1")) + "</color> \n";
        }
        if (gmBuffer.def.effect_consume != null)
        {
            var effect = gmBuffer.def.effect_consume;

            desc += $"<color={(effect.Item2 < 0 ? "red" : "green")}>" + TaisEngine.ModManager.LocalString.Get("CONSUME_EFFECT", effect.Item2.ToString("N1")) + "</color> \n";
        }

        return (title, desc);
    }
}
