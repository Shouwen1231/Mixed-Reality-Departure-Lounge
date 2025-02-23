using UnityEngine;
using System.Collections;

public class PoopGenerated : MonoBehaviour
{
    public GameObject poopPrefab;  // Poop 的 Prefab，設置在 Inspector 中
    public GameObject rangeObject; // 定義範圍的 GameObject（它應該有 Collider）
    public AudioClip poopSound;    // 設定 Poop 生成時播放的音效

    private void OnTriggerEnter(Collider other)
    {
        // 檢查進入範圍的物件是否有 Apple 或 Wheat 標籤
        if (other.CompareTag("Apple") || other.CompareTag("Wheat"))
        {
            // 在指定 GameObject 的範圍內隨機生成 Poop 物件
            Vector3 randomPosition = GetRandomPositionInCollider(rangeObject.GetComponent<Collider>());
            
            // 設定生成的 Poop 物件的旋轉，使其 x 方向為 -90
            Quaternion rotation = Quaternion.Euler(-90, 0, 0); // 使 x 旋轉為 -90 度

            // 生成 Poop 物件
            GameObject poop = Instantiate(poopPrefab, randomPosition, rotation);

            // 播放 Poop 生成的音效
            PlayPoopSound(poop);

            // 開始進行縮放動畫
            StartCoroutine(ScaleOverTime(poop));

            // 銷毀進入的物件
            Destroy(other.gameObject);
        }
    }

    // 根據指定的 GameObject 的 Collider 來生成隨機位置
    private Vector3 GetRandomPositionInCollider(Collider collider)
    {
        // 獲取 Collider 的邊界
        Bounds bounds = collider.bounds;

        // 隨機生成一個位置，限制在範圍內
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        // 返回隨機生成的位置
        return new Vector3(randomX, randomY, randomZ);
    }

    // 在 0.5 秒內將物件的縮放從 sin(0) 到 sin(90)
    private IEnumerator ScaleOverTime(GameObject poop)
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // 縮放時間 0.5 秒
        Vector3 initialScale = poop.transform.localScale;

        while (elapsedTime < duration)
        {
            float scaleFactor = Mathf.Sin((elapsedTime / duration) * Mathf.PI / 2); // sin(0) ~ sin(90) 之間
            poop.transform.localScale = initialScale * scaleFactor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最後確保縮放為 sin(90) = 1
        poop.transform.localScale = initialScale;
    }

    // 播放 Poop 生成的音效
    private void PlayPoopSound(GameObject poop)
    {
        // 確保 Poop 物件上有 AudioSource 組件
        AudioSource audioSource = poop.GetComponent<AudioSource>();

        // 如果 Poop 物件沒有 AudioSource，則添加一個
        if (audioSource == null)
        {
            audioSource = poop.AddComponent<AudioSource>();
        }

        // Debug 日誌，檢查是否播放音效
        Debug.Log("Playing Poop sound...");
        
        // 播放指定的音效
        audioSource.PlayOneShot(poopSound);
    }
}
