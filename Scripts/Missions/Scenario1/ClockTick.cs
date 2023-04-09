using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTick : MonoBehaviour
{
    public GameObject SecondHand;
    public GameObject MinuteHand;
    public GameObject HourHand;
    public float StartTime;
    public float CurrentTime;

    private float AccumulatedTime;
    private float Seconds;
    private float Minutes;
    private float Hours;
    private const float DegreesPerSecond = 360.0f / 60.0f;
    private const float DegreesPerMinute = 360.0f / 3600.0f;
    private const float DegreesPerHour = 360.0f / 43200.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartTime = Mathf.Clamp(StartTime, 0.0f, 43200.0f);
        CurrentTime = StartTime;

        Seconds = StartTime % 60.0f;
        Minutes = StartTime % 3600.0f;
        Hours = StartTime % 43200.0f;

        SecondHand.transform.localRotation = Quaternion.Euler(SecondHand.transform.localRotation.x, Seconds * DegreesPerSecond, SecondHand.transform.localRotation.z);
        MinuteHand.transform.localRotation = Quaternion.Euler(MinuteHand.transform.localRotation.x, Minutes * DegreesPerMinute, MinuteHand.transform.localRotation.z);
        HourHand.transform.localRotation = Quaternion.Euler(HourHand.transform.localRotation.x, Hours * DegreesPerHour, HourHand.transform.localRotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        AccumulatedTime += Time.deltaTime;

        if (AccumulatedTime >= 1.0f)
        {
            CurrentTime += AccumulatedTime;
            AccumulatedTime = 0.0f;

            Seconds = CurrentTime % 60.0f;
            Minutes = CurrentTime % 3600.0f;
            Hours = CurrentTime % 43200.0f;

            SecondHand.transform.localRotation = Quaternion.Euler(SecondHand.transform.localRotation.x, Seconds * DegreesPerSecond, SecondHand.transform.localRotation.z);
            MinuteHand.transform.localRotation = Quaternion.Euler(MinuteHand.transform.localRotation.x, Minutes * DegreesPerMinute, MinuteHand.transform.localRotation.z);
            HourHand.transform.localRotation = Quaternion.Euler(HourHand.transform.localRotation.x, Hours * DegreesPerHour, HourHand.transform.localRotation.z);
        }
    }
}
