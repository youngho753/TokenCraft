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
using UnityEngine.Analytics;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/DeleteGameData ")]
    public static void DeleteGameData()
    {
        PlayerPrefs.DeleteAll();
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
            File.Delete(path);
    }
    
    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcel()
    {
        ParseTokenData("Token");
        ParseProductData("Product");
        ParseProductOutputData("ProductOutput");
        Debug.Log("Complete DataTransformer");
    }
    
    static void ParseTokenData(string filename)
    {
        TokenDataLoader loader = new TokenDataLoader();

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
            TokenData td = new TokenData();
            td.DataId = ConvertValue<int>(row[i++]);
            td.PrefabName = ConvertValue<string>(row[i++]);
            td.KoreanName = ConvertValue<string>(row[i++]);
            td.EnglishName = ConvertValue<string>(row[i++]);
            td.Value = ConvertValue<int>(row[i++]);
            td.Icon = ConvertValue<string>(row[i++]);
    
            loader.tokens.Add(td);
        }

        #endregion
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    
    static void ParseProductData(string filename)
    {
        ProductDataLoader loader = new ProductDataLoader();

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
            ProductData td = new ProductData();
            td.DataId = ConvertValue<int>(row[i++]);
            td.MakerId = ConvertValue<int>(row[i++]);
            td.InputList = ConvertList<int>(row[i++]);
            td.MakeOrder = ConvertValue<int>(row[i++]);
            td.MakeId = ConvertValue<int>(row[i++]);
    
            loader.products.Add(td);
        }

        #endregion
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    
    static void ParseProductOutputData(string filename)
    {
        Dictionary<int, List<ProductOutputRateData>> productOutputTable = ParseProductOutputRateData("ProductOutput");
        ProductOutputDataLoader loader = new ProductOutputDataLoader();

        #region ExcelData
        
        foreach(int key in productOutputTable.Keys)
        {
            ProductOutputTableData productOutputData = new ProductOutputTableData()
            {
                MakeId = key
            };
            if (productOutputTable.TryGetValue(productOutputData.MakeId, out List<ProductOutputRateData> productOutputRate))
                productOutputData.ProductOutputRateTable.AddRange(productOutputRate);

            loader.ProductTable.Add(productOutputData);
        }

        #endregion
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    
    static Dictionary<int, List<ProductOutputRateData>> ParseProductOutputRateData(string filename)
    {
        Dictionary<int, List<ProductOutputRateData>> productOutputTable = new Dictionary<int, List<ProductOutputRateData>>();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for(int y=1; y<lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;

            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            int makeId =  ConvertValue<int>(row[i++]);
            ProductOutputRateData rateData = new ProductOutputRateData()
            {
                Output = ConvertValue<int>(row[i++]),
                Rate = float.Parse(row[i++]),
                IsBaseItem = row[i++].Equals("true")
            };

            if (productOutputTable.ContainsKey(makeId) == false)
                productOutputTable.Add(makeId, new List<ProductOutputRateData>());

            productOutputTable[makeId].Add(rateData);
        }
        #endregion

        return productOutputTable;
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