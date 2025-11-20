using UnityEngine;
using MagicSurvivors.Data;
using MagicSurvivors.Core;

namespace MagicSurvivors.Stage
{
    public class StageManager : MonoBehaviour
    {
        [Header("Stage Prefabs")]
        [SerializeField] private GameObject forestStagePrefab;
        [SerializeField] private GameObject caveStagePrefab;
        
        [Header("Mini-Dungeon Templates")]
        [SerializeField] private GameObject plazaTemplate;
        [SerializeField] private GameObject corridorTemplate;
        [SerializeField] private GameObject crossTemplate;
        [SerializeField] private GameObject mazeTemplate;
        [SerializeField] private GameObject objectHeavyTemplate;
        
        private GameObject currentStageObject;
        private StageType currentStage = StageType.Forest;
        
        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += InitializeStage;
                GameManager.Instance.OnStageTransition += TransitionToNextStage;
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= InitializeStage;
                GameManager.Instance.OnStageTransition -= TransitionToNextStage;
            }
        }
        
        private void InitializeStage()
        {
            LoadStage(StageType.Forest);
        }
        
        private void TransitionToNextStage(float time)
        {
            LoadStage(StageType.Cave);
        }
        
        private void LoadStage(StageType stageType)
        {
            if (currentStageObject != null)
            {
                Destroy(currentStageObject);
            }
            
            currentStage = stageType;
            
            switch (stageType)
            {
                case StageType.Forest:
                    if (forestStagePrefab != null)
                    {
                        currentStageObject = Instantiate(forestStagePrefab);
                        Debug.Log("StageManager: Loaded Forest stage");
                    }
                    break;
                    
                case StageType.Cave:
                    if (caveStagePrefab != null)
                    {
                        currentStageObject = Instantiate(caveStagePrefab);
                        Debug.Log("StageManager: Loaded Cave stage");
                    }
                    break;
            }
            
            SpawnRandomMiniDungeon();
        }
        
        private void SpawnRandomMiniDungeon()
        {
            GameObject[] templates = { plazaTemplate, corridorTemplate, crossTemplate, mazeTemplate, objectHeavyTemplate };
            GameObject selectedTemplate = templates[Random.Range(0, templates.Length)];
            
            if (selectedTemplate != null)
            {
                Vector3 spawnPosition = GetRandomDungeonPosition();
                Instantiate(selectedTemplate, spawnPosition, Quaternion.identity, currentStageObject.transform);
                Debug.Log("StageManager: Spawned mini-dungeon");
            }
        }
        
        private Vector3 GetRandomDungeonPosition()
        {
            float x = Random.Range(-20f, 20f);
            float y = Random.Range(-20f, 20f);
            return new Vector3(x, y, 0f);
        }
    }
}
