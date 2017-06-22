//basic class to get random number between 2 given nums
public class IntRange
{
    public int m_Min;   //min value in this range
    public int m_Max;   //max value in this range


    //constructor
    public IntRange(int min, int max)
    {
        m_Min = min;
        m_Max = max;
    }


    //get a rand val in range
    public int Random
    {
        get
        {
            return UnityEngine.Random.Range(m_Min, m_Max);
        }
    }
}