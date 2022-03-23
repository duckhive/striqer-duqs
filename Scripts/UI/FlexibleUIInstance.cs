using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlexibleUIInstance : Editor
{
    [MenuItem("GameObject/Flexible UI/Button", priority = 0)]
    public static void AddButton()
    {
        Create("Button");
    }
    
    private static GameObject _clickedObject;

    private static GameObject Create(string objectName)
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>(objectName));
        instance.name = objectName;
        _clickedObject = Selection.activeObject as GameObject;
        if (_clickedObject is not null)
        {
            instance.transform.SetParent(_clickedObject.transform, false);
        }

        return instance;
    }
    
    
}
