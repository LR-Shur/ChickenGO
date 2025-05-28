using UnityEngine;

namespace UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        // 面板打开时的初始化，可在子类 override
        public virtual void Show() 
        { 
            gameObject.SetActive(true); 
        }

        // 面板关闭时的清理
        public virtual void Close() 
        { 
            gameObject.SetActive(false); 
        }
    }

}
