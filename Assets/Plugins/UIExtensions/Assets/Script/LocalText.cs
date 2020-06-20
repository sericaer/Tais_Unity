using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{ 
    public class LocalText: MonoBehaviour
    {
        public Text text
        {
            get
            {
                return GetComponentInChildren<Text>();
            }
        }

        public static Func<string, string> getLocalString = (string key) =>
        {
            return key;
        };

        [SerializeField]
        private string _format;
        //public bool isAutoSize;

        public string format
        {
            get
            {
                return _format;

            }
            set
            {
                _format = value;

                text.text = getLocalString(_format);
            }
        }

        //public void RefreshSize()
        //{
        //    var selfRectTransform = GetComponent<RectTransform>();
        //    var textRectTransform = text.GetComponent<RectTransform>();

        //    if (isAutoSize)
        //    {
        //        textRectTransform.SetAnchor(AnchorPresets.MiddleCenter);
        //        textRectTransform.localPosition = new Vector2(0, 0);

        //        selfRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x, textRectTransform.sizeDelta.y);
        //    }
        //    else
        //    {
        //        textRectTransform.SetAnchor(AnchorPresets.StretchAll);
        //        textRectTransform.sizeDelta = new Vector2(0, 0);
        //    }
        //}

        void Start()
        {
            text.text = getLocalString(_format);
        }

        void Update()
        {
            //RefreshSize();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Extensions/LocalText", false, 10)]
        public static void Create(MenuCommand menuCommand)
        {
            var res = Resources.Load("Prefabs/LocalText");
            GameObject go = Instantiate(res) as GameObject;
            go.name = "LocalText";
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Selection.activeObject = go;
        }
#endif 
    }
}
