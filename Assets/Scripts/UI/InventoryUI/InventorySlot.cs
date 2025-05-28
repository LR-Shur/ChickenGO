namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using TMPro;

    [RequireComponent(typeof(Button))]
    public class InventorySlot : MonoBehaviour
    {
        [Header("UI 引用")]
        public Image icon;                 // 道具图标
        public TextMeshProUGUI nameText;         // 数量文本
        public Image highlightFrame;       // 高亮边框

        // 点击回调：参数是本 Slot
        public UnityEvent<InventorySlot> onClick;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(NotifyClick);
            highlightFrame.gameObject.SetActive(false);
        }

        private void NotifyClick()
        {
            onClick?.Invoke(this);
        }

        /// <summary> 初始化槽位显示 </summary>
        public void Setup(Sprite sprite, string name)
        {
            icon.sprite      = sprite;
            icon.enabled     = true;
            nameText.text   = name.ToString();
        }

        /// <summary> 清空槽位 </summary>
        public void Clear()
        {
            icon.enabled       = false;
            nameText.text     = "";
            Unhighlight();
        }

        /// <summary> 高亮显示 </summary>
        public void Highlight()
        {
            highlightFrame.gameObject.SetActive(true);
        }

        /// <summary> 取消高亮 </summary>
        public void Unhighlight()
        {
            highlightFrame.gameObject.SetActive(false);
        }
    }

}
