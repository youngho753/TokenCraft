using Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ObjectManager
{
    public HashSet<TokenController> Tokens { get; } = new HashSet<TokenController>();
    
    public Transform SkillTransform
    {
        get
        {
            GameObject root = GameObject.Find("@Skill");
            if (root == null)
            {
                root = new GameObject { name = "@Skill" };
            }
            return root.transform;
        }
    }

    public Transform MonsterTransform
    {
        get
        {
            GameObject root = GameObject.Find("@Monster");
            if (root == null)
                root = new GameObject { name = "@Monster" };
            return root.transform;
        }
    }
    public Transform TextFontTransform
    {
        get
        {
            GameObject root = GameObject.Find("@TextFont");
            if (root == null)
            {
                root = new GameObject { name = "@TextFont" };
            }
            return root.transform;
        }
    }

    public ObjectManager()
    {
        Init();
    }

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public string TestMapId = "Map_01";
    public void LoadMap(string mapName)
    {
        GameObject objMap = Managers.Resource.Instantiate(TestMapId);
        objMap.transform.position = Vector3.zero;
        objMap.name = "@Map";

        // objMap.GetComponent<Map>().Init();
    }

    public T SpawnToken<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : TokenController
    {
        System.Type type = typeof(T);
        
        
        Stack<TokenController> tokenStack = new Stack<TokenController>();

       if (type == typeof(MaterialTokenController))
       {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            MaterialTokenController cc = go.GetOrAddComponent<MaterialTokenController>();
            go.transform.position = position;
            Managers.Game._tokenIndex.Add(cc);

            int pkGroupNum = Managers.Game._tokenIndex.Count; 
            cc.pkGroupNum = pkGroupNum;
            cc.groupNum = pkGroupNum;
            tokenStack.Push(cc);
            
            
            Managers.Game._tokenStackDic.Add(cc.pkGroupNum,tokenStack);
            Tokens.Add(cc);

            return cc as T;
       } else if (type == typeof(NatureTokenController))
       {
           GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
           NatureTokenController cc = go.GetOrAddComponent<NatureTokenController>();
           go.transform.position = position;
           Managers.Game._tokenIndex.Add(cc);

           int pkGroupNum = Managers.Game._tokenIndex.Count; 
           cc.pkGroupNum = pkGroupNum;
           cc.groupNum = pkGroupNum;
           tokenStack.Push(cc);
           Managers.Game._tokenStackDic.Add(cc.pkGroupNum,tokenStack);
           Tokens.Add(cc);
           
           for (int i = 0; i <  cc.TokenBackground.maxTokenCnt; i++)
           {
               BlankTokenController btc = Managers.Object.SpawnToken<BlankTokenController>(cc.TokenBackground.transform.position + new Vector3(-0.6f + (1.2f * i), 0.2f, 0),0,"BlankToken");
               btc._backgroundOrder = i;
               cc.TokenBackground.BlankTokenDic.Add(i,btc);
           }
           return cc as T;
       } else if (type == typeof(BlankTokenController))
       {
           
           GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
           BlankTokenController btc = go.GetOrAddComponent<BlankTokenController>();
           go.transform.position = position;
           Managers.Game._tokenIndex.Add(btc);

           int pkGroupNum = Managers.Game._tokenIndex.Count; 
           btc.pkGroupNum = pkGroupNum;
           btc.groupNum = pkGroupNum;
           tokenStack.Push(btc);
           Managers.Game._tokenStackDic.Add(btc.pkGroupNum,tokenStack);
           Tokens.Add(btc);
           
           return btc as T;
       }
       
        return null;
    }
    
    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(TokenController))
        {
            Tokens.Remove(obj as TokenController);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }
}
