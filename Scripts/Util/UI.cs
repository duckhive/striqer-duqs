using Data_Scripts;
using Gameplay;

namespace Util
{
    public static class UI
    {
        public static void NextAgent(int index, GameData gameDataData)
        {
            index += 1;
            
            if (index > gameDataData.globalAgents.Count - 1)
                index = 0;
        }

        public static void PreviousAgent(int index, GameData gameDataData)
        {
            index -= 1;
            
            if (index < 0)
                index = gameDataData.globalAgents.Count - 1;
        }
    }
}
