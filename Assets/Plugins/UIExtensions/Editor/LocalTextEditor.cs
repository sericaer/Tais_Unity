using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

[CustomEditor(typeof(LocalText), true)]
public class CustomEditorTestEditor : Editor
{
    string text;
    //bool isAutoSize;
    public override void OnInspectorGUI()
    {
        var localText = serializedObject.targetObject as LocalText;
        text = localText.format;
        //isAutoSize = localText.isAutoSize;

        EditorGUI.BeginChangeCheck();
        text = EditorGUILayout.TextField("format", text);
        //isAutoSize = EditorGUILayout.Toggle("autosize", isAutoSize);

        if (EditorGUI.EndChangeCheck())
        {
            localText.format = text;
            //localText.isAutoSize = isAutoSize;

            //localText.RefreshSize();

            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();

            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}
