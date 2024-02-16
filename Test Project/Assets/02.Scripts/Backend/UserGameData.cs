[System.Serializable]
public class UserGameData
{
    public int level;
    public int bread; //레벨업시 사용하는 것 -> 경험치 개념
    public int corn; //유료 재화
    public int threadmill;
    public bool isAdRemoved;

    public int[] levelUpData = { 50, 60, 70, 80, 90, 110, 130, 150, 170, 190, 220, 250, 280, 310, 340, 370, 400, 430, 460, 500 };
    public int[] cornCostToLevelUp = { 120, 160, 200, 240, 280, 330, 380, 430, 460, 510, 570, 630, 690, 750, 810, 880, 950, 1020, 1090, 1200 };
    public int[] damageUpgradeAmount = { 0, 3, 6, 9, 12, 19, 22, 25, 28, 31, 38, 41, 44, 47, 50, 58, 61, 64, 67, 70, 78 };

    public void Reset()
    {
        level = 1;
        bread = 0;
        corn = 0;
        threadmill = 10;
        isAdRemoved = false;
    }
}

[System.Serializable]
public class ClearData
{
    public int lastClear; //0이면 깬거 없는거 -> 1이 C1S1
    //1 -> 1-1, 2->1-2 .... 20 -> 4-5

    public void Reset()
    {
        lastClear = 0;
    }
}

[System.Serializable]
public class DogamData //마주친 적 있는지 여부
{
    public bool m0;
    public bool m1;
    public bool m2;
    public bool m3;
    public bool m4;
    public bool m5;
    public bool m6;
    public bool m7;
    public bool m8;
    public bool m9;
    public bool m10;
    public bool m11;
    public bool m12;
    public bool m13;
    public bool m14;
    public bool m15;
    public bool m16;
    public bool m17;
    public bool m18;
    public bool m19;

    public void Reset()
    {
        m0 = false;
        m1 = false;
        m2 = false; 
        m3 = false;
        m4 = false;
        m5 = false;
        m6 = false;
        m7 = false;
        m8 = false;
        m9 = false;
        m10 = false;
        m11 = false;
        m12 = false;
        m13 = false;
        m14 = false;
        m15 = false;
        m16 = false;
        m17 = false;
        m18 = false;
        m19 = false;
    }
}

[System.Serializable]
public class TowerDB
{
    public bool t0;
    public bool t1;
    public bool t2;
    public bool t3;
    public bool t4;

    public void Reset()
    {
        t0 = false;
        t1 = false;
        t2 = false;
        t3 = false;
        t4 = false;
    }
}

[System.Serializable]
public class StarData
{
    public int c1s1;
    public int c1s2;
    public int c1s3;
    public int c1s4;
    public int c1s5;
    public int c2s1;
    public int c2s2;
    public int c2s3;
    public int c2s4;
    public int c2s5;
    public int c3s1;
    public int c3s2;
    public int c3s3;
    public int c3s4;
    public int c3s5;
    public int c4s1;
    public int c4s2;
    public int c4s3;
    public int c4s4;
    public int c4s5;

    public void Reset()
    {
        c1s1 = 0;
        c1s2 = 0;
        c1s3 = 0;
        c1s4 = 0;
        c1s5 = 0;
        c2s1 = 0;
        c2s2 = 0;
        c2s3 = 0;
        c2s4 = 0;
        c2s5 = 0;
        c3s1 = 0;
        c3s2 = 0;
        c3s3 = 0;
        c3s4 = 0;
        c3s5 = 0;
        c4s1 = 0;
        c4s2 = 0;
        c4s3 = 0;
        c4s4 = 0;
        c4s5 = 0;
    }
}