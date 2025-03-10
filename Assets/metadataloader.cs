//using System;
//using System.Collections.Generic;
//using System.IO;
//using FPSCommando.EnvironmenLoader;
//using FPSCommando.LevelDataSystem;
//using FPSCommando.MetaData;
//using FPSCommando.SpinWheelSystem.Helper_Classes;
//using MetaData;
//using MetaData.DataHolders;
//using ProjectCore.Variables;
//using Sirenix.OdinInspector;
//using UnityEngine;
//using String = System.String;

//namespace FPSCommando.Weapon
//{


//    [CreateAssetMenu(fileName = "MetaLoader", menuName = "CommandoFPS/Meta/MetaData")]
//    public class MetaDataLoader : SerializedScriptableObject
//    {

//        [BoxGroup("Meta Files")] public string MetaFileName;
//        [BoxGroup("Meta Files")] public string MetaFilePath;

//        [BoxGroup("Meta Files")] public string LoadMetaFile;

//        public Dictionary<string, Action<string[]>> _metaDataParseActions;
//        public Dictionary<string, List<string[]>> _ParsedCsv;

//        [SerializeField] private GunBulletDataSO GunBulletDataSo;
//        [SerializeField] private WeaponsDataListContainer WeaponsDataListContainer;
//        [SerializeField] private BotsDataListContainer BotsDataListContainer;
//        [SerializeField] private LevelDataSystem.LevelDataSystem LevelDataSystemDustTown;
//        [SerializeField] private LevelDataSystem.LevelDataSystem LevelDataSystemSquad;
//        [SerializeField] private LevelDataSystem.LevelDataSystem LevelDataSystemWinterland;
//        [SerializeField] private LevelDataSystem.LevelDataSystem LevelDataSystemTank;
//        [SerializeField] private DailyRewardDataHolder DailyRewardDataHolder;
//        [SerializeField] private DBInt InitialHealth;
//        [SerializeField] private FPSInitialEconomy FPSInitialEconomy;
//        [SerializeField] private SpinWheelDataSO SpinWheelDataSO;
//        [SerializeField] private ModeLevelDataSO DustTownModeLevelDataSO;
//        [SerializeField] private ModeLevelDataSO TankModeLevelDataSO;
//        [SerializeField] private ModeLevelDataSO WinterHoldModeLevelDataSO;
//        [SerializeField] private ModeLevelDataSO SquadModeLevelDataSO;
//        [SerializeField] private WeaponSkinDataContainer WeaponSkinDataContainer;
//        [SerializeField] private IAPItemDataContainer IAPItemDataContainer;
//        [SerializeField] private EnemyDeathRewardDataSO EnemyDeathRewardDataSO;
//        [SerializeField] private WeaponPricingDataContainer WeaponPricingDataContainer;

//        [Button]
//        public void TestLoading()
//        {
//            ClearAll();
//            Init();
//            LoadData();
//        }

//        [Button]
//        public void LoadAllData()
//        {
//            ClearAll();
//            Init();
//            LoadData();
//            if(Debug.isDebugBuild)
//            {Debug.Log("[MetaData Loader]Data Loaded");}
//        }

//        private void LoadData()
//        {
//            TextAsset textAsset = null;
//            textAsset = Resources.Load(MetaFileName) as TextAsset;
//            if (textAsset != null)
//            {
//                //Debug.Log("Is not Null");
//                LoadDataUsingCSV(textAsset.text);
//            }
//        }

//        [Button]
//        private void ClearAll()
//        {
//            _metaDataParseActions.Clear();
//            _ParsedCsv.Clear();
//            GunBulletDataSo.GunBulletDataList.Clear();
//            WeaponsDataListContainer.WeaponsDataList.Clear();
//            BotsDataListContainer.BotsDataList.Clear();
//            LevelDataSystemDustTown.LevelDataList.Clear();
//            LevelDataSystemWinterland.LevelDataList.Clear();
//            LevelDataSystemSquad.LevelDataList.Clear();
//            LevelDataSystemTank.LevelDataList.Clear();
//            DailyRewardDataHolder.DailyRewardDataList.Clear();
//            SpinWheelDataSO.dataContainer.rewards.Clear();
//            DustTownModeLevelDataSO.ModeLevelDataDataList.Clear();
//            TankModeLevelDataSO.ModeLevelDataDataList.Clear();
//            WinterHoldModeLevelDataSO.ModeLevelDataDataList.Clear();
//            SquadModeLevelDataSO.ModeLevelDataDataList.Clear();
//            WeaponSkinDataContainer.WeaponSkinsList.Clear();
//            IAPItemDataContainer.IAPItemsList.Clear();
//            IAPItemDataContainer.InGameIAPUsedList.Clear();
//            IAPItemDataContainer.IAPRewardsList.Clear();
//            EnemyDeathRewardDataSO.EnemyDeathRewards.Clear();   
//            WeaponPricingDataContainer.WeaponPricingList.Clear();
//        }

