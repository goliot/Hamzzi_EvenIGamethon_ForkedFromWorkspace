using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    private void Awake()
    {
        base.Initialize_DontDestroyOnLoad();
    }

    public void ChangeScene(int i)
    {
        StartCoroutine(Loading(i));
    }

    IEnumerator Loading(int i)
    {
        yield return SceneManager.LoadSceneAsync(2);            // 백그라운드에서 비동기로 씬을 로드
        AsyncOperation op = SceneManager.LoadSceneAsync(i);     // 코루틴의 진행 상황을 확인하기 위함
        op.allowSceneActivation = false;                        // 씬 로딩이 끝나면 바로 활성화 되게 하는 불 값 => false

        Slider slider = FindObjectOfType<Slider>();

        float targetProgress = 0.9f;                            // 로딩이 멈출 지점의 진행률
        float smoothTimeInitial = 0.5f;                         // 초기에 로딩 바가 찰 때의 보간에 사용되는 시간
        float smoothTimeFinal = 0.2f;                           // 90% 이후로 진행될 때의 보간에 사용되는 시간
        float velocity = 0.0f;                                  // 보간에 사용되는 속도

        while (!op.isDone)
        {
            float currentProgress = op.progress / 0.9f;                                         // 현재 프레임의 진행률 계산

            // slider.value = Mathf.Lerp(slider.value, currentProgress, Time.deltaTime * 7f);   // Lerp로 선형 보간

            // 초기에 로딩 바가 찰 때의 보간
            if (currentProgress < targetProgress)
            {
                slider.value = Mathf.SmoothDamp(slider.value, targetProgress, ref velocity, smoothTimeInitial);
            }
            // 90% 이후로 진행될 때의 보간
            else
            {
                slider.value = Mathf.SmoothDamp(slider.value, currentProgress, ref velocity, smoothTimeFinal);
            }

            Debug.Log(slider.value);

            if (Mathf.Approximately(slider.value, 1.0f))
            {
                yield return new WaitForSeconds(1.5f);          // Debug용 딜레이 로딩완료되면 1초 딜레이 후 씬 활성화
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
