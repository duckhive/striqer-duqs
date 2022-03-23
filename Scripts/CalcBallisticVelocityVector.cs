using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcBallisticVelocityVector : MonoBehaviour
{
    public Vector3 BallisticVelocityVector(Vector3 source, Vector3 target, float angle){
        Vector3 direction = target - source;                            
        float h = direction.y;                                           
        direction.y = 0;                                               
        float distance = direction.magnitude;                           
        float a = angle * Mathf.Deg2Rad;                                
        direction.y = distance * Mathf.Tan(a);                            
        distance += h/Mathf.Tan(a);                                      
 
        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2*a));
        return velocity * direction.normalized;    
    }
}