//        private void LoadDataUsingCSV(string data)
//        {
//            var tableCount = 0;
//            var lastTableName = "";
//            var reader = new StringReader(data);
//            if (reader == null)
//            {
//                Debug.Log("metaData data not readable");
//                return;
//            }
//            else
//            {
//                string line;
//                while ((line = reader.ReadLine()) != null)
//                {
//                    var elements = line.Split(',');
//                    var sectionId = elements[0];
//                    var skippedHeader = false;
//                    string dataLine;

//                    while ((dataLine = reader.ReadLine()) != null)
//                    {
//                        var dataElements = dataLine.Split(',');
//                        if (dataElements[0] == "[END]")
//                        {

//                            break;
//                        }

//                        if (!skippedHeader)
//                        {
//                            skippedHeader = true;
//                            continue;
//                        }

//                        if (_metaDataParseActions.ContainsKey(sectionId))
//                        {
//                            if (!string.Equals(lastTableName, sectionId))
//                            {
//                                lastTableName = sectionId;
//                                tableCount++;
//                            }

//                            _metaDataParseActions[sectionId].Invoke(dataElements);
//                        }

//                        if (!_ParsedCsv.ContainsKey(sectionId))
//                        {
//                            _ParsedCsv.Add(sectionId, new List<string[]>());
//                        }

//                        List<string[]> container = _ParsedCsv[sectionId];
//                        container.Add(dataElements);
//                    }
//                }
//                reader.Close();
//            }
//            tableCount = _metaDataParseActions.Count;
//        }
//        public void Init()
//        {
//            _ParsedCsv = new Dictionary<string, List<string[]>>();
//            _metaDataParseActions = new Dictionary<string, Action<string[]>>()
//            {
//                { "[WEAPON_BULLETS]", LoadBulletData },
//                { "[WEAPONS_DATA]", LoadWeaponsData },
//                { "[BOTS_DATA]", LoadBotsData },
//                { "[DUSTTOWN_LEVELS]", LoadDustTownLevelData },
//                { "[SQUAD_LEVELS]", LoadSquadLevelData },
//                { "[WINTERLAND_LEVELS]", LoadWinterlandLevelData },
//                { "[TANK_LEVELS]", LoadTankLevelData },
//                { "[DAILY_REWARD]", DailyRewardData },
//                { "[INITIAL_ECONOMY]", InitialEconomy },
//                { "[INITIAL_HEALTH]", InitialPlayerHealth },
//                { "[SPIN_WHEEL]", SpinWheelData },
//                {"[DUST_TOWN_MODE_LEVEL_DATA]",LoadDustTownGameLevelsData },
//                {"[TANK_MODE_LEVEL_DATA]",LoadTankGameLevelsData },
//                { "[SQUAD_MODE_LEVEL_DATA]",LoadSquadGameLevelsData},
//                { "[WINTER_HOLD_MODE_LEVEL_DATA]",LoadWinterGameLevelsData},
//                 {"[WEAPON_SKIN_DATA]",LoadWeaponsSkinData },
//                {"[Total_IAP_InGame]",LoadIAPItemsData },
//                {"[InGame_IAP_used]",LoadIAPUsedInGame },
//                {"[Reward_with_ID]",LoadIAPRewardList },
//                {"[ENEMY_DEATH_REWARD]",LoadEnemyDeathReward },
//                {"[WEAPON_PRICE]",LoadWeaponPriceData }
//            };
//        }

