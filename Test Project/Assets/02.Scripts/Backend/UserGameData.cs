[System.Serializable]
public class UserGameData
{
    public int level;
    public float experience;
    public int bread; //무료 재화
    public int corn; //유료 재화
    public int threadmill;

    public void Reset()
    {
        level = 1;
        experience = 0;
        bread = 0;
        corn = 0;
        threadmill = 10;
    }
}
