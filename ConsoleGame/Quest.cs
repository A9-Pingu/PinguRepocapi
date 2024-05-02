using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public class Quest
    {
        public string Title = "공 백";
        public string Content = string.Empty;
        public bool IsClear = false;
        public string ClearContent = string.Empty;
        public int NowStack = 0;
        public int ClearStack = 0;
        public int Gold = 0;
        public Item Reward;
        public event Action<Quest> OnCheck;
        public Quest(string title, string content, string clearContent , int clearStack,  int gold = 0, Item reward = null)
        {
            Title = title;
            Content = content;
            ClearContent = clearContent;
            ClearStack = clearStack;
            Gold = gold;
            Reward = reward;
        }
    }
}
