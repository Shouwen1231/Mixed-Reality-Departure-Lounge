using UnityEngine;
using TMPro;
using UnityEngine.Video;  // 用來控制 VideoPlayer

public class FirstLevelCondition : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // VideoPlayer 組件
    public TextMeshProUGUI counterText; // TextMeshPro 計數器
    public GameObject objectToDisable; // 當計數器不為0時需要禁用的物件
    public GameObject loseCanvas;     // 當計數器不為0時顯示的 LoseCanvas
    public GameObject objectToEnable; // 當計數器為0時需要啟用的物件

    private void Start()
    {
        // 註冊 VideoPlayer 播放結束事件
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        // 每幀檢查計數器的值
        if (float.TryParse(counterText.text, out float counterValue))
        {
            if (counterValue == 0)
            {
                // 計數器為 0 時，立即切換物件狀態
                objectToEnable.SetActive(true);
                objectToDisable.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("TextMeshPro counter value is not a valid number!");
        }
    }

    // 當 Video 播放結束時執行的事件
    private void OnVideoEnd(VideoPlayer vp)
    {
        // 計數器不為 0 時，禁用指定物件並顯示 LoseCanvas
        if (float.TryParse(counterText.text, out float counterValue))
        {
            if (counterValue != 0)
            {
                objectToDisable.SetActive(false);
                loseCanvas.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("TextMeshPro counter value is not a valid number!");
        }
    }

    private void OnDestroy()
    {
        // 確保在物件銷毀時取消註冊事件，防止記憶體洩漏
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}