using UnityEngine;
using TMPro;
using UnityEngine.Video;  // 用來控制 VideoPlayer

public class SecondLevelCondition : MonoBehaviour
{
    public VideoPlayer videoPlayer;    // VideoPlayer 組件
    public TextMeshProUGUI WheatCounter; // 原來的計數器，現在改名為 WheatCounter
    public TextMeshProUGUI PigCounter;  // 新增的 PigCounter 計數器
    public GameObject objectToDisable; // 當計數器不為0時需要禁用的物件
    public GameObject loseCanvas;      // 當計數器不為0時顯示的 LoseCanvas
    public GameObject objectToEnable;  // 當 PigCounter 和 WheatCounter 都為 0 時需要啟用的物件

    private void Start()
    {
        // 註冊 VideoPlayer 播放結束事件
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        // 每幀檢查 WheatCounter 和 PigCounter 的值
        if (float.TryParse(WheatCounter.text, out float wheatValue) && float.TryParse(PigCounter.text, out float pigValue))
        {
            if (wheatValue == 0 && pigValue == 0)
            {
                // 當兩個計數器都為 0 時，啟用物件
                objectToEnable.SetActive(true);
                objectToDisable.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("TextMeshPro counter values are not valid numbers!");
        }
    }

    // 當 Video 播放結束時執行的事件
    private void OnVideoEnd(VideoPlayer vp)
    {
        // 當 WheatCounter 和 PigCounter 皆不為 0，或者一方為 0 另一方不為 0 時，禁用指定物件並顯示 LoseCanvas
        if (float.TryParse(WheatCounter.text, out float wheatValue) && float.TryParse(PigCounter.text, out float pigValue))
        {
            if ((wheatValue != 0 && pigValue != 0) || (wheatValue == 0 && pigValue != 0) || (pigValue == 0 && wheatValue != 0))
            {
                objectToDisable.SetActive(false);
                loseCanvas.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("TextMeshPro counter values are not valid numbers!");
        }
    }

    private void OnDestroy()
    {
        // 確保在物件銷毀時取消註冊事件，防止記憶體洩漏
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
