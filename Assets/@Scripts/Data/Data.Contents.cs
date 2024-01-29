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
        public string PrefabName;
        public string KoreanName;
        public string EnglishName;
        public int Value;
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
        public int MakeOrder;
        public int MakeId;
    }

    [Serializable]
    public class ProductDataLoader : ILoader<int, ProductData>
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

    [Serializable]
    public class ProductOutputData
    {
        public int MakeId;
        public List<ProductOutputRateData> ProductOutputRateTable = new List<ProductOutputRateData>();
    }
    
    [Serializable]
    public class ProductOutputDataLoader : ILoader<int, ProductOutputData>
    {
        public List<ProductOutputData> productOutputs = new List<ProductOutputData>();
        public Dictionary<int, ProductOutputData> MakeDict()
        {
            Dictionary<int, ProductOutputData> dict = new Dictionary<int, ProductOutputData>();
            foreach (ProductOutputData productOutput in productOutputs)
                dict.Add(productOutput.MakeId, productOutput);
            return dict;
        }
    }
    
    #endregion

    #region ProductOutputRateData

    [Serializable]
    public class ProductOutputRateData
    {
        public int Output;
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


}