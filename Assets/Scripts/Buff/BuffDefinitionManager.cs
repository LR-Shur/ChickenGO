namespace Buff
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    [DefaultExecutionOrder(-100)] 
    public class BuffDefinitionManager : MonoBehaviour
    {
        public static BuffDefinitionManager Instance { get; private set; }

        public string buffLabel = "Buff";

        private Dictionary<int, BuffDefinition> _map = new Dictionary<int, BuffDefinition>();
        

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadAll();
        }

        void LoadAll()
        {
            var handle = Addressables.LoadAssetsAsync<BuffDefinition>(
                buffLabel,
                callback: null
            );

            var allDefs = handle.WaitForCompletion();
            if (allDefs == null)
            {
                Debug.LogError($"[BuffDefMgr] 同步加载失败：标签={buffLabel}");
                return;
            }

            foreach (var def in allDefs)
            {
                if (def == null) continue;
                if (_map.ContainsKey(def.buffId))
                    Debug.LogWarning($"[BuffDefMgr] 重复 buffId={def.buffId} ({def.name})");
                else
                    _map.Add(def.buffId, def);
            }

            Debug.Log($"[BuffDefMgr] 同步加载完成，共 {_map.Count} 个 BuffDefinition");
        }

        void OnLoadCompleted(AsyncOperationHandle<IList<BuffDefinition>> h)
        {
            if (h.Status == AsyncOperationStatus.Succeeded)
                Debug.Log($"[BuffDefMgr] 已加载 {_map.Count} 个 BuffDefinition");
            else
                Debug.LogError($"[BuffDefMgr] 加载失败: {h.OperationException}");
        }

        /// <summary>
        /// 根据 buffId 查找 BuffDefinition
        /// </summary>
        public BuffDefinition Get(int buffId)
        {
            _map.TryGetValue(buffId, out var def);
            return def;
        }
    }

}
