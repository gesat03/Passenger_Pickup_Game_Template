using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class LevelManager : BaseGamePhaseObserver
    {
        public static LevelManager Instance;

        public GridGenerator GridGen;
        public TransporterGenerator TransporterGen;
        public NPCGenerator NPCGen;
        public PassageGenerator PassageGen;

        public int CurrentLevel = 1;
        public Vector2Int CurrentLevelMapAxis;
        public Vector2Int CurrentLevelEditorAxis;

        [SerializeField] private LevelDataScriptableObject[] _levelData;
        public List<EditorCellData> BorderCellIndexs { get; private set; } // This variable need for NPC generation
        public List<EditorCellData> NormalCellIndexs { get; private set; } // This variable need for Transporter generation

        private List<int> _currentMapTransportersArray;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Initialization();
        }

        public void Initialization()
        {
            GridGen.Initialization();
            TransporterGen.Initialization();
            NPCGen.Initialization();
            PassageGen.Initialization();
        }

        public override void LevelPreparingPhase()
        {
            base.LevelPreparingPhase();

            ClearLevel();
        }

        public override void StartPhase()
        {
            base.StartPhase();

            LevelGeneration();
        }

        public override void LevelWinPhase()
        {
            base.LevelWinPhase();

            CurrentLevel ++;
        }

        [ContextMenu("LevelGeneration")]
        public void LevelGeneration()
        {
            LevelChecker();

            GetLevelData(_levelData[CurrentLevel - 1]);

            GridGen.CreateGridMap(CurrentLevelMapAxis.y, CurrentLevelMapAxis.x);
            TransporterGen.GetTransportersPlacementDataFromEditorData(NormalCellIndexs);
            NPCGen.GetNPCPlacementDataFromEditorData(BorderCellIndexs);
        }

        [ContextMenu("ClearLevel")]
        public void ClearLevel()
        {
            NPCGen.ReleaseAllNPCs();
            PassageGen.ReleaseAllPassages();
            TransporterGen.ReleaseAllTransporters();
            GridGen.ReleaseAllGrids();
        }

        public void LevelChecker()
        {
            if(CurrentLevel > _levelData.Length)
            {
                CurrentLevel = 1;
            }
        }

        private void GetLevelData(LevelDataScriptableObject levelData)
        {
            BorderCellIndexs = new List<EditorCellData>();
            NormalCellIndexs = new List<EditorCellData>();

            CurrentLevelEditorAxis.x = levelData.gridSizeX;
            CurrentLevelEditorAxis.y = levelData.gridSizeY;

            CurrentLevelMapAxis.x = levelData.gridSizeX - 2;
            CurrentLevelMapAxis.y = levelData.gridSizeY - 2;

            for (int i = 0; i < levelData.cells.Length; i++)
            {
                if (levelData.cells[i].IsClosed)
                {
                    continue;
                }
                else if (levelData.cells[i].IsBorder)
                {
                    if(!levelData.cells[i].IsSelected) { continue; }
                    BorderCellIndexs.Add(levelData.cells[i]);
                }
                else
                {
                    if (!levelData.cells[i].IsSelected) { continue; }
                    NormalCellIndexs.Add(levelData.cells[i]);
                }
            }
        }
    }
}
