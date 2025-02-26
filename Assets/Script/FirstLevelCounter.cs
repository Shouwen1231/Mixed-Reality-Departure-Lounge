using UnityEngine;
using TMPro;

public class FirstLevelCounter : MonoBehaviour
{
    public int counter = 4;               // Public 計數器的數字
    private const int minCounter = 0;      // 計數器的最小值
    public TextMeshProUGUI counterText;    // TextMesh Pro UI 元件

    void Start()
    {
        UpdateCounterText();
    }

    void OnTriggerEnter(Collider other)
    {
        // 根據標籤執行相應操作
        switch (other.tag)
        {
            case "Wheat":
                if (counter > minCounter) counter--;
                UpdateCounterText();
                break;
            case "Pig":
            case "Apple":
            case "Poop":
                // 只銷毀物體，不影響計數器
                break;
        }

        // 無論是 "Wheat" 或其他標籤，都銷毀物體
        Destroy(other.gameObject);
    }

    void UpdateCounterText()
    {
        counterText.text = counter.ToString();
    }
}
