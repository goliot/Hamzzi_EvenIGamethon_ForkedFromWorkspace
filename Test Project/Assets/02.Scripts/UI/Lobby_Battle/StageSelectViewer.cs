using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectViewer : MonoBehaviour
{
    [Header ("# Objects")]
    public Button[] buttons;
    public Image[] starImages;
    public TextMeshProUGUI notEnoughStarsText;

    public Dictionary<Button, int> buttonsKey = new Dictionary<Button, int>();
    public Dictionary<Image, bool> starsKey = new Dictionary<Image, bool>();

    private int lastStage;
    private int[] starDatas = new int[20];
    private int[] stageStars = new int[5];

    public Color unlockColor;
    public Color unlockStarColor;
    public Color grayColor;

    public int chapter;

    public void Awake()
    {
        lastStage = BackendGameData.Instance.ClearData.lastClear;

        starDatas[0] = BackendGameData.Instance.StarData.c1s1;
        starDatas[1] = BackendGameData.Instance.StarData.c1s2;
        starDatas[2] = BackendGameData.Instance.StarData.c1s3;
        starDatas[3] = BackendGameData.Instance.StarData.c1s4;
        starDatas[4] = BackendGameData.Instance.StarData.c1s5;

        starDatas[5] = BackendGameData.Instance.StarData.c2s1;
        starDatas[6] = BackendGameData.Instance.StarData.c2s2;
        starDatas[7] = BackendGameData.Instance.StarData.c2s3;
        starDatas[8] = BackendGameData.Instance.StarData.c2s4;
        starDatas[9] = BackendGameData.Instance.StarData.c2s5;

        starDatas[10] = BackendGameData.Instance.StarData.c3s1;
        starDatas[11] = BackendGameData.Instance.StarData.c3s2;
        starDatas[12] = BackendGameData.Instance.StarData.c3s3;
        starDatas[13] = BackendGameData.Instance.StarData.c3s4;
        starDatas[14] = BackendGameData.Instance.StarData.c3s5;

        starDatas[15] = BackendGameData.Instance.StarData.c4s1;
        starDatas[16] = BackendGameData.Instance.StarData.c4s2;
        starDatas[17] = BackendGameData.Instance.StarData.c4s3;
        starDatas[18] = BackendGameData.Instance.StarData.c4s4;
        starDatas[19] = BackendGameData.Instance.StarData.c4s5;

        for (int i=0; i<buttons.Length; i++)
        {
            buttonsKey.Add(buttons[i], i);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) //전체 해금 치트
        {
            BackendGameData.Instance.ClearData.lastClear = 19;
            BackendGameData.Instance.ClearDataUpdate();
        }

        lastStage = BackendGameData.Instance.ClearData.lastClear;
        chapter = StageSelect.instance.chapter;

        switch (chapter)
        {
            case 1:
                for(int i=0; i<buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i;
                }
                for(int i=0; i<stageStars.Length; i++)
                {
                    stageStars[i] = starDatas[i];
                }
                break;
            case 2:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i + 5;
                }
                for(int i=0; i<stageStars.Length; i++)
                {
                    stageStars[i] = starDatas[i + 5];
                }
                break;
            case 3:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i + 10;
                }
                for(int i=0; i<stageStars.Length; i++)
                {
                    stageStars[i] = starDatas[i + 10];
                }
                break;
            case 4:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i + 15;
                }
                for(int i=0; i<stageStars.Length; i++)
                {
                    stageStars[i] = starDatas[i + 15];
                }
                break;
        }

        for(int i=0; i<5; i++)
        {
            if(buttonsKey[buttons[i]] > lastStage)
            {
                buttons[i].interactable = false;
                buttons[i].GetComponent<Image>().color = unlockColor;
            }
            else
            {
                buttons[i].interactable = true;
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }

        int starThisChapter = 0;
        for(int i=0; i<5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                switch (stageStars[i])
                {
                    case 0:
                        if (buttons[i].interactable == false) starImages[i * 3 + j].color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
                        else starImages[i * 3 + j].color = unlockStarColor;
                        break;
                    case 1:
                        if (j < 1)
                        {
                            starImages[i * 3 + j].color = new Color(1f, 1f, 1f, 1f);
                            starThisChapter++;
                        }
                        else starImages[i * 3 + j].color = unlockStarColor;
                        break;
                    case 2:
                        if (j < 2)
                        {
                            starImages[i * 3 + j].color = new Color(1f, 1f, 1f, 1f);
                            starThisChapter++;
                        }
                        else starImages[i * 3 + j].color = unlockStarColor;
                        break;
                    case 3:
                        if (j < 3)
                        {
                            starImages[i * 3 + j].color = new Color(1f, 1f, 1f, 1f);
                            starThisChapter++;
                        }
                        else starImages[i * 3 + j].color = unlockStarColor;
                        break;
                    default:
                        starImages[i * 3 + j].color = unlockStarColor;
                        break;
                }
            }
        }

        if (starThisChapter < 13) //5챕터가 깨져있지 않다면 스토리 버튼 비활성화 -> 이걸 수정하면 될듯?
        {
            buttons[5].interactable = false;
            buttons[5].GetComponent<Image>().color = unlockColor;

            notEnoughStarsText.gameObject.SetActive(true);
            notEnoughStarsText.text = "스토리가 궁금하다면\n별을 더 모아주세요!\n" + starThisChapter + " / 13";
        }
        else
        {
            buttons[5].interactable = true;
            buttons[5].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            notEnoughStarsText.gameObject.SetActive(false);
        }
    }
}
