using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Define;
using Random = UnityEngine.Random;
[Serializable]
public class GameData
{
    public int CurrentStageIndex = 1;
    public int CurrentWaveIndex = 1;
    public int UserLevel = 1;
    public string UserName = "Player";
    public int InGameLevel = 1;
    public float InGameExp = 0;
    public int Stamina = 30;
    public int Gold = 0;
    public int Dia = 0;
    public int NumDeadMonsters = 0;
    public float SoulCount = 0;
    public TokenController OnClickToken;

}
public class GameManager
{
    #region TokenControl
    public TokenController OnClickToken
    {
        get { return _gameData.OnClickToken; }
        set { _gameData.OnClickToken = value; }
    }
    
    #endregion
    
    #region GameData
    public GameData _gameData = new GameData();
    public GameData SaveData
    {
        get { return _gameData; }
        set
        {
            _gameData = value;
            SaveGame();
        }
    }
    
    public float TimeRemaining { get; set; } = 30;
    #endregion

    #region Action

    #endregion
    
    public CameraController CameraController { get; set; }

    #region CurrentStage, Map
    public StageData CurrentStageData { get; set; }
    public WaveData CurrentWaveData { get; set; }
    #endregion
    
    
    #region Mouse
    
    public MouseController Mouse { get; set; }

    #endregion
    
    #region Save&Load
    string _path;
    public void SaveGame()
    {

        //windows -> packagemanager 이동후 [add package by name] 선택 후 com.unity.nuget.newtonsoft-json 입력
        // string jsonStr = JsonConvert.SerializeObject(Managers.Game.SaveData);

        // File.WriteAllText(_path, jsonStr);
    }

    public bool LoadGame()
    {
        return false;
    }
  
    #endregion
}

