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
    public Dictionary<int, Data.TokenData> TokenDic { get; private set; } = new Dictionary<int, Data.TokenData>();
    public Dictionary<int, Data.ProductData> ProductDic { get; private set; } = new Dictionary<int, Data.ProductData>();
    public Dictionary<int, Data.ProductOutputData> ProductOutputTableDic { get; private set; } = new Dictionary<int, Data.ProductOutputData>();
    
    public void Init()
    {
        TokenDic = LoadJson<Data.TokenDataLoader, int, Data.TokenData>("TokenData").MakeDict();
        ProductDic = LoadJson<Data.ProductDataLoader, int, Data.ProductData>("ProductData").MakeDict();
        ProductOutputTableDic = LoadJson<Data.ProductOutputDataLoader, int, Data.ProductOutputData>("ProductOutputData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
	}


}
