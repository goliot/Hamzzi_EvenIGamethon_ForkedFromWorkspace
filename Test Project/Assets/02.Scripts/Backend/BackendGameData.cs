using System;
using UnityEngine;
using BackEnd;
using UnityEngine.Events;
using System.Threading.Tasks;

public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new BackendGameData();
            }

            return instance;
        }
    }

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowInDate = string.Empty;

    /// <summary>
    /// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가
    /// </summary>
    public void GameDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            {"level", userGameData.level },
            {"experience", userGameData.experience },
            {"bread", userGameData.bread },
            {"corn", userGameData.corn},
            {"threadmill", userGameData.threadmill}
        };

        Backend.GameData.Insert("USER_DATA", param, callback =>
        {
            if(callback.IsSuccess())
            {
                gameDataRowInDate = callback.GetInDate();

                Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {callback}");

                onGameDataLoadEvent?.Invoke();
            }
            else
            {
                Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {callback}");
            }
        });
    }

    /// <summary>
    /// 뒤끝 콘솔 테이블에서 유저 정보를 불러옴
    /// </summary>
    public void GameDataLoad()
     {
         Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
         {
             if (callback.IsSuccess())
             {
                 Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다 : {callback}");

                 try
                 {
                     LitJson.JsonData gameDataJson = callback.FlattenRows();

                     if(gameDataJson.Count <= 0)
                     {
                         Debug.LogWarning("데이터가 존재하지 않습니다");
                         GameDataInsert(); //데이터가 없다는 거니까 새로 생성
                     }
                     else
                     {
                         gameDataRowInDate = gameDataJson[0]["inDate"].ToString();

                         userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                         userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
                         userGameData.bread = int.Parse(gameDataJson[0]["bread"].ToString());
                         userGameData.corn = int.Parse(gameDataJson[0]["corn"].ToString());
                         userGameData.threadmill = int.Parse(gameDataJson[0]["threadmill"].ToString());

                         onGameDataLoadEvent?.Invoke();
                     }
                 }
                 catch(System.Exception e)
                 {
                     userGameData.Reset();
                     Debug.LogError(e);
                 }
             }
             else
             {
                 Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다 : {callback}");
             }
         });
     }

    /*public Task GameDataLoad() //비동기 호출방식
    {
        var tcs = new TaskCompletionSource<bool>();

        Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
        {
            try
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다 : {callback}");

                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                        GameDataInsert(); // 데이터가 없다는 거니까 새로 생성
                    }
                    else
                    {
                        gameDataRowInDate = gameDataJson[0]["inDate"].ToString();

                        userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
                        userGameData.bread = int.Parse(gameDataJson[0]["bread"].ToString());
                        userGameData.corn = int.Parse(gameDataJson[0]["corn"].ToString());
                        userGameData.threadmill = int.Parse(gameDataJson[0]["threadmill"].ToString());
                    }
                    onGameDataLoadEvent?.Invoke();
                    tcs.SetResult(true);
                }
                else
                {
                    Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다 : {callback}");
                    //GameDataInsert(); // 데이터가 없다는 거니까 새로 생성
                    tcs.SetResult(false);
                }
            }
            catch (System.Exception e)
            {
                userGameData.Reset();
                Debug.LogError(e);
                tcs.SetResult(false);
            }
        });
        return tcs.Task;
    }*/


    /// <summary>
    /// 유저 데이터 갱신 -> 상점 구매 등에서 활용할 함수
    /// </summary>
    /// <param name="action"></param>
    public void GameDataUpdate(UnityAction action = null)
    {
        if(userGameData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
                           "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param()
        {
            {"level", userGameData.level },
            {"experience", userGameData.experience },
            {"bread", userGameData.bread },
            {"corn", userGameData.corn},
            {"threadmill", userGameData.threadmill}
        };

        // 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        // 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
        // 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();
                    GameDataLoad();
                }
                else
                {
                    Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }
}