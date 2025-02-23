using UnityEngine;
using System.Collections;


public class PigGenerated : MonoBehaviour
{
    private int appleCount = 0;
    private int wheatCount = 0;

    public GameObject pigPrefab;  // 用於指定生成 Pig 的物件
    public GameObject rangeObject;  // 用來指定生成範圍的物件
    public Vector3 rangeOffset = Vector3.zero; // 可選的偏移量，用於調整生成位置
    public AudioClip pigSound;  // 用於指定豬的生成音效

    // 當物件進入觸發器時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            appleCount++; // 如果進來的是 Apple，數量 +1
            Destroy(other.gameObject); // 銷毀 Apple 物件
        }

        if (other.CompareTag("Wheat"))
        {
            wheatCount++; // 如果進來的是 Wheat，數量 +1
            Destroy(other.gameObject); // 銷毀 Wheat 物件
        }
        
        // 檢查條件是否符合：至少 1 個 Apple 和 3 個 Wheat
        if (appleCount >= 1 && wheatCount >= 3)
        {
            SpawnPig(); // 符合條件，生成 Pig
        }
    }

    // 當物件離開觸發器時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            appleCount--; // 如果離開的是 Apple，數量 -1
        }

        if (other.CompareTag("Wheat"))
        {
            wheatCount--; // 如果離開的是 Wheat，數量 -1
        }
    }

    // 生成 Pig 物件的方法，隨機生成在指定範圍內
    private void SpawnPig()
    {
        if (pigPrefab != null && rangeObject != null)
        {
            // 獲取範圍物件的位置和大小
            Vector3 rangeCenter = rangeObject.transform.position;
            Vector3 rangeSize = rangeObject.GetComponent<Renderer>().bounds.size;

            // 隨機生成範圍內的 X、Y 和 Z 坐標
            float randomX = Random.Range(rangeCenter.x - rangeSize.x / 2, rangeCenter.x + rangeSize.x / 2);
            float randomY = Random.Range(rangeCenter.y - rangeSize.y / 2, rangeCenter.y + rangeSize.y / 2);
            float randomZ = Random.Range(rangeCenter.z - rangeSize.z / 2, rangeCenter.z + rangeSize.z / 2);

            // 計算隨機位置
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ) + rangeOffset;

            // 設置旋轉角度為 -90 度，旋轉沿著 X 軸
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);

            // 在隨機位置生成 Pig，並設置旋轉
            GameObject pig = Instantiate(pigPrefab, randomPosition, rotation);

            // 扣除相應數量的 Apple 和 Wheat
            appleCount--; // 扣除 1 個 Apple
            wheatCount -= 3; // 扣除 3 個 Wheat

            // 播放音效
            PlayPigSound(randomPosition); // 在生成豬的地方播放音效

            // 啟動縮放動畫
            StartCoroutine(ScalePig(pig)); // 呼叫協程來進行縮放
        }
    }

    // 協程來處理 Pig 的縮放動畫
    private IEnumerator ScalePig(GameObject pig)
    {
        Vector3 originalScale = pig.transform.localScale;
        float elapsedTime = 0f;
        float duration = 0.5f; // 動畫持續時間為 0.5 秒

        // 縮放從 sin(0) = 0 到 sin(90) = 1
        while (elapsedTime < duration)
        {
            // 計算當前縮放的比例
            float scaleValue = Mathf.Sin((elapsedTime / duration) * Mathf.PI / 2); // 使用 sin(0) 到 sin(90) 之間的變化
            pig.transform.localScale = originalScale * scaleValue;

            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一幀
        }

        // 確保縮放到最終值
        pig.transform.localScale = originalScale; // 最終縮放恢復為正常大小
    }

    // 播放豬的生成音效
    private void PlayPigSound(Vector3 position)
    {
        if (pigSound != null)
        {
            AudioSource.PlayClipAtPoint(pigSound, position); // 在指定位置播放音效
        }
        else
        {
            Debug.LogWarning("未指定音效文件！"); // 如果音效文件未指定，顯示警告
        }
    }
}