//        void LoadEnemyDeathReward(string[] elements)
//        {
//            if (Enum.TryParse(elements[0].ToString(), out   ModeRewardType gameMode))
//            {
//                EnemyDeathReward enemyDeathReward = new EnemyDeathReward(gameMode, elements[1].ToInt(), elements[2].ToInt());
//                EnemyDeathRewardDataSO.EnemyDeathRewards.Add(enemyDeathReward);
//            }
//            else
//            {
//                if(Debug.isDebugBuild)
//                {Console.WriteLine("Invalid GAMEMODE type.");}
//            }
//        }
//        private void LoadIAPItemsData(string[] elements)
//        {
//            if (elements == null)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError("elements is null");}
//                return;
//            }
//            IAPItemsData iapItemsData = new IAPItemsData(
//                elements[0].ToInt(),
//                elements[1].ToString(),
//                elements[2].ToString(),
//                elements[3].ToFloat(),
//                IAPItemDataContainer.ParseProductType(elements[4]),
//                elements[5].ToParseStringToIntArray(),   //  IAPItemDataContainer.ParseStringToIntArray(elements[5]),
//                elements[6].ToParseStringToIntArray(),   // IAPItemDataContainer.ParseStringToIntArray(elements[6]),
//                elements[7].ToParseStringToFloatArray(), //  IAPItemDataContainer.ParseStringToFloatArray(elements[7]),
//                elements[8].ToString(),
//                elements[9].ToString(),
//                elements[10].ToFloat(),
//                elements[11].ToFloat(),
//                elements[12].ToInt()
//                );

//            try
//            {
//                IAPItemDataContainer.IAPItemsList.Add(iapItemsData);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError(ex);
//            }
//        }
//        private void LoadIAPUsedInGame(string[] elements)
//        {
//            if (elements == null)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError("elements is null");}
//                return;
//            }
//            string name = elements[0].ToString();
//            try
//            {
//                IAPItemDataContainer.InGameIAPUsedList.Add(name);
//            }
//            catch (System.Exception ex)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError(ex);}
//            }
//        }

//        private void LoadIAPRewardList(string[] elements)
//        {
//            if (elements == null)
//            {
//                Debug.LogError("elements is null");
//                return;
//            }
//            RewardType iapItemsData = new RewardType(
//              elements[0].ToInt(),
//              IAPItemDataContainer.ParseIAPRewardName(elements[1])
//                  );
//            try
//            {
//                IAPItemDataContainer.IAPRewardsList.Add(iapItemsData);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError(ex);
//            }

//        }

//        private void LoadWeaponPriceData(string[] elements)
//        {
//            if (elements == null)
//            {
//                Debug.LogError("elements is null");
//                return;
//            }

//            var weaponPriceData = new WeaponPricingData(
//                WeaponData.ParseWeaponType(elements[0]),
//                elements[1].ToInt()
//                );
            
//            try
//            {
//                WeaponPricingDataContainer.WeaponPricingList.Add(weaponPriceData);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError(ex);
//            }
//        }
        
//        private void LoadWeaponsSkinData(string[] elements)
//        {
//            if (elements == null)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError("elements is null");}
//                return;
//            }

//            var weaponSkinData = new WeaponSkinData(
//                WeaponData.ParseWeaponType(elements[0]),
//                elements[1].ToInt(),
//                elements[2].ToInt(),
//                elements[3].ToString()
//                );
//            try
//            {
//                WeaponSkinDataContainer.WeaponSkinsList.Add(weaponSkinData);
//            }
//            catch (System.Exception ex)
//            {
//                Debug.LogError(ex);
//            }
//        }
//        void SpinWheelData(string[] elements)
//        {
//            if (elements.Length < 2)
//            {

//                return;
//            }
//            SpinWheelReward reward = new SpinWheelReward { Reward = elements[0], Amount = elements[1].ToInt(), Probability = elements[2].ToFloat() };
//            SpinWheelDataSO.dataContainer.rewards.Add(reward);
//        }
//        private void DailyRewardData(string[] elements)
//        {

//            if (elements.Length < 2)
//            {
//                Debug.Log("DailyReward Not parsable");
//                return;
//            }
//            // Parse the day
//            int rewardDay = int.Parse(elements[0]);
//            // Create a new DailyRewardData for this day
//            DailyRewardData dailyRewardData = new DailyRewardData();
//            dailyRewardData.RewardDay = rewardDay;
//            // Loop through rewards (every pair of elements: reward type, reward amount)
//            for (int i = 1; i < elements.Length - 1; i += 3)
//            {
//                string rewardTypeStr = elements[i].ToString();
//                int rewardAmount = elements[i + 1].ToInt();
//                string rewardCardType = elements[i + 2].ToString();
//                // Ignore invalid rewards (-1)
//                if (rewardAmount == -1) continue;

