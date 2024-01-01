using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // public Dictionary<int, Data.MaterialData> MaterialDic { get; private set; } = new Dictionary<int, Data.MaterialData>();
    // public Dictionary<int, Data.SupportSkillData> SupportSkillDic { get; private set; } = new Dictionary<int, Data.SupportSkillData>();
    // public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
    // public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    // public Dictionary<int, Data.LevelData> LevelDataDic { get; private set; } = new Dictionary<int, Data.LevelData>();
    // public Dictionary<string, Data.EquipmentData> EquipDataDic { get; private set; } = new Dictionary<string, Data.EquipmentData>();
    // public Dictionary<int, Data.EquipmentLevelData> EquipLevelDataDic { get; private set; } = new Dictionary<int, Data.EquipmentLevelData>();
    
    public void Init()
    {
        // MaterialDic = LoadJson<Data.MaterialDataLoader, int, Data.MaterialData>("MaterialData").MakeDict();
        // SupportSkillDic = LoadJson<Data.SupportSkillDataLoader, int, Data.SupportSkillData>("SupportSkillData").MakeDict();
        // StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData").MakeDict();
        // SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        // LevelDataDic = LoadJson<Data.LevelDataLoader, int, Data.LevelData>("LevelData").MakeDict();
        // EquipDataDic = LoadJson<Data.EquipmentDataLoader, string, Data.EquipmentData>("EquipmentData").MakeDict();
        // EquipLevelDataDic = LoadJson<Data.EquipmentLevelDataLoader, int, Data.EquipmentLevelData>("EquipmentLevelData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
	}


}
