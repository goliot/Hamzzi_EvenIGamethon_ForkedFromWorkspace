using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    [System.Serializable]
    public class GameDataUpdateEvent : UnityEvent { }
    public GameDataUpdateEvent onGameDataUpdateEvent = new GameDataUpdateEvent();

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

    private ClearData clearData = new ClearData();
    public ClearData ClearData => clearData;

    private DogamData dogamData = new DogamData();
    public DogamData DogamData => dogamData;

    private TowerDB towerDB = new TowerDB();
    public TowerDB TowerDB => towerDB;

    private string gameDataRowInDate = string.Empty;
    private string clearDataRowInDate = string.Empty;
    private string towerDataRowInDate = string.Empty;
    private string dogamDataRowInDate = string.Empty;

    /// <summary>
    /// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가
    /// </summary>
    public void GameDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            {"level", userGameData.level },
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

                GameDataLoad();
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
                 //gameDataRowInDate = callback.GetInDate();

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
                    onGameDataUpdateEvent.Invoke();
                    //GameDataLoad();
                }
                else
                {
                    Debug.Log(gameDataRowInDate);
                    Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }

    public void ClearDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            {"c1s1", clearData.c1s1 },
            {"c1s2", clearData.c1s2 },
            {"c1s3", clearData.c1s3 },
            {"c1s4", clearData.c1s4 },
            {"c1s5", clearData.c1s5 },
            {"c2s1", clearData.c2s1 },
            {"c2s2", clearData.c2s2 },
            {"c2s3", clearData.c2s3 },
            {"c2s4", clearData.c2s4 },
            {"c2s5", clearData.c2s5 },
            {"c3s1", clearData.c3s1 },
            {"c3s2", clearData.c3s2 },
            {"c3s3", clearData.c3s3 },
            {"c3s4", clearData.c3s4 },
            {"c3s5", clearData.c3s5 },
            {"c4s1", clearData.c4s1 },
            {"c4s2", clearData.c4s2 },
            {"c4s3", clearData.c4s3 },
            {"c4s4", clearData.c4s4 },
            {"c4s5", clearData.c4s5 },
            {"lastClear", clearData.lastClear }
        };

        Backend.GameData.Insert("CLEAR_DATA", param, callback =>
        {
            if (callback.IsSuccess())
            {
                gameDataRowInDate = callback.GetInDate();

                Debug.Log($"클리어 정보 데이터 삽입에 성공했습니다. : {callback}");

                ClearDataLoad();
            }
            else
            {
                Debug.LogError($"클리어 정보 데이터 삽입에 실패했습니다. : {callback}");
            }
        });
    }

    public void ClearDataLoad()
    {
        Backend.GameData.GetMyData("CLEAR_DATA", new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"클리어 정보 데이터 불러오기에 성공했습니다 : {callback}");
                clearDataRowInDate = callback.GetInDate();
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                        ClearDataInsert(); //데이터가 없다는 거니까 새로 생성
                    }
                    else
                    {
                        clearDataRowInDate = gameDataJson[0]["inDate"].ToString();

                        clearData.c1s1 = int.Parse(gameDataJson[0]["c1s1"].ToString());
                        clearData.c1s2 = int.Parse(gameDataJson[0]["c1s2"].ToString());
                        clearData.c1s3 = int.Parse(gameDataJson[0]["c1s3"].ToString());
                        clearData.c1s4 = int.Parse(gameDataJson[0]["c1s4"].ToString());
                        clearData.c1s5 = int.Parse(gameDataJson[0]["c1s5"].ToString());
                        clearData.c2s1 = int.Parse(gameDataJson[0]["c2s1"].ToString());
                        clearData.c2s2 = int.Parse(gameDataJson[0]["c2s2"].ToString());
                        clearData.c2s3 = int.Parse(gameDataJson[0]["c2s3"].ToString());
                        clearData.c2s4 = int.Parse(gameDataJson[0]["c2s4"].ToString());
                        clearData.c2s5 = int.Parse(gameDataJson[0]["c2s5"].ToString());
                        clearData.c3s1 = int.Parse(gameDataJson[0]["c3s1"].ToString());
                        clearData.c3s2 = int.Parse(gameDataJson[0]["c3s2"].ToString());
                        clearData.c3s3 = int.Parse(gameDataJson[0]["c3s3"].ToString());
                        clearData.c3s4 = int.Parse(gameDataJson[0]["c3s4"].ToString());
                        clearData.c3s5 = int.Parse(gameDataJson[0]["c3s5"].ToString());
                        clearData.c4s1 = int.Parse(gameDataJson[0]["c4s1"].ToString());
                        clearData.c4s2 = int.Parse(gameDataJson[0]["c4s2"].ToString());
                        clearData.c4s3 = int.Parse(gameDataJson[0]["c4s3"].ToString());
                        clearData.c4s4 = int.Parse(gameDataJson[0]["c4s4"].ToString());
                        clearData.c4s5 = int.Parse(gameDataJson[0]["c4s5"].ToString());
                        clearData.lastClear = int.Parse(gameDataJson[0]["lastClear"].ToString());
                    }
                }
                catch (System.Exception e)
                {
                    clearData.Reset();
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"클리어 정보 데이터 불러오기에 실패했습니다 : {callback}");
            }
        });
    }

    public void ClearDataUpdate(UnityAction action = null)
    {
        if (clearData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
                           "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param()
        {
            {"c1s1", clearData.c1s1 },
            {"c1s2", clearData.c1s2 },
            {"c1s3", clearData.c1s3 },
            {"c1s4", clearData.c1s4 },
            {"c1s5", clearData.c1s5 },
            {"c2s1", clearData.c2s1 },
            {"c2s2", clearData.c2s2 },
            {"c2s3", clearData.c2s3 },
            {"c2s4", clearData.c2s4 },
            {"c2s5", clearData.c2s5 },
            {"c3s1", clearData.c3s1 },
            {"c3s2", clearData.c3s2 },
            {"c3s3", clearData.c3s3 },
            {"c3s4", clearData.c3s4 },
            {"c3s5", clearData.c3s5 },
            {"c4s1", clearData.c4s1 },
            {"c4s2", clearData.c4s2 },
            {"c4s3", clearData.c4s3 },
            {"c4s4", clearData.c4s4 },
            {"c4s5", clearData.c4s5 },
            {"lastClear", clearData.lastClear }
        };

        // 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
        if (string.IsNullOrEmpty(clearDataRowInDate))
        {
            Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        // 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
        // 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
        else
        {
            Debug.Log($"{clearDataRowInDate}의 클리어 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("CLEAR_DATA", clearDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"클리어 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();
                    //ClearDataLoad();
                }
                else
                {
                    Debug.LogError($"클리어 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }

    public void DogamDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            {"m0", dogamData.m0 },
            {"m1", dogamData.m1 },
            {"m2", dogamData.m2 },
            {"m3", dogamData.m3 },
            {"m4", dogamData.m4 },
            {"m5", dogamData.m5 },
            {"m6", dogamData.m6 },
            {"m7", dogamData.m7 },
            {"m8", dogamData.m8 },
            {"m9", dogamData.m9 },
            {"m10", dogamData.m10 },
            {"m11", dogamData.m11 },
            {"m12", dogamData.m12 },
            {"m13", dogamData.m13 },
            {"m14", dogamData.m14 },
            {"m15", dogamData.m15 },
            {"m16", dogamData.m16 },
            {"m17", dogamData.m17 },
            {"m18", dogamData.m18 },
            {"m19", dogamData.m19 }
        };

        Backend.GameData.Insert("DOGAM_INFO", param, callback =>
        {
            if (callback.IsSuccess())
            {
                dogamDataRowInDate = callback.GetInDate();

                Debug.Log($"도감 정보 데이터 삽입에 성공했습니다. : {callback}");

                DogamDataLoad();
            }
            else
            {
                Debug.LogError($"도감 정보 데이터 삽입에 실패했습니다. : {callback}");
            }
        });
    }

    public void DogamDataLoad()
    {
        Backend.GameData.GetMyData("DOGAM_INFO", new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"도감 정보 데이터 불러오기에 성공했습니다 : {callback}");

                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                        DogamDataInsert(); //데이터가 없다는 거니까 새로 생성
                    }
                    else
                    {
                        dogamDataRowInDate = gameDataJson[0]["inDate"].ToString();

                        dogamData.m0 = bool.Parse(gameDataJson[0]["m0"].ToString());
                        dogamData.m1 = bool.Parse(gameDataJson[0]["m1"].ToString());
                        dogamData.m2 = bool.Parse(gameDataJson[0]["m2"].ToString());
                        dogamData.m3 = bool.Parse(gameDataJson[0]["m3"].ToString());
                        dogamData.m4 = bool.Parse(gameDataJson[0]["m4"].ToString());
                        dogamData.m5 = bool.Parse(gameDataJson[0]["m5"].ToString());
                        dogamData.m6 = bool.Parse(gameDataJson[0]["m6"].ToString());
                        dogamData.m7 = bool.Parse(gameDataJson[0]["m7"].ToString());
                        dogamData.m8 = bool.Parse(gameDataJson[0]["m8"].ToString());
                        dogamData.m9 = bool.Parse(gameDataJson[0]["m9"].ToString());
                        dogamData.m10 = bool.Parse(gameDataJson[0]["m10"].ToString());
                        dogamData.m11 = bool.Parse(gameDataJson[0]["m11"].ToString());
                        dogamData.m12 = bool.Parse(gameDataJson[0]["m12"].ToString());
                        dogamData.m13 = bool.Parse(gameDataJson[0]["m13"].ToString());
                        dogamData.m14 = bool.Parse(gameDataJson[0]["m14"].ToString());
                        dogamData.m15 = bool.Parse(gameDataJson[0]["m15"].ToString());
                        dogamData.m16 = bool.Parse(gameDataJson[0]["m16"].ToString());
                        dogamData.m17 = bool.Parse(gameDataJson[0]["m17"].ToString());
                        dogamData.m18 = bool.Parse(gameDataJson[0]["m18"].ToString());
                        dogamData.m19 = bool.Parse(gameDataJson[0]["m19"].ToString());
                    }
                }
                catch (System.Exception e)
                {
                    dogamData.Reset();
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"도감 정보 데이터 불러오기에 실패했습니다 : {callback}");
            }
        });
    }

    public void DogamDataUpdate(UnityAction action = null)
    {
        if (clearData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
                           "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param()
        {
            {"m0", dogamData.m0 },
            {"m1", dogamData.m1 },
            {"m2", dogamData.m2 },
            {"m3", dogamData.m3 },
            {"m4", dogamData.m4 },
            {"m5", dogamData.m5 },
            {"m6", dogamData.m6 },
            {"m7", dogamData.m7 },
            {"m8", dogamData.m8 },
            {"m9", dogamData.m9 },
            {"m10", dogamData.m10 },
            {"m11", dogamData.m11 },
            {"m12", dogamData.m12 },
            {"m13", dogamData.m13 },
            {"m14", dogamData.m14 },
            {"m15", dogamData.m15 },
            {"m16", dogamData.m16 },
            {"m17", dogamData.m17 },
            {"m18", dogamData.m18 },
            {"m19", dogamData.m19 }
        };

        // 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
        if (string.IsNullOrEmpty(dogamDataRowInDate))
        {
            Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        // 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
        // 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
        else
        {
            Debug.Log($"{dogamDataRowInDate}의 도감 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("DOGAM_INFO", dogamDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"도감 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();
                    //DogamDataLoad();
                }
                else
                {
                    Debug.LogError($"도감 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }

    public void TowerDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            {"t0", towerDB.t0 },
            {"t1", towerDB.t1 },
            {"t2", towerDB.t2 },
            {"t3", towerDB.t3 },
            {"t4", towerDB.t4 },
        };

        Backend.GameData.Insert("TOWER_DATA", param, callback =>
        {
            if (callback.IsSuccess())
            {
                towerDataRowInDate = callback.GetInDate();

                Debug.Log($"타워 정보 데이터 삽입에 성공했습니다. : {callback}");

                TowerDataLoad();
            }
            else
            {
                Debug.LogError($"타워 정보 데이터 삽입에 실패했습니다. : {callback}");
            }
        });
    }

    public void TowerDataLoad()
    {
        Backend.GameData.GetMyData("TOWER_DATA", new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"타워 정보 데이터 불러오기에 성공했습니다 : {callback}");

                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                        TowerDataInsert(); //데이터가 없다는 거니까 새로 생성
                    }
                    else
                    {
                        towerDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        
                        towerDB.t0 = bool.Parse(gameDataJson[0]["t0"].ToString());
                        towerDB.t1 = bool.Parse(gameDataJson[0]["t1"].ToString());
                        towerDB.t2 = bool.Parse(gameDataJson[0]["t2"].ToString());
                        towerDB.t3 = bool.Parse(gameDataJson[0]["t3"].ToString());
                        towerDB.t4 = bool.Parse(gameDataJson[0]["t4"].ToString());
                    }
                }
                catch (System.Exception e)
                {
                    towerDB.Reset();
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"타워 정보 데이터 불러오기에 실패했습니다 : {callback}");
            }
        });
    }

    public void TowerDataUpdate(UnityAction action = null)
    {
        if (clearData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
                           "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param()
        {
            {"t0", towerDB.t0 },
            {"t1", towerDB.t1 },
            {"t2", towerDB.t2 },
            {"t3", towerDB.t3 },
            {"t4", towerDB.t4 },
        };

        // 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
        if (string.IsNullOrEmpty(towerDataRowInDate))
        {
            Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        // 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
        // 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
        else
        {
            Debug.Log($"{towerDataRowInDate}의 타워 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("TOWER_DATA", towerDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"타워 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();
                    //TowerDataLoad();
                }
                else
                {
                    Debug.LogError($"타워 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }
}