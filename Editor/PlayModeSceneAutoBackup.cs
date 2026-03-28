using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayModeSceneAutoBackup
{
    private const string BACKUP_DIR = "Assets/PlayModeSceneAutoBackup/";

    static PlayModeSceneAutoBackup()
    {
        EditorApplication.playModeStateChanged += PlayModeChange;
        Debug.Log("[PlayModeSceneAutoBackup]Script Loaded");
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
            string currentScenePath = currentScene.path; //現在開いているシーンのパス取得(別名保存から帰ってくるため必要)
            string backupFullPath = BACKUP_DIR + currentScene.name + "_AutoBackup.unity";

            //バックアップ保存
            EditorSceneManager.SaveScene(currentScene, backupFullPath);
            //元のシーンを再度開く
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);

            Debug.Log("[PlayModeSceneAutoBackup]Success\n"
                + "<color=green>" + backupFullPath + "</color>");
        }
    }
}