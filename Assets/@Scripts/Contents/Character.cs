using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본슬라임 스탯
public class Character 
{
    public Data.CreatureData Data;
    public int DataId { get; set; } = 1;
    public int Level { get; set; } = 1;
    public int MaxHp { get; set; } = 1;
    public int Atk { get; set; } = 1;
    public int Def { get; set; } = 1;
    public int TotalExp { get; set; } = 1;
    public float MoveSpeed { get; set; } = 1;
    public bool isCurrentCharacter = false;

    public void SetInfo(int key)
    {
        // DataId = key;
        // Data = Managers.Data.CreatureDic[key];
        
    }

    public void LevelUp()
    {
        // Level++;
        // Data = Managers.Data.CreatureDic[DataId];
    }


}