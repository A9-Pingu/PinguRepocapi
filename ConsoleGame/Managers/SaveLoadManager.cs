using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Newtonsoft.Json;

namespace ConsoleGame.Managers
{
    public sealed class SaveLoadManager
    {
        private const string SAVE_FOLDER = "SaveGames";

        public SaveLoadManager() {}  // private 생성자로 외부에서 인스턴스화 방지

        public Character LoadOrStartGame(InputManager input)
        {
            List<string> savedGames = GetSavedGames();

            if (savedGames.Count > 0)
            {
                DisplaySavedGames(savedGames);
                int selectedIndex = input.GetValidSelectedIndex(savedGames.Count, 1);

                string selectedGame = savedGames[selectedIndex - 1];
                Character player = LoadGame(selectedGame);
                return player;
            }
            else
            {
                Console.WriteLine("저장된 게임 파일이 없습니다.");
                Character player = CreateNewCharacter();
                return player;
            }            
        }


        public List<string> GetSavedGames()
        {
            string saveFolderPath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER);

            if (!Directory.Exists(saveFolderPath))
            {
                return new List<string>();
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(saveFolderPath);
            FileInfo[] files = directoryInfo.GetFiles("*.json");

            List<string> savedGames = new List<string>();
            foreach (var file in files)
            {
                savedGames.Add(file.Name);
            }

            return savedGames;
        }

        public void SaveGame(Character player)
        {
            string saveFolderPath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER);

            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            string saveName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            string filePath = Path.Combine(saveFolderPath, $"{saveName}.json");

            string json = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllText(filePath, json);

            Console.WriteLine($"게임이 저장되었습니다. ({saveName}.json)");
        }


        public Character LoadGame(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    Character player = JsonConvert.DeserializeObject<Character>(json);
                    Console.WriteLine("게임 불러오기 완료");
                    return player;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"게임 불러오기 실패: {e.Message}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("선택한 게임 파일이 없습니다.");
                return null;
            }
        }



        //private void SaveGame(Character player)
        //{
        //    SaveLoadManager.Instance.SaveGame(player);
        //}

        Character CreateNewCharacter()
        {
            Console.Write("캐릭터 이름을 입력하세요: ");
            string name = Console.ReadLine();

            Console.Write("직업을 선택하세요 (전사, 마법사, 도적 등): ");
            string job = Console.ReadLine();

            return new Character(name, job);
        }


        public void DeleteSavedGame(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine($"게임 파일 {fileName}을(를) 삭제하였습니다.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"게임 파일 삭제 실패: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("선택한 게임 파일이 없습니다.");
            }
        }

        public void DisplaySavedGames(List<string> savedGames)
        {
            Console.WriteLine("불러올 게임을 선택해주세요:");
            for (int i = 0; i < savedGames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {savedGames[i]}");
            }
        }


    }
}
