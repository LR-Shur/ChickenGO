// ItemDefinitionManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[DefaultExecutionOrder(-100)] 
public class ItemDefinitionManager : MonoBehaviour
{
    public static ItemDefinitionManager Instance { get; private set; }
    [Header("请在 Addressables 里给所有 ItemDefinition 打上这个标签")]
    public string itemLabel = "Item";

    Dictionary<int, ItemDefinition> _map = new Dictionary<int, ItemDefinition>();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAll();
    }

    void LoadAll()
    {
        // 发起批量加载
        var handle = Addressables.LoadAssetsAsync<ItemDefinition>(
            itemLabel,
            callback: null  // 我们不需要单个回调
        );

        // 阻塞直到加载完成
        var allDefs = handle.WaitForCompletion();
        if (allDefs == null)
        {
            Debug.LogError($"[ItemDefMgr] 同步加载失败：标签={itemLabel}");
            return;
        }

        // 遍历添加到字典
        foreach (var def in allDefs)
        {
            if (def == null) continue;
            if (_map.ContainsKey(def.itemId))
                Debug.LogWarning($"[ItemDefMgr] 重复 itemId={def.itemId} ({def.name})");
            else
                _map.Add(def.itemId, def);
        }

        Debug.Log($"[ItemDefMgr] 同步加载完成，共 {_map.Count} 个 ItemDefinition");
    }

    public ItemDefinition Get(int itemId)
    {
        _map.TryGetValue(itemId, out var def);
        return def;
    }
}
