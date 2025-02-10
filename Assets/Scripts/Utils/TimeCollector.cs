
using UnityEngine;
using System;

public class TimeCollector : MonoBehaviour
{
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;
    void FixedUpdate()
    {
        Year = System.DateTime.Now.Year;
        Month = System.DateTime.Now.Month;
        Day = System.DateTime.Now.Day;
        Hour = System.DateTime.Now.Hour;
        Minute = System.DateTime.Now.Minute;
        Second = System.DateTime.Now.Second;
    }
}
