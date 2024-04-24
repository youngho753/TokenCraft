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
    public HashSet<ProductController> Products { get; } = new HashSet<ProductController>();

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
    
    public void LoadMap(string mapName)
    {
        GameObject objMap = Managers.Resource.Instantiate(mapName);
        objMap.transform.position = Vector3.zero;
        objMap.name = "@Map";

        objMap.GetComponent<Map>().Init();
    }

    public T SpawnToken<T>(Vector3 position, int templateID = 0, string prefabName = "", bool isRandomSpawn = false) where T : TokenController
    {
        System.Type type = typeof(T);
        
        if (isRandomSpawn)
        {
            position = new Vector3(Util.GetRandomFloatValue(-6.5f, 6.5f), Util.GetRandomFloatValue(-2.5f, 4f), 0);
        }
        
       if (type == typeof(MaterialTokenController))
       {
            GameObject go = Managers.Resource.Instantiate(Managers.Data.TokenDic[templateID].PrefabName, pooling: true);
            MaterialTokenController cc = go.GetOrAddComponent<MaterialTokenController>();
            if (isRandomSpawn) cc.FloatValue = 5f;
            cc.Position = position + new Vector3(0, cc.FloatValue, 0);;
            cc.SetInfo(templateID);

            Tokens.Add(cc);

            cc.tokenOrder = Tokens.Count;

            return cc as T;
       // } else if (type == typeof(NatureTokenController))
       // {
       //     GameObject go = Managers.Resource.Instantiate(Managers.Data.TokenDic[templateID].PrefabName, pooling: true);
       //     NatureTokenController cc = go.GetOrAddComponent<NatureTokenController>();
       //     cc.Position = position;
       //     cc.SetInfo(templateID);
       //
       //     Tokens.Add(cc);
       //     
       //     cc.tokenOrder = Tokens.Count;
       //     
       //     return cc as T;
       } 
       
        return null;
    }
    
    public T SpawnProduct<T>(Vector3 position, int templateID = 0, string prefabName = "", bool isRandomSpawn = false) where T : ProductController
    {
        System.Type type = typeof(T);
        
        if (isRandomSpawn)
        {
            position = new Vector3(Util.GetRandomFloatValue(-6.5f, 6.5f), Util.GetRandomFloatValue(-2.5f, 4f), 0);
        }
        
        if (type == typeof(FactoryProductController))
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Data.ProductDic[templateID].PrefabName, pooling: true);
            FactoryProductController fpc = go.GetOrAddComponent<FactoryProductController>();
            if (isRandomSpawn) fpc.FloatValue = 5f;
            fpc.Position = position + new Vector3(0, fpc.FloatValue, 0);
            fpc.SetInfo(templateID);

            Products.Add(fpc);

            return fpc as T;
            // } else if (type == typeof(NatureTokenController))
            // {
            //     GameObject go = Managers.Resource.Instantiate(Managers.Data.TokenDic[templateID].PrefabName, pooling: true);
            //     NatureTokenController cc = go.GetOrAddComponent<NatureTokenController>();
            //     cc.Position = position;
            //     cc.SetInfo(templateID);
            //
            //     Tokens.Add(cc);
            //     
            //     cc.tokenOrder = Tokens.Count;
            //     
            //     return cc as T;
        } 
       
        return null;
    }
    
    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);
        
        // if (type == typeof(BlankZoneController))
        // {
        //     GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        //     BlankZoneController bzc = go.GetOrAddComponent<BlankZoneController>();
        //     go.transform.position = position;
        //
        //     BlankZoneControllers.Add(bzc);
        //    
        //     return bzc as T;
        // }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(MaterialTokenController))
        {
            Tokens.Remove(obj as MaterialTokenController);
            Managers.Resource.Destroy(obj.gameObject);
        }else if (type == typeof(MaterialTokenController))
        {
            Tokens.Remove(obj as MaterialTokenController);
            Managers.Resource.Destroy(obj.gameObject);
        }else if (type == typeof(NatureProductController))
            
            Products.Remove(obj as NatureProductController);
            Managers.Resource.Destroy(obj.gameObject);
        // }else if (type == typeof(FactoryTokenController))
        // {
        //     Tokens.Remove(obj as FactoryTokenController);
        //     Managers.Resource.Destroy(obj.gameObject);
    }
}
