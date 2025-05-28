using System.Collections.Generic;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI
{
    

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // 缓存所有已加载的 UI
    private Dictionary<string, BaseUI> _cache = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject.transform.root.gameObject);
    }

    /// <summary>
    /// 通用打开 UI 界面
    /// </summary>
    public async Task<T> ShowUI<T>() where T : BaseUI
    {
        string key = typeof(T).Name;

        if (!_cache.TryGetValue(key, out var ui))
        {
            // Addressables 路径约定： "ui/{UI 名称}"
            var handle = Addressables.LoadAssetAsync<GameObject>($"ui/{key}");
            var prefab = await handle.Task;
            var go = Instantiate(prefab, transform);
            ui = go?.GetComponent<T>();
            if (ui == null) {
                ui =go.AddComponent<T>();
            }
            _cache[key] = ui;
        }

        ui.Show();
        return ui as T;
    }

    /// <summary>
    /// 关闭指定 UI 界面
    /// </summary>
    public void CloseUI<T>() where T : BaseUI
    {
        string key = typeof(T).Name;
        if (_cache.TryGetValue(key, out var ui))
        {
            ui.Close();
        }
    }
}
}