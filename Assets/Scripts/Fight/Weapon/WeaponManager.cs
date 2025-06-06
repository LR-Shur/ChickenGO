namespace Fight.Weapon
{
    // WeaponManager.cs
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// WeaponManager：单例。游戏启动时自动加载 Resources/Weapons 下的所有 WeaponBase SO，
    /// 并建立一个 itemId→WeaponBase 的字典，以便运行时根据物品 ID 快速查到对应武器。
    /// </summary>
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; private set; }

        private Dictionary<int, WeaponBase> weaponDict;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAllWeapons();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 从 Resources/Weapons 文件夹里一次性加载所有 WeaponBase 类型的 SO，
        /// 并把它们放到 weaponDict[itemId] = 对应 SO
        /// </summary>
        private void LoadAllWeapons()
        {
            weaponDict = new Dictionary<int, WeaponBase>();
            WeaponBase[] all = Resources.LoadAll<WeaponBase>("Weapons");
            foreach (var w in all)
            {
                if (weaponDict.ContainsKey(w.itemId))
                {
                    Debug.LogWarning($"WeaponManager: 重复的 itemId {w.itemId}，武器 {w.name} 会覆盖之前的条目。");
                }
                weaponDict[w.itemId] = w;
            }
        }

        /// <summary>
        /// 外部调用：根据 ItemDefinition.itemId 获取对应的武器 SO，
        /// 如果找不到，返回 null
        /// </summary>
        public WeaponBase GetWeaponByItemId(int itemId)
        {
            weaponDict.TryGetValue(itemId, out var w);
            return w;
        }
    }

}
