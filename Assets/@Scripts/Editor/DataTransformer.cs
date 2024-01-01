using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Linq;
using UnityEditor.AddressableAssets;
using Unity.Plastic.Newtonsoft.Json;
using Data;
using System.ComponentModel;
using static Define;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/DeleteGameData ")]
    public static void DeleteGameData()
    {
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
            File.Delete(path);
    }

    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcel()
    {
        ParseSkillData("Skill");
        ParseStageData("Stage");
        ParseCreatureData("Creature");
        ParseLevelData("Level");
        ParseEquipmentLevelData("EquipmentLevel");
        ParseEquipmentData("Equipment");
        ParseMaterialData("Material");
        //ParseBossData("Boss");
        //ParseChapterResourceData("ChapterResource");
    }

    static void ParseSkillData(string filename)
    {
        SkillDataLoader loader = new SkillDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            SkillData skillData = new SkillData();
            skillData.DataId = ConvertValue<int>(row[i++]);
            skillData.Name = ConvertValue<string>(row[i++]);
            skillData.Description = ConvertValue<string>(row[i++]);
            skillData.IconLabel = ConvertValue<string>(row[i++]);
            skillData.PrefabLabel = ConvertValue<string>(row[i++]);
            skillData.SoundLabel = ConvertValue<string>(row[i++]);
            skillData.Category = ConvertValue<string>(row[i++]);
            skillData.CoolTime = ConvertValue<float>(row[i++]);
            skillData.DamageMultiplier = ConvertValue<float>(row[i++]);
            skillData.ProjectileSpacing = ConvertValue<float>(row[i++]);
            skillData.Duration = ConvertValue<float>(row[i++]);
            skillData.RecognitionRange = ConvertValue<float>(row[i++]);
            skillData.NumProjectiles = ConvertValue<int>(row[i++]);
            skillData.CastingSound = ConvertValue<string>(row[i++]);
            skillData.AngleBetweenProj = ConvertValue<float>(row[i++]);
            skillData.AttackInterval = ConvertValue<float>(row[i++]);
            skillData.NumBounce = ConvertValue<int>(row[i++]);
            skillData.BounceSpeed = ConvertValue<float>(row[i++]);
            skillData.BounceDist = ConvertValue<float>(row[i++]);
            skillData.NumPenerations = ConvertValue<int>(row[i++]);
            skillData.CastingEffect = ConvertValue<int>(row[i++]);
            skillData.ProbCastingEffect = ConvertValue<float>(row[i++]);
            skillData.HitEffect = ConvertValue<int>(row[i++]);
            skillData.HitSound = ConvertValue<string>(row[i++]);
            skillData.ProbHitEffect = ConvertValue<float>(row[i++]);
            skillData.ProjRange = ConvertValue<float>(row[i++]);
            skillData.MinCoverage = ConvertValue<float>(row[i++]);
            skillData.MaxCoverage = ConvertValue<float>(row[i++]);
            skillData.RoatateSpeed = ConvertValue<float>(row[i++]);
            skillData.ProjSpeed = ConvertValue<float>(row[i++]);
            skillData.ScaleMultiplier = ConvertValue<float>(row[i++]);
            loader.skills.Add(skillData);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

   
    static void ParseStageData(string filename)
    {
        Dictionary<int, List<WaveData>> waveTable = ParseWaveData("Wave");
        StageDataLoader loader = new StageDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            StageData stageData = new StageData();
            stageData.StageIndex = ConvertValue<int>(row[i++]);
            stageData.StageName = ConvertValue<string>(row[i++]);
            stageData.StageLevel = ConvertValue<int>(row[i++]);
            stageData.MapName = ConvertValue<string>(row[i++]);
            stageData.StageSkill = ConvertValue<int>(row[i++]);
            stageData.ClearReward = ConvertValue<string>(row[i++]);
            stageData.FirstClearReward = ConvertValue<string>(row[i++]);
            stageData.StageImage = ConvertValue<string>(row[i++]);
            stageData.AppearingMonsters = ConvertList<int>(row[i++]);
            waveTable.TryGetValue(stageData.StageIndex, out stageData.WaveArray);

            loader.stages.Add(stageData);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseCreatureData(string filename)
    {
        CreatureDataLoader loader = new CreatureDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            CreatureData cd = new CreatureData();
            cd.DataId = ConvertValue<int>(row[i++]);
            cd.DescriptionTextID = ConvertValue<string>(row[i++]);
            cd.PrefabLabel = ConvertValue<string>(row[i++]);
            // cd.SkillTypeList = ConvertList<int>(row[i++]);
            loader.creatures.Add(cd);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseLevelData(string filename)
    {
        LevelDataLoader loader = new LevelDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            LevelData data = new LevelData();
            data.Level = ConvertValue<int>(row[i++]);
            data.TotalExp = ConvertValue<int>(row[i++]);
            loader.levels.Add(data);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseEquipmentLevelData(string filename)
    {
        EquipmentLevelDataLoader loader = new EquipmentLevelDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            EquipmentLevelData data = new EquipmentLevelData();
            data.Level = ConvertValue<int>(row[i++]);
            data.UpgradCost = ConvertValue<int>(row[i++]);
            data.UpgradeRequiredItems = ConvertValue<int>(row[i++]);

            loader.levels.Add(data);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseEquipmentData(string filename)
    {
        EquipmentDataLoader loader = new EquipmentDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            EquipmentData ed = new EquipmentData();
            ed.DataId = ConvertValue<string>(row[i++]);
            ed.GachaRarity = ConvertValue<GachaRarity>(row[i++]);
            ed.EquipmentType = ConvertValue<EquipmentType>(row[i++]);
            ed.EquipmentGrade = ConvertValue<EquipmentGrade>(row[i++]);
            ed.NameTextID = ConvertValue<string>(row[i++]);
            ed.DescriptionTextID = ConvertValue<string>(row[i++]);
            ed.SpriteName = ConvertValue<string>(row[i++]);
            ed.MaxHpBonus = ConvertValue<int>(row[i++]);
            ed.MaxHpBonusPerUpgrade = ConvertValue<int>(row[i++]);
            ed.AtkDmgBonus = ConvertValue<int>(row[i++]);
            ed.AtkDmgBonusPerUpgrade = ConvertValue<int>(row[i++]);
            ed.MaxLevel = ConvertValue<int>(row[i++]);
            ed.UncommonGradeSkill = ConvertValue<int>(row[i++]);
            ed.RareGradeSkill = ConvertValue<int>(row[i++]);
            ed.EpicGradeSkill = ConvertValue<int>(row[i++]);
            ed.LegendaryGradeSkill = ConvertValue<int>(row[i++]);
            ed.MythGradeSkill = ConvertValue<int>(row[i++]);
            loader.Equipments.Add(ed);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseMaterialData(string filename)
    {
        MaterialDataLoader loader = new MaterialDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            MaterialData material = new MaterialData();
            material.DataId = ConvertValue<int>(row[i++]);
            material.MaterialType = ConvertValue<Define.MaterialType>(row[i++]);
            material.NameTextID = ConvertValue<string>(row[i++]);
            material.SpriteName = ConvertValue<string>(row[i++]);

            loader.Materials.Add(material);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static Dictionary<int, List<WaveData>> ParseWaveData(string filename)
    {
        Dictionary<int, List<WaveData>> waveTable = new Dictionary<int, List<WaveData>>();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;

            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            //int respawnID = ConvertValue<int>(row[i++]);
            WaveData waveData = new WaveData();
            waveData.StageIndex = ConvertValue<int>(row[i++]);
            waveData.WaveIndex = ConvertValue<int>(row[i++]);
            waveData.SpawnInterval = ConvertValue<float>(row[i++]);
            waveData.OnceSpawnCount = ConvertValue<int>(row[i++]);
            waveData.MonsterId = ConvertList<int>(row[i++]);
            waveData.EleteId = ConvertList<int>(row[i++]);
            waveData.BossId = ConvertList<int>(row[i++]);
            waveData.RemainsTime = ConvertValue<float>(row[i++]);
            waveData.FirstMonsterSpawnRate = ConvertValue<float>(row[i++]);
            waveData.HpIncreaseRate = ConvertValue<float>(row[i++]);
            waveData.nonDropRate = ConvertValue<float>(row[i++]);
            waveData.SmallGemDropRate = ConvertValue<float>(row[i++]);
            waveData.GreenGemDropRate = ConvertValue<float>(row[i++]);
            waveData.BlueGemDropRate = ConvertValue<float>(row[i++]);
            waveData.YellowGemDropRate = ConvertValue<float>(row[i++]);

            if (waveTable.ContainsKey(waveData.StageIndex) == false)
                waveTable.Add(waveData.StageIndex, new List<WaveData>());

            waveTable[waveData.StageIndex].Add(waveData);
        }
        #endregion

        return waveTable;
    }

    public static T ConvertValue<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }

    public static List<T> ConvertList<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new List<T>();

        return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
    }
#endif

}