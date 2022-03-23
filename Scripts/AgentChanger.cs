using Data_Scripts;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class AgentChanger : MonoBehaviour
{
   public TMP_Text agentText;

   public GameData gameData;
   public HomeTeamData homeTeamData;
   public AwayTeamData awayTeamData;
   
   public int gameAgentIndex;
   [HideInInspector] public int currentIndex;

   private void Start()
   {
      currentIndex = UnityEngine.Random.Range(0, gameData.globalAgents.Count - 1);
      SetAgentInGame(gameAgentIndex);

      agentText = GetComponentInChildren<TMP_Text>();
   }

   private void Update()
   {
      agentText.text = "" + gameData.globalAgents[currentIndex].character;
   }
   
   public void NextAgent()
   {
      currentIndex += 1;
            
      if (currentIndex > gameData.globalAgents.Count - 1)
         currentIndex = 0;
      
      SetAgentInGame(currentIndex);
   }
   
   public void PreviousAgent()
   {
      currentIndex -= 1;
            
      if (currentIndex < 0)
         currentIndex = gameData.globalAgents.Count - 1;
      
      SetAgentInGame(currentIndex);
   }

   public void SetAgentInGame(int index)
   {
      switch (gameAgentIndex)
      {
         case 1:
            awayTeamData.teamAgentsData[0] = gameData.globalAgents[index];
            break;
         case 2:
            awayTeamData.teamAgentsData[1] = gameData.globalAgents[index];
            break;
         case 3:
            awayTeamData.teamAgentsData[2] = gameData.globalAgents[index];
            break;
         case 4:
            awayTeamData.teamAgentsData[3] = gameData.globalAgents[index];
            break;
         case 5:
            homeTeamData.teamAgentsData[0] = gameData.globalAgents[index];
            break;
         case 6:
            homeTeamData.teamAgentsData[1] = gameData.globalAgents[index];
            break;
         case 7:
            homeTeamData.teamAgentsData[2] = gameData.globalAgents[index];
            break;
         case 8:
            homeTeamData.teamAgentsData[3] = gameData.globalAgents[index];
            break;
         /*case 9:
            GameData.Instance.awayTeam.teamAgentsData[4] = GameData.Instance.globalAgents[index];
            break;
         case 10:
            GameData.Instance.scriptableTeam.teamAgentsData[4] = GameData.Instance.globalAgents[index];
            break;*/
      }
   }
}
