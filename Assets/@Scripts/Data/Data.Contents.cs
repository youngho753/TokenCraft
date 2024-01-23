using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region TokenData
    [Serializable]
    public class TokenData
    {
        public int DataId;
        public string PrefabId;
        public string KoreanName;
        public string EnglishName;
        public string Value;
        public string Icon;
    }

    [Serializable]
    public class TokenDataLoader : ILoader<int, TokenData>
    {
        public List<TokenData> tokens = new List<TokenData>();
        public Dictionary<int, TokenData> MakeDict()
        {
            Dictionary<int, TokenData> dict = new Dictionary<int, TokenData>();
            foreach (TokenData token in tokens)
                dict.Add(token.DataId, token);
            return dict;
        }
    }
    #endregion
    
    #region ProductData
    [Serializable]
    public class ProductData
    {
        public int DataId;
        public int MakerId;
        public List<int> InputList;
        public int MakeId;
    }

    [Serializable]
    public class ProductLoader : ILoader<int, ProductData>
    {
        public List<ProductData> products = new List<ProductData>();
        public Dictionary<int, ProductData> MakeDict()
        {
            Dictionary<int, ProductData> dict = new Dictionary<int, ProductData>();
            foreach (ProductData product in products)
                dict.Add(product.DataId, product);
            return dict;
        }
    }
    #endregion

    #region ProductOutputData

    public class ProductOutputTableData
    {
        public int MakeId;
        public List<ProductOutputRateData> ProductOutputRateTable = new List<ProductOutputRateData>();
    }
    
    [Serializable]
    public class ProductOutputLoader : ILoader<int, ProductOutputTableData>
    {
        public List<ProductOutputTableData> ProductTable = new List<ProductOutputTableData>();
        public Dictionary<int, ProductOutputTableData> MakeDict()
        {
            Dictionary<int, ProductOutputTableData> dict = new Dictionary<int, ProductOutputTableData>();
            foreach (ProductOutputTableData product in ProductTable)
                dict.Add(product.MakeId, product);
            return dict;
        }
    }
    
    #endregion

    #region ProductOutputRateData

    public class ProductOutputRateData
    {
        public List<int> OutputList;
        public float Rate;
        public bool IsBaseItem;
    }
    

    #endregion

    #region MaterialtData
    [Serializable]
    public class MaterialData
    {
        public int DataId;
        public Define.MaterialType MaterialType;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;

    }

    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> Materials = new List<MaterialData>();
        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
            foreach (MaterialData mat in Materials)
                dict.Add(mat.DataId, mat);
            return dict;
        }
    }
    #endregion

    #region LevelData
    [Serializable]
    public class EquipmentLevelData
    {
        public int Level;
        public int UpgradCost;
        public int UpgradeRequiredItems;
    }

    [Serializable]
    public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
    {
        public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();
        public Dictionary<int, EquipmentLevelData> MakeDict()
        {
            Dictionary<int, EquipmentLevelData> dict = new Dictionary<int, EquipmentLevelData>();
            foreach (EquipmentLevelData levelData in levels)
                dict.Add(levelData.Level, levelData);
            return dict;
        }
    }
    #endregion

}