using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorMenu
{
    private const string TOOLS_MENU_PATH = "Tools/";

    [MenuItem(TOOLS_MENU_PATH + "테이블 코드 생성")]
    public static void GenerateTableCode()
    {
        if (IsPlayingMode()) return;

        try
        {
            DataManager.GenDatatable();
            DataManager.GenConfigTable();
            DataManager.GenTableEnum();
            Debug.Log("테이블 코드가 성공적으로 생성되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"테이블 코드 생성 중 오류 발생: {e.Message}");
        }
    }

    [MenuItem(TOOLS_MENU_PATH + "유저 데이터 초기화")]
    public static void ClearUserData()
    {
        if (IsPlayingMode()) return;

        try
        {
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
            int deletedFiles = 0;

            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
                deletedFiles++;
            }

            Debug.Log($"유저 데이터 초기화 완료: {deletedFiles}개 파일 삭제됨");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"유저 데이터 초기화 중 오류 발생: {e.Message}");
        }
    }

    private static bool IsPlayingMode()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("플레이 모드에서는 이 작업을 수행할 수 없습니다.");
            return true;
        }
        return false;
    }
}
