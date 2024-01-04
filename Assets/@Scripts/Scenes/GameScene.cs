using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Diagnostics;

public class GameScene : BaseScene
{
    GameManager _game;

    bool isGameEnd = false;

    #region Action
    #endregion

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        
        
        Debug.Log("@>> GameScene Init()");
        base.Init();
        SceneType = Define.Scene.GameScene;
        _game = Managers.Game;

        //1. 맵 생성
        // LoadStage();

        //2. 플레이어 생성
        // Managers.Resource.Instantiate("player", null, false);
        // _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, 201000);
        
        //3. GameManager 세팅
        
        Managers.Game.Mouse = FindObjectOfType<MouseController>();
        Managers.Game.CameraController = FindObjectOfType<CameraController>();
        
        //4. 의존성주입
        
        
        

    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TokenController _tc = Managers.Object.SpawnToken<MaterialTokenController>(Managers.Game.Mouse._mousePosition, 0, "MaterialToken");
            
        }else if (Input.GetKeyDown(KeyCode.F3))
        {
            Stack<TokenController> tokenStack = new Stack<TokenController>();

            TokenController lowestToken = Managers.Object.SpawnToken<MaterialTokenController>(Managers.Game.Mouse._mousePosition, 0, "MaterialToken");
            tokenStack.Push(lowestToken);

            TokenController tc;
            for(int i=1; i<10; i++){
                tc = Managers.Object.SpawnToken<MaterialTokenController>(Managers.Game.Mouse._mousePosition, 0, "MaterialToken");
                Util.SettingTokenStack(null, false, tc);
                
                tokenStack.Push(tc);
            }

            Util.SettingTokenStack(tokenStack);
            // Managers.Game._tokenStackDic.Add(lowestToken.pkGroupNum,tokenStack);
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Stack<TokenController> tokenStack = new Stack<TokenController>();

            TokenController _tc = Managers.Object.SpawnToken<NatureTokenController>(Managers.Game.Mouse._mousePosition, 0, "ProductToken");
            tokenStack.Push(_tc);
            
            Managers.Game._tokenIndex.Add(_tc);
            _tc.groupNum = Managers.Game._tokenIndex.Count;
            _tc.pkGroupNum = Managers.Game._tokenIndex.Count;
            Managers.Game._tokenStackDic.Add(_tc.pkGroupNum,tokenStack);
            
            Debug.Log(_tc.groupNum);
        }
        
        if (isGameEnd == true)
            return;
    }

    public void LoadStage()
    {
        // if (_spawningPool == null)
        //     _spawningPool = gameObject.AddComponent<SpawningPool>();
        //
        //
        // Managers.Object.LoadMap(_game.CurrentStageData.MapName);
        //
        // _game.CurrentStageIndex = _game.CurrentStageData.StageIndex;
        // _game.CurrentWaveIndex = 0;
        //
        // // 웨이브 정보 적용
        // StopAllCoroutines();
        // StartCoroutine(StartWave(_game.CurrentStageData.WaveArray[_game.CurrentWaveIndex]));

    }

    void GameOver()
    {
        // 스테이지 종료 처리
        // ex) 게임 결과 UI 표시, 스테이지 선택 UI로 돌아감 등
        // UI_GameoverPopup gp = Managers.UI.ShowPopupUI<UI_GameoverPopup>();
        // gp.SetInfo();
    }

    public override void Clear()
    {

    }

}