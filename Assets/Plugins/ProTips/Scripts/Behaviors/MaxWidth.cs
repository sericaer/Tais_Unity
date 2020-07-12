using UnityEngine;
using UnityEngine.UI;

public class MaxWidth : MonoBehaviour 
{
 
    public RectTransform textTransform;
    public LayoutElement layoutElement;
 
    private void Start()
    {
        checkWidth();
    }
 
    public void checkWidth()
    {
        layoutElement.enabled = (textTransform.rect.width > layoutElement.preferredWidth);
    }
 
}