//                // Parse the reward type dynamically from the enum
//                if ((Enum.TryParse(rewardTypeStr, out DailyRewardEnum rewardType)) && (Enum.TryParse(rewardCardType, out DailyRewardCardType cardType)))
//                {
//                    DayReward reward = new DayReward { RewardType = rewardType, RewardAmount = rewardAmount, CardType = cardType };
//                    dailyRewardData.RewardList.Add(reward);
//                }
//                else
//                {
////                    Debug.LogError($"Unknown reward type: {rewardTypeStr} on Day {rewardDay}");
//                }
//            }

//            // Add the parsed daily reward data to the holder
//            DailyRewardDataHolder.DailyRewardDataList.Add(dailyRewardData);
//        }
//        private void InitialPlayerHealth(string[] elements)
//        {
//            InitialHealth.SetValue(elements[0].ToInt());
//        }

//        private void InitialEconomy(string[] elements)
//        {
//            FPSInitialEconomy.InitialCoinCount =elements[0].ToInt();
//            FPSInitialEconomy.GrenadeCount.SetValue(elements[1].ToInt());
//        }
//        void LoadDustTownLevelData(String[] obj)
//        {

//            if (Enum.TryParse(obj[2].ToString(), out LevelCardType leveltype))
//            {
//                LevelCardData levelCardData = new LevelCardData(obj[0].ToInt(), obj[1].ToString(), leveltype);
//                LevelDataSystemDustTown.LevelDataList.Add(levelCardData);
//            }
//            else
//            {
//                Console.WriteLine("Invalid level type.");
//            }
//        }

//        void LoadSquadLevelData(String[] obj)
//        {
//            if (Enum.TryParse(obj[2].ToString(), out LevelCardType leveltype))
//            {
//                LevelCardData levelCardData = new LevelCardData(obj[0].ToInt(), obj[1].ToString(), leveltype);
//                LevelDataSystemSquad.LevelDataList.Add(levelCardData);
//            }
//            else
//            {
//                if(Debug.isDebugBuild)
//                {Debug.Log("Invalid level type.");}
//            }
//        }
//        void LoadWinterlandLevelData(String[] obj)
//        {
//            if (Enum.TryParse(obj[2].ToString(), out LevelCardType leveltype))
//            {
//                LevelCardData levelCardData = new LevelCardData(obj[0].ToInt(), obj[1].ToString(), leveltype);
//                LevelDataSystemWinterland.LevelDataList.Add(levelCardData);
//            }
//            else
//            {
//                if(Debug.isDebugBuild)
//                {Debug.Log("Invalid level type.");}
//            }
//        }
//        void LoadTankLevelData(String[] obj)
//        {
//            if (Enum.TryParse(obj[2].ToString(), out LevelCardType leveltype))
//            {
//                LevelCardData levelCardData = new LevelCardData(obj[0].ToInt(), obj[1].ToString(), leveltype);
//                LevelDataSystemTank.LevelDataList.Add(levelCardData);
//            }
//            else
//            {
//                if(Debug.isDebugBuild)
//                {Console.WriteLine("Invalid level type.");}
//            }
//        }
//        void LoadBulletData(string[] elements)
//        {
//            GunBulletData _bulletData = new GunBulletData(Utilities.ToInt(elements[0]), elements[1], elements[2].ToInt(), elements[3].ToInt(), elements[4].ToInt(), elements[5].ToInt(), elements[6].ToInt(), elements[7].ToInt(), elements[8].ToInt());
//            GunBulletDataSo.GunBulletDataList.Add(_bulletData);
//        }
//        private void LoadWeaponsData(string[] elements)
//        {
//            if (elements == null || elements.Length < 22)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError(elements == null ? "elements is null" : "elements array is too short");}
//                return;
//            }

//            try
//            {
//                var weaponUpgrades = new WeaponUpgrades
//                (
//                    ParseInt(elements[3]),
//                    ParseFloat(elements[5]),
//                    ParseFloat(elements[7]),
//                    ParseInt(elements[9]),
//                    ParseFloat(elements[11])
//                );

//                var weaponUpgradesCost = new WeaponUpgradesCost
//                (
//                    ParseInt(elements[21]),
//                    ParseInt(elements[13]),
//                    ParseInt(elements[15]),
//                    ParseInt(elements[17]),
//                    ParseInt(elements[19])
//                );

