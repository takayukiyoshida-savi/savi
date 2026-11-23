using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PetrifiedVillage.EditorTools
{
    public static class PetrifiedVillageMenu
    {
        private const string MenuRoot = "Tools/Field/砂漠の石化した村/";

        [MenuItem(MenuRoot + "生成（カメラ・太陽・フォグ込み）")]
        private static void GenerateWithEnvironment()
        {
            Generate(true);
        }

        [MenuItem(MenuRoot + "生成（フィールドのみ）")]
        private static void GenerateFieldOnly()
        {
            Generate(false);
        }

        private static void Generate(bool withEnvironment)
        {
            GameObject existing = GameObject.Find(PetrifiedVillageBuilder.RootName);
            if (existing != null)
            {
                bool replace = EditorUtility.DisplayDialog(
                    "砂漠の石化した村",
                    "シーンに生成済みの村があります。作り直しますか？\n（手で調整した配置は失われます）",
                    "作り直す",
                    "キャンセル");
                if (!replace)
                {
                    return;
                }

                Undo.DestroyObjectImmediate(existing);
            }

            GameObject root = PetrifiedVillageBuilder.Build(withEnvironment);
            Undo.RegisterCreatedObjectUndo(root, "Generate Petrified Village");
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Selection.activeGameObject = root;
        }
    }
}
