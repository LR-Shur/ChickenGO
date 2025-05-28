// Scripts/Core/CameraFollow.cs
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("要跟随的目标")]
    public Transform target;

    [Header("平滑跟随参数")]
    [Range(0f, 1f)] public float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // 目标位置（保持Z轴不变）
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 平滑插值
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
