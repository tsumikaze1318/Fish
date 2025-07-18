using Cysharp.Threading.Tasks;
using UnityEngine;

public class TimingCatcher : MonoBehaviour
{
    // タイムアップまでの時間
    private const float TIME_UP = 1f;

    public async UniTask<float> MeasureTime()
    {
        float timeline = 0f;

        while (timeline < TIME_UP)
        {
            timeline += Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                return timeline;
            }

            await UniTask.Yield();
        }

        return timeline;
    }
}
