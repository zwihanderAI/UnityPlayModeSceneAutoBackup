using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class AutoSaveSceneOnPlay
{
    private const string BACKUP_DIR = "Assets/PlayModeAutoBackup/";
    static AutoSaveSceneOnPlay()
    {
        EditorApplication.playModeStateChanged += PlayModeChange;
        Debug.Log("[PlayModeAutoBackup]Script Loaded");
    }

    private static void PlayModeChange(PlayModeStateChange PlayMode)
    {
        if (PlayMode == PlayModeStateChange.ExitingEditMode)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string backupFullPath = BACKUP_DIR + currentScene.name + ".unity";

            //バックアップフォルダが存在しない場合作成
            if (!System.IO.Directory.Exists(BACKUP_DIR))
            {
                System.IO.Directory.CreateDirectory(BACKUP_DIR);
            }

            //シーン保存
            EditorSceneManager.SaveScene(currentScene, backupFullPath);
            Debug.Log("[PlayModeAutoBackup]Success\n"
                + "<color=green>" + backupFullPath + "</color>");
        }
    }
}