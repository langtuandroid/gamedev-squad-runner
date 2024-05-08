using UnityEngine;

namespace JetSystems
{

    public static class PlayerPrefsManager
    {
        static string COINSKEY = "COINS";
        static string DIAMONDSKEY = "DIAMONDS";
        static string HEROMODELKEY = "HEROMODEL";
        static string ITEMUNLOCKEDKEY = "ITEMUNLOCKED";
        static string LEVELKEY = "LEVEL";

        public static void SaveSettings(string key, bool state)
        {
            PlayerPrefs.SetInt(key, state ? 1 : 0);
            PlayerPrefs.Save();
        }
         
        public  static bool LoadSettings(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                SaveSettings(key, false);
                PlayerPrefs.Save();
            }
            return PlayerPrefs.GetInt(key) == 1;
        }


        public static int GetCoins()
        { return PlayerPrefs.GetInt(COINSKEY,800); }

        public static void SaveCoins(int coinsAmount)
        {
            PlayerPrefs.SetInt(COINSKEY, coinsAmount);
            PlayerPrefs.Save();
        }
        
        public static int GetDiamonds()
        { return PlayerPrefs.GetInt(DIAMONDSKEY); }

        public static void SaveDiamonds(int diamondsAmount)
        {
            PlayerPrefs.SetInt(DIAMONDSKEY, diamondsAmount); 
            PlayerPrefs.Save();
        }

        public static int GetSelectHeroModel()
        {
            if (!PlayerPrefs.HasKey(HEROMODELKEY))
            {
                SaveSelectHeroModel(0);
            }
            
            return PlayerPrefs.GetInt(HEROMODELKEY);
        }

        public static void SaveSelectHeroModel(int indexCharacter)
        {
            PlayerPrefs.SetInt(HEROMODELKEY, indexCharacter);
            PlayerPrefs.Save();
        }

        
        public static int GetItemUnlockedState(int itemIndex)
        { return PlayerPrefs.GetInt(ITEMUNLOCKEDKEY + itemIndex); }

        public static void SetItemUnlockedState(int itemIndex, int state)
        { PlayerPrefs.SetInt(ITEMUNLOCKEDKEY + itemIndex, state); }
        

        public static int GetLevel()
        {
            if (!PlayerPrefs.HasKey(LEVELKEY))
            {
                SaveLevel(1);
            }
            return PlayerPrefs.GetInt(LEVELKEY);
        }

        public static void SaveLevel(int level)
        { PlayerPrefs.SetInt(LEVELKEY, level); }
    }
}
