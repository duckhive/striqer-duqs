using System.Collections;
using System.Collections.Generic;
using Data_Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentDisplay : MonoBehaviour
{
    public TMP_Text agentName;
    public TMP_Text agentDescription;
    public GameObject agentPrefab;

    public void DisplayAgent(AgentData agentData)
    {
        agentName.text = agentData.character.ToString();
        agentDescription.text = agentData.description;
        agentPrefab = agentData.prefab;
    }
}
