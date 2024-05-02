using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Scenes
{
    public class GuildScene
    {
        Dictionary<int, Quest> dicQuests;
        List<Quest> EnrolledQuest = new List<Quest>();
        Character player;

        public GuildScene(Character character)
        {
            player = character;
            dicQuests = Game.instance.questManager.dicQuestInfos;
        }
        public void GuildMenu()
        {
            while (true) 
            {
                Game.instance.uiManager.ShowGuildMenu();
                int inputKey = Game.instance.inputManager.GetValidSelectedIndex(2);
                switch (inputKey)
                {
                    case 0:
                        Game.instance.inputManager.InputAnyKey();
                        return;
                    case 1:
                        CheckedQuestCondition();
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }

        public void CheckedQuestCondition()
        {
            while(true) 
            {
                Console.WriteLine("\n\n모험가 길드 - 의뢰 목록 확인\n\n");
                int i = 0;
                foreach (Quest quest in EnrolledQuest) 
                {                    
                    Console.WriteLine($"{i++}. {quest.Title}");
                }
                int inputKey = Game.instance.inputManager.GetValidSelectedIndex(3);
                if (inputKey == 0)
                {
                    return;
                }

                ShowQuesInfo(dicQuests[inputKey]);
            }
        }

        public void ShowQuesInfo(Quest quest)
        {
            Console.WriteLine($"Quest - {quest.Title}");
            Console.WriteLine($"\n\n{quest.Content}");
            Console.WriteLine($"\n- {quest.ClearContent} ({quest.NowStack} / {quest.ClearStack}");
            Console.WriteLine($"\n- 보상 -");
            Console.WriteLine("- {0} \n -{1}", quest.Gold ,quest.Reward == null ? "" : quest.Reward);

            int inputKey = Game.instance.inputManager.GetValidSelectedIndex(2);
            switch(inputKey) 
            { 
                case 0:

                    return;
                case 1:
                    QuestClearCheck(quest);
                    break;
                default :
                    return;
            }
        }

        public void QuestClearCheck(Quest quest)
        {
            if (quest.IsClear)
            {


            }
            else
            {

            }
        }
    }


}
