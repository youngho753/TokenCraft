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
            td.Value = ConvertValue<int>(row[i++]);
            aps.AccountLevel = ConvertValue<int>(row[i++]);
            aps.FreeRewardItemId = ConvertValue<int>(row[i++]);
            aps.FreeRewardItemValue = ConvertValue<int>(row[i++]);
            aps.RareRewardItemId = ConvertValue<int>(row[i++]);
            aps.RareRewardItemValue = ConvertValue<int>(row[i++]);
            aps.EpicRewardItemId = ConvertValue<int>(row[i++]);
            aps.EpicRewardItemValue = ConvertValue<int>(row[i++]);


            loader.accounts.Add(aps);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
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