using System.Collections.Generic;
using UnityEngine;
using AGFG.Core;

namespace AGFG.Activities
{
    public class AreaManager : MonoBehaviour
    {
        #region Singleton
        private static AreaManager _instance;
        public static AreaManager Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance != null)
                    Debug.LogError("[Area] Trying to set singleton instance when instance already exists !");
                else
                    _instance = value;
            }
        }
        #endregion

        public List<ActivityFighting> FightingSpots { get; private set; } = new List<ActivityFighting>();
        public List<ActivityFishing> FishingSpots { get; private set; } = new List<ActivityFishing>();
        public List<ActivityMining> MiningSpots { get; private set; } = new List<ActivityMining>();
        public List<AICCharacter> AICharacters { get; private set; } = new List<AICCharacter>();
        public List<ControllableCharacter> CharacterEntities { get; private set; } = new List<ControllableCharacter>();
        public List<AGFGEnemyBase> EnemyEntities { get; private set; } = new List<AGFGEnemyBase>();

        private Dictionary<System.Type, ActivityBase[]> ActivityArraysByType = new Dictionary<System.Type, ActivityBase[]>();

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            FindSelectables();
            FindEntities();
        }

        private void FindSelectables()
        {
            var selectables = FindObjectsOfType<Selectable>(true);
            foreach (Selectable s in selectables)
            {
                if (s is ActivityFighting af)
                    FightingSpots.Add(af);
                else if (s is ActivityFishing fs)
                    FishingSpots.Add(fs);
                else if (s is ActivityMining ms)
                    MiningSpots.Add(ms);
                else if (s is AICCharacter ai)
                    AICharacters.Add(ai);
            }

            ActivityArraysByType.Add(typeof(ActivityFighting), FightingSpots.ToArray());
            ActivityArraysByType.Add(typeof(ActivityFishing), FishingSpots.ToArray());
            ActivityArraysByType.Add(typeof(ActivityMining), MiningSpots.ToArray());
        }

        private void FindEntities()
        {
            var entities = FindObjectsOfType<CharacterBase>(true);
            foreach (CharacterBase e in entities)
            {
                if (e is ControllableCharacter c)
                    CharacterEntities.Add(c);
                else if (e is AGFGEnemyBase enemy)
                    EnemyEntities.Add(enemy);
            }
        }

        public ActivityBase[] TryGetRelevantActivityArray<T>()
        {
            if (ActivityArraysByType.TryGetValue(typeof(T), out ActivityBase[] result))
                return result;
            else
                Debug.LogError("[Area] TryGetRelevantArray -> No array for type : " + typeof(T));

            return null;
        }
    }
}