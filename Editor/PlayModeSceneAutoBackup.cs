using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class AutoSaveSceneOnPlay
{
    private const string BACKUP_DIR = "Assets/PlayModeSceneAutoBackup/";
    private const string TEMP_SCENE_PATH = "Assets/PlayModeSceneAutoBackup/__temp_backup_scene.unity";
    static AutoSaveSceneOnPlay()
    {
        EditorApplication.playModeStateChanged += PlayModeChange;
        Debug.Log("[PlayModeAutoBackup]Script Loaded");
    }

    private static void PlayModeChange(PlayModeStateChange PlayMode)
    {
        if (PlayMode == PlayModeStateChange.ExitingEditMode)
        {
            //バックアップフォルダが存在しない場合作成
            if (!System.IO.Directory.Exists(BACKUP_DIR))
            {
                System.IO.Directory.CreateDirectory(BACKUP_DIR);
            }

            //シーン保存            
            Scene currentScene = SceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(currentScene); //現在開いているシーンを上書き保存
            string currentScenePath = currentScene.path; //現在開いているシーンのパス取得(tempから帰ってくるため必要)
            string backupFullPath = BACKUP_DIR + currentScene.name + "_AutoBackup.unity";

            // ① 現在のシーンを一時保存（元シーンはそのまま）
            EditorSceneManager.SaveScene(currentScene, TEMP_SCENE_PATH);

            // ② tempシーンを開く
            Scene tempScene = EditorSceneManager.OpenScene(TEMP_SCENE_PATH, OpenSceneMode.Single);

            // ③ バックアップとして保存
            EditorSceneManager.SaveScene(tempScene, backupFullPath);

            // ④ 元のシーンを再度開く
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);

            // ⑤ tempシーン削除
            AssetDatabase.DeleteAsset(TEMP_SCENE_PATH);

            Debug.Log("[PlayModeAutoBackup]Success\n"
                + "<color=green>" + backupFullPath + "</color>");
        }
    }
}