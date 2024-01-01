using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TargetController : BaseController
{
    public TileBase _tileBase;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }
    
    public override bool Init()
    {
        if (base.Init() == false) return false;
        
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTarget(TileBase _tileBase)
    {
        this._tileBase = _tileBase;
    }

    public void setActive(bool activeType)
    {
        gameObject.SetActive(activeType);
    }

}
