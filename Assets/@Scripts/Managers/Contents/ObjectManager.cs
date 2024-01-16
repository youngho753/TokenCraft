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
    public HashSet<BlankZoneController> BlankZoneControllers { get; } = new HashSet<BlankZoneController>();
    
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
            cc.position = position;

            tokenStack.Push(cc);
            
            Tokens.Add(cc);

            return cc as T;
       } else if (type == typeof(NatureTokenController))
       {
           GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
           NatureTokenController cc = go.GetOrAddComponent<NatureTokenController>();
           cc.position = position;

           tokenStack.Push(cc);
           Tokens.Add(cc);
           
           return cc as T;
       } 
       
        return null;
    }
    
    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);
        
        if (type == typeof(BlankZoneController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            BlankZoneController bzc = go.GetOrAddComponent<BlankZoneController>();
            go.transform.position = position;

            BlankZoneControllers.Add(bzc);
           
            return bzc as T;
        }

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
