using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : MonoBehaviour
{
    private bool _isLoadComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                _isLoadComplete = true;
                Managers.Data.Init();
            }
        });
    }

    void Update()
    {
        if (_isLoadComplete)
            Managers.Scene.LoadScene(Define.Scene.UnderGroundScene);
    }
}
