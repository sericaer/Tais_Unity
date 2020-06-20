using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

[CustomEditor(typeof(LocalImage), true)]
public class CustomEditorImageEditor : Editor
{
    string text;

    public override void OnInspectorGUI()
    {
        var localImage = serializedObject.targetObject as LocalImage;
        text = localImage.imagePath;

        EditorGUI.BeginChangeCheck();
        text = EditorGUILayout.TextField("imagePath", text);
        if (EditorGUI.EndChangeCheck())
        {
            localImage.imagePath = text;

            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();

            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}
