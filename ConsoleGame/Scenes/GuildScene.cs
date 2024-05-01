﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Scenes
{
    public class GuildScene
    {
        Dictionary<int, Quest> dicQuests = new Dictionary<int, Quest>();
        Character player;
        public GuildScene(Character character)
        {
            player = character;
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
                    case 2:
                        ShowQuestList();
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }

        public void CheckedQuestCondition()
        {


        }

        public void ShowQuestList()
        {

        }
    }


}