//                var weaponStats = new WeaponStats(ParseInt(elements[2]), weaponUpgrades, weaponUpgradesCost);
                
//                // Helper function to convert string elements to the appropriate type
//                var weaponData = new WeaponData(
//                    elements[0], 
//                    WeaponData.ParseWeaponType(elements[1]),
//                    weaponStats
//                );

//                WeaponsDataListContainer.AddWeaponData(weaponData);
//            }
//            catch (System.Exception ex)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError(ex);}
//            }
//        }

//// Helper methods for parsing
//        private int ParseInt(string value) => int.TryParse(value, out var result) ? result : 0;
//        private float ParseFloat(string value) => float.TryParse(value, out var result) ? result : 0f;

//        private void LoadBotsData(string[] elements)
//        {
//            if (elements == null)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError("elements is null");}
//                return;
//            }

//            if (elements.Length < 16)
//            {
//                if(Debug.isDebugBuild)
//                {Debug.LogError("elements array is too short");}
//                return;
//            }

//            try
//            {
//                var botData = new BotData(elements[0], elements[1].ToFloat(), elements[3].ToFloat(), elements[5].ToFloat(), elements[7].ToInt(), elements[9].ToInt(), elements[11].ToFloat(), elements[13].ToFloat(), elements[15].ToFloat());
//                BotsDataListContainer.BotsDataList.Add(botData);
//            }
//            catch (System.Exception ex)
//            {
//                Debug.LogError(ex);
//            }
//        }
//        void LoadDustTownGameLevelsData(string[] elements)
//        {
//            if (Enum.TryParse(elements[12].ToString(), out ModeLevelData.EnemyAi enemyAiType))
//            {
//                ModeLevelData _modeLevelData = new ModeLevelData(elements[1].ToInt(), elements[2].ToBool(),
//                    elements[3].ToInt(),
//                    elements[4].ToInt(), elements[5].ToInt(), elements[6].ToInt(), elements[7].ToInt(),
//                    elements[8].ToInt(), elements[9].ToBool(), elements[10].ToBool(), elements[11].ToBool(), enemyAiType);
//                DustTownModeLevelDataSO.ModeLevelDataDataList.Add(_modeLevelData);

//            }
//        }
//        void LoadTankGameLevelsData(string[] elements)
//        {
//            if (Enum.TryParse(elements[12].ToString(), out ModeLevelData.EnemyAi enemyAiType))
//            {
//                ModeLevelData _modeLevelData = new ModeLevelData(elements[1].ToInt(), elements[2].ToBool(),
//                    elements[3].ToInt(),
//                    elements[4].ToInt(), elements[5].ToInt(), elements[6].ToInt(), elements[7].ToInt(),
//                    elements[8].ToInt(), elements[9].ToBool(), elements[10].ToBool(), elements[11].ToBool(), enemyAiType);
//                TankModeLevelDataSO.ModeLevelDataDataList.Add(_modeLevelData);
//            }
//        }
//        void LoadSquadGameLevelsData(string[] elements)
//        {
//            if (Enum.TryParse(elements[12].ToString(), out ModeLevelData.EnemyAi enemyAiType))
//            {
//                ModeLevelData _modeLevelData = new ModeLevelData(elements[1].ToInt(), elements[2].ToBool(),
//                    elements[3].ToInt(),
//                    elements[4].ToInt(), elements[5].ToInt(), elements[6].ToInt(), elements[7].ToInt(),
//                    elements[8].ToInt(), elements[9].ToBool(), elements[10].ToBool(), elements[11].ToBool(), enemyAiType);
//                SquadModeLevelDataSO.ModeLevelDataDataList.Add(_modeLevelData);
//            }
//        }
//        void LoadWinterGameLevelsData(string[] elements)
//        {
//            if (Enum.TryParse(elements[12].ToString(), out ModeLevelData.EnemyAi enemyAiType))
//            {
//                ModeLevelData _modeLevelData = new ModeLevelData(elements[1].ToInt(), elements[2].ToBool(),
//                    elements[3].ToInt(),
//                    elements[4].ToInt(), elements[5].ToInt(), elements[6].ToInt(), elements[7].ToInt(),
//                    elements[8].ToInt(), elements[9].ToBool(), elements[10].ToBool(), elements[11].ToBool(), enemyAiType);
//                WinterHoldModeLevelDataSO.ModeLevelDataDataList.Add(_modeLevelData);
//            }
//        }
//    }
//}
