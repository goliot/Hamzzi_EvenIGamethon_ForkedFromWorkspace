//쳇바퀴 원본

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Threadmill : MonoBehaviour
{
    public static Threadmill instance;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI threadmillText;

    public int m_HeartAmount = 0; //보유 하트 개수
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    private const int MAX_HEART = 10; //하트 최대값
    public int HeartRechargeInterval = 1200;// 하트 충전 간격(단위:초)
    private Coroutine m_RechargeTimerCoroutine = null;
    private int m_RechargeRemainTime = 0;

    private void Awake()
    {
        Init();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            timeText = GameObject.Find("StaminaTime").GetComponent<TextMeshProUGUI>();
            threadmillText = GameObject.Find("StaminaText").GetComponent<TextMeshProUGUI>();

            timeText.text = ConvertToMinutesAndSeconds(m_RechargeRemainTime);
            threadmillText.text = m_HeartAmount.ToString() + " / 10";
        }
        //BackendGameData.Instance.UserGameData.threadmill = m_HeartAmount;
    }

    string ConvertToMinutesAndSeconds(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("다음 충전까지\n{0:00} : {1:00}", minutes, seconds);
    }

    //게임 초기화, 중간 이탈, 중간 복귀 시 실행되는 함수
    public void OnApplicationFocus(bool value)
    {
        Debug.Log("OnApplicationFocus() : " + value);
        if (value)
        {
            LoadHeartInfo();
            LoadAppQuitTime();
            SetRechargeScheduler();
        }
        else
        {
            SaveHeartInfo();
            SaveAppQuitTime();
            if (m_RechargeTimerCoroutine != null) { StopCoroutine(m_RechargeTimerCoroutine); }
        }
    }
    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        Debug.Log("GoodsRechargeTester: OnApplicationQuit()");
        SaveHeartInfo();
        SaveAppQuitTime();
    }
    //버튼 이벤트에 이 함수를 연동한다.
    public void OnClickUseHeart()
    {
        Debug.Log("OnClickUseHeart");
        UseHeart();
    }

    public void Init()
    {
        m_HeartAmount = 0;
        m_RechargeRemainTime = 0;
        m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
        Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
        //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
    }
    public bool LoadHeartInfo()
    {
        Debug.Log("LoadHeartInfo");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("HeartAmount"))
            {
                Debug.Log("PlayerPrefs has key : HeartAmount");
                m_HeartAmount = PlayerPrefs.GetInt("HeartAmount");
                if (m_HeartAmount < 0)
                {
                    m_HeartAmount = 0;
                }
            }
            else
            {
                m_HeartAmount = 0;
            }
            //heartAmountLabel.text = m_HeartAmount.ToString();
            Debug.Log("Loaded HeartAmount : " + m_HeartAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveHeartInfo()
    {
        Debug.Log("SaveHeartInfo");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("HeartAmount", m_HeartAmount);
            PlayerPrefs.Save();
            //BackendGameData.Instance.UserGameData.threadmill = m_HeartAmount;
            //BackendGameData.Instance.GameDataUpdate();
            Debug.Log("Saved HeartAmount : " + m_HeartAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool LoadAppQuitTime()
    {
        Debug.Log("LoadAppQuitTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                Debug.Log("PlayerPrefs has key : AppQuitTime");
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));
            }
            else { Debug.Log("현재시간 불러옴"); m_AppQuitTime = DateTime.Now.ToLocalTime(); }
            Debug.Log(string.Format("Loaded AppQuitTime : {0}", m_AppQuitTime.ToString()));
            //appQuitTimeLabel.text = string.Format("AppQuitTime : {0}", m_AppQuitTime.ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveAppQuitTime()
    {
        Debug.Log("SaveAppQuitTime");
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.SetInt("RemainTime", m_RechargeRemainTime);
            PlayerPrefs.Save();
            Debug.Log("Saved AppQuitTime : " + DateTime.Now.ToLocalTime().ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }
    public void SetRechargeScheduler(Action onFinish = null)
    {
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);
        Debug.Log("TimeDifference In Sec :" + timeDifferenceInSec + "s");
        int origintimeDifferenceInSec = timeDifferenceInSec; int remainTime = 0;
        int heartToAdd;
        if (timeDifferenceInSec > 0)
        {
            timeDifferenceInSec = PlayerPrefs.GetInt("RemainTime") - timeDifferenceInSec;
            if (timeDifferenceInSec <= 0)
            {
                m_HeartAmount++;
                timeDifferenceInSec = Mathf.Abs(timeDifferenceInSec);
                heartToAdd = timeDifferenceInSec / HeartRechargeInterval;
                Debug.Log("Heart to add : " + heartToAdd);
                if (heartToAdd == 0) remainTime = HeartRechargeInterval - timeDifferenceInSec;
                else remainTime = HeartRechargeInterval - (timeDifferenceInSec % HeartRechargeInterval);
            }
            else
            {
                heartToAdd = timeDifferenceInSec / HeartRechargeInterval;
                Debug.Log("Heart to add : " + heartToAdd);
                if (heartToAdd == 0) remainTime = PlayerPrefs.GetInt("RemainTime") - origintimeDifferenceInSec;
            }
            Debug.Log("RemainTime : " + remainTime);
            m_HeartAmount += heartToAdd;
        }
        else if (timeDifferenceInSec < 0) Debug.Log("선생님? 시간여행자이십니까?");

        if (m_HeartAmount >= MAX_HEART)
        {
            //m_HeartAmount = MAX_HEART;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        Debug.Log("HeartAmount : " + m_HeartAmount);
    }
    public void UseHeart(Action onFinish = null)
    {
        if (m_HeartAmount <= 0)
        {
            return;
        }

        m_HeartAmount--;
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        if (m_RechargeTimerCoroutine == null)
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval));
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        Debug.Log("DoRechargeTimer");
        if (remainTime <= 0)
        {
            m_RechargeRemainTime = HeartRechargeInterval;
        }
        else
        {
            m_RechargeRemainTime = remainTime;
        }
        Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
        //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);

        while (m_RechargeRemainTime > 0)
        {
            Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
            //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
            m_RechargeRemainTime -= 1;

            if(m_HeartAmount >= MAX_HEART)
            {
                //m_HeartAmount = MAX_HEART;
                m_RechargeRemainTime = 0;
                //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
                Debug.Log("HeartAmount reached max amount");
                m_RechargeTimerCoroutine = null;
            }
            yield return new WaitForSeconds(1f);
        }
        m_HeartAmount++;
        if (m_HeartAmount >= MAX_HEART)
        {
            //m_HeartAmount = MAX_HEART;
            m_RechargeRemainTime = 0;
            //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
            Debug.Log("HeartAmount reached max amount");
            m_RechargeTimerCoroutine = null;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval, onFinish));
        }
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        Debug.Log("HeartAmount : " + m_HeartAmount);
    }
}