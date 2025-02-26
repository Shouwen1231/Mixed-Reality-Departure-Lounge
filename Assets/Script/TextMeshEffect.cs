using UnityEngine;
using TMPro;  // 引用 TextMeshPro 的命名空間
using System.Collections;

public class TextMeshProEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // 用來參考 TextMeshProUGUI 組件
    public float duration = 0.8f; // 變化持續時間

    private void OnEnable()
    {
        // 當物件啟用時，開始協程進行動畫效果
        StartCoroutine(AnimateMaterialAndScale());
    }

    private IEnumerator AnimateMaterialAndScale()
    {
        // 開始時間
        float startTime = Time.time;

        // 初始值
        Color initialColor = textMeshPro.color;
        Vector3 initialScale = textMeshPro.transform.localScale;

        // 取得 TMP 的材質
        Material material = textMeshPro.fontMaterial;

        while (Time.time < startTime + duration)
        {
            // 計算進度
            float progress = (Time.time - startTime) / duration;

            // 逐步改變 alpha (從 0 到 1)
            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(0f, 1f, progress);
            textMeshPro.color = newColor;

            // 逐步改變 scale (從 sin(0) 到 sin(90))
            float scaleFactor = Mathf.Sin(progress * Mathf.PI / 2); // Mathf.Sin(0) -> Mathf.Sin(90°)
            textMeshPro.transform.localScale = initialScale * scaleFactor;

            yield return null;
        }

        // 確保最後值為完全的目標狀態
        textMeshPro.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        textMeshPro.transform.localScale = initialScale * Mathf.Sin(Mathf.PI / 2); // 即 sin(90°)
    }
}