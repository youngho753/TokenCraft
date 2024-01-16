using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius = 6, float maxRadius = 12)
    {
        float randomDist = UnityEngine.Random.Range(minRadius, maxRadius);

        Vector2 randomDir = new Vector2(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100)).normalized;
        //Debug.Log(randomDir);
        var point = origin + randomDir * randomDist;
        return point;
    }

    public static Transform GetNearestMonster(Vector3 pos, LayerMask targetLayer, float scanRange)
    {
        Transform result = null;
        float dist = 100;
        
        RaycastHit2D[] _targets = Physics2D.CircleCastAll(pos, scanRange, Vector2.zero, 0, targetLayer);

        foreach (RaycastHit2D target in _targets)
        {
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(pos, targetPos);
            if (curDiff < dist)
            {
                dist = curDiff;
                result = target.transform;
            }
        }
        return result;
    }

    public static List<Transform> GetFindMonstersInFanShape(Vector3 origin, Vector3 forward, float radius = 2,  float angleRange = 80 )
    {
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster");
        RaycastHit2D[] _targets = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 0, targetLayer);

        // 타겟중에 부채꼴 안에 있는애만 리스트에 넣는다.
        foreach (RaycastHit2D target in _targets)
        {
            // '타겟-origin 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot((target.transform.position - origin).normalized, forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;
            // 시야각 판별
            if (degree <= angleRange / 2f)
                listMonster.Add(target.transform);
        }

        return listMonster;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool GetRandomBool(float probability)
    {
        return Random.value < probability * 0.01f;
    }

    public static Color HexToColor(string color)
    {
        Color parsedColor;
        ColorUtility.TryParseHtmlString("#"+color, out parsedColor);

        return parsedColor;
    }

    //Enum값중 랜덤값 반환
    public static T GetRandomValue<T>() where T : struct, Enum
    {
        Type type = typeof(T);

        if (!_enumDict.ContainsKey(type))
            _enumDict[type] = Enum.GetValues(type);

        Array values = _enumDict[type];

        int index = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(index);
    }
    
    public static Stack<TokenController> DeepCopy(Stack<TokenController> tokenStack)
    {
        if (tokenStack == null) return null;
        
        Stack<TokenController> copyStack =  new Stack<TokenController>();
        List<TokenController> stackToList = new List<TokenController>(tokenStack);
    
        
        for (int i = stackToList.Count-1 ; i >= 0 ; i--)
        {
            copyStack.Push(stackToList[i]);
        }
    
        return copyStack;
    }
    
    public static Queue<TokenController> DeepCopy(Queue<TokenController> tokenQueue)
    {
        if (tokenQueue == null) return null;
        
        Queue<TokenController> copyQueue =  new Queue<TokenController>();
        List<TokenController> queueToList = new List<TokenController>(tokenQueue);
    
        
        for (int i = queueToList.Count-1 ; i >= 0 ; i--)
        {
            copyQueue.Enqueue(queueToList[i]);
        }
    
        return copyQueue;
    }
    
    

    // public static Stack<TokenController> SettingTokenStack(Stack<TokenController> tokenStack, bool isMoveTokenStack = false, TokenController deleteToken = null)
    // {
    //     if (tokenStack == null || tokenStack.Count == 0)
    //     {
    //         SettingTokenDictionary(tokenStack,deleteToken);
    //         return null;
    //     }
    //     
    //     int idx = tokenStack.Count - 1;
    //     
    //     TokenController lowestToken = GetLowestToken(tokenStack);
    //     foreach (TokenController tc in tokenStack)
    //     {
    //         tc.SettingToken(lowestToken.pkGroupNum, idx, isMoveTokenStack);
    //         idx--;
    //     }
    //     
    //     MoveTokenStack(tokenStack, lowestToken.transform.position);
    //     
    //     SettingTokenDictionary(tokenStack);
    //
    //     return tokenStack;
    // }
    
    /**
     * @desc deleteToken을 받으면 토큰스택카운트가 0개일때도 tokenStackDic에서 GroupNum으로 remove를 한다.
     */
    // public static void SettingTokenDictionary(Stack<TokenController> tokenStack, TokenController deleteToken = null)
    // {
    //     // deleteToken을 받으면 토큰스택카운트가 0개일때도 tokenStackDic에서 GroupNum으로 remove를 한다.
    //     if ((tokenStack == null || tokenStack.Count == 0) && deleteToken != null)
    //     {
    //         if (Managers.Game._tokenStackDic.ContainsKey(deleteToken.groupNum))
    //         {
    //             Managers.Game._tokenStackDic.Remove(deleteToken.groupNum);
    //         }
    //     }
    //
    //     //deleteToken을 안 받고 토큰스택카운트가 0개면 return
    //     if (tokenStack == null || tokenStack.Count == 0) return;
    //
    //     
    //     //가장 아래에 있는 토큰 구하기
    //     TokenController lowestToken = GetLowestToken(tokenStack);
    //     
    //     
    //     //Dictionary Upsert
    //     if(Managers.Game._tokenStackDic.ContainsKey(lowestToken.pkGroupNum)) Managers.Game._tokenStackDic.Remove(lowestToken.pkGroupNum);
    //     Managers.Game._tokenStackDic.Add(lowestToken.pkGroupNum,tokenStack);
    // }

    /**
     * @desc ExceptValue
     * 1 : BlankTokenContoller
     * 2 : NatureTokencontroller
     * 4 : FactoryTokenContoller
     * 8 : MaterialTokenContoller
     */
    public static TokenController GetTokenController(GameObject gameObject, int exceptValue = 0)
    {
        if (exceptValue >= Constants.ExceptMaterialTokenContoller)
            exceptValue -= Constants.ExceptMaterialTokenContoller;
        else
            if (gameObject.GetComponent<MaterialTokenController>())
                return gameObject.GetComponent<MaterialTokenController>();

        if (exceptValue >= Constants.ExceptFactoryTokenContoller)
            exceptValue -= Constants.ExceptFactoryTokenContoller;
        else
        if (gameObject.GetComponent<FactoryTokenController>())
            return gameObject.GetComponent<FactoryTokenController>();
        
        if (exceptValue >= Constants.ExceptNatureTokencontroller)
            exceptValue -= Constants.ExceptNatureTokencontroller;
        else
        if (gameObject.GetComponent<NatureTokenController>())
            return gameObject.GetComponent<NatureTokenController>();
        
        // if (exceptValue >= Constants.ExceptBlankTokenContoller)
        //     exceptValue -= Constants.ExceptBlankTokenContoller;
        // else
        // if (gameObject.GetComponent<BlankTokenController>())
        //     return gameObject.GetComponent<BlankTokenController>();
        
        return null;
    }


}
