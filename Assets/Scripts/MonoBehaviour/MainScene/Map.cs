using System;

using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Image image;
    public GameObject depart;

    public void OnClickMap()
    {
        RectTransform rectTransform = image.transform as RectTransform;


        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);

        var r = rectTransform.rect;
        var tex = image.mainTexture as Texture2D;
        int px = Mathf.Clamp(0, (int)(((localPoint.x - r.x) * tex.width) / r.width), tex.width);
        int py = Mathf.Clamp(0, (int)(((localPoint.y - r.y) * tex.height) / r.height), tex.height);


        var color = (image.mainTexture as Texture2D).GetPixel(Convert.ToInt32(px), Convert.ToInt32(py));

        var strColor = string.Format("({0:0}, {1:0}, {2:0})", color.r * 255, color.g * 255, color.b * 255);

        Debug.Log(strColor);

        //var gmDepart = TaisEngine.GMData.inst.FindDepartByColor(strColor);
        //if (gmDepart == null)
        //{
        //    return;
        //}

        //var gameObj = Instantiate(depart, GetComponentInParent<Canvas>().transform);
        //gameObj.GetComponentInChildren<Depart>().gmDepart = gmDepart;
    }
}
