using System;
using System.Collections;
using System.Collections.Generic;
using Data_Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

[ExecuteInEditMode()]
public class FlexibleUI : MonoBehaviour
{
    public FlexibleUIData skinData;

    protected virtual void OnSkinUI()
    {
        
    }

    public virtual void Awake()
    {
        OnSkinUI();
    }

    private void Update()
    {
        if(Application.isEditor)
            OnSkinUI();
    }
}
