using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
    public class LocalImage : MonoBehaviour
    {
        public static Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();

        public static void LoadImage(string imagePath)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(imagePath);

            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);

            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            dict.Add(imagePath, sprite);
        }

        [SerializeField]
        string _imagePath;

        public string imagePath
        {
            get
            {
                return _imagePath;

            }
            set
            {
                _imagePath = value;
                Refresh();
            }
        }

        private Sprite Load(string imagePath)
        {
            return null;

            //var path = sceneLoad.modsPath + "/Image/" + imagePath;
            //if (!File.Exists(path))
            //{
            //    path = sceneLoad.modsPath + "/native/Image/" + imagePath;
            //    if (!File.Exists(path))
            //    {
            //        Debug.Log("can not find:" + imagePath);
            //        return null;
            //    }
            //}

            //if (!dict.ContainsKey(path))
            //{
            //    LoadImage(path);
            //}

            //return dict[path];


        }

        public void Refresh()
        {
            var sprite = Load(_imagePath);
            if(sprite == null)
            { 
                return;
            }

            GetComponent<Image>().sprite = sprite;
            GetComponent<Image>().type = Image.Type.Simple;
            GetComponent<Image>().SetNativeSize();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Extensions/LocalImage", false, 10)]
        public static void Create(MenuCommand menuCommand)
        {
            var res = Resources.Load("Prefabs/LocalImage");
            GameObject go = Instantiate(res) as GameObject;
            go.name = "LocalImage";
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Selection.activeObject = go;
        }
#endif 
    }
}
