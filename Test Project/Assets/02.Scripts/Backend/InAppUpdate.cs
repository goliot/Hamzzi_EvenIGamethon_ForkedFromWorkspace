using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;

public class InAppUpdate : MonoBehaviour
{
    AppUpdateManager appUpdateManager;

    private void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("인앱 업데이트 실행");
#elif UNITY_ANDROID
        StartCoroutine(CheckForUpdate());
#endif
    }

    IEnumerator CheckForUpdate()
    {
        appUpdateManager = new AppUpdateManager();

        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoOperation;

        if(appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            //업데이트 가능
            if(appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                //즉시 업데이트(업데이트 안하면 실행 불가)
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);
                yield return startUpdateRequest;

                while(!startUpdateRequest.IsDone)
                {
                    if(startUpdateRequest.Status == AppUpdateStatus.Downloading)
                    {
                        Debug.Log("업데이트 다운로드가 진행중입니다.");
                    }
                    else if(startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                    {
                        Debug.Log("업데이트가 완료되었습니다.");
                    }
                    yield return null;
                }

                var result = appUpdateManager.CompleteUpdate();
                while (!result.IsDone)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return (int)startUpdateRequest.Status;
            }
            //업데이트가 없는 경우
            else if(appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                Debug.Log("업데이트 없음");
                yield return (int)UpdateAvailability.UpdateAvailable;
            }
            else
            {
                Debug.Log("정보 없음");
                yield return (int)UpdateAvailability.Unknown;
            }
        }
        else
        {
            //appUpdateInfoOperation.Error 기록
            Debug.Log("Error!");
        }
    }
}
