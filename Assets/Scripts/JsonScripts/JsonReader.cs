using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JsonReader;

public class JsonReader : CustomSingleton<JsonReader>
{
    public TextAsset levelsJson;
    LevelsData levelsData;

    [System.Serializable]
    public class Level
    {
        public int level_number;
        public int total_move;
        public int row;
        public int column;
        public TargetObjective[] target_objectives;
    }

    [System.Serializable]
    public class TargetObjective
    {
        public int count;
        public string name;
    }

    [System.Serializable]
    public class LevelsData
    {
        public Level[] levels;
    }

    protected override void Awake()
    {
        base.Awake();
        try
        {
            string jsonContent = levelsJson.text;

            levelsData = JsonUtility.FromJson<LevelsData>(jsonContent);

            // Elde edilen verileri kullanarak istediðiniz iþlemleri gerçekleþtirebilirsiniz
            

        }
        catch (Exception ex)
        {
            Debug.LogError("Hata oluþtu: " + ex.Message);
        }
    }

    public Level GetLevelData(int LevelIndex)
    {
        return levelsData.levels[LevelIndex];
    }
}
