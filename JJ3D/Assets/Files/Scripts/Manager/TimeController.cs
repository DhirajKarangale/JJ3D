using TMPro;
using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Day")]
    [SerializeField] Light dayLight;
    [SerializeField] Color colorDay;
    [SerializeField] Material skyBoxDay;
    [SerializeField] float mxDayLightInt;
    [SerializeField] float sunRiseHour;
    private TimeSpan sunRiseTime;

    [Header("Night")]
    [SerializeField] Light moonLight;
    [SerializeField] Color colorNight;
    [SerializeField] Material skyBoxNight;
    [SerializeField] float mxNightLightInt;
    [SerializeField] float sunSetHour;
    private TimeSpan sunSetTime;

    [Header("Extra")]
    [SerializeField] TMP_Text txtTime;
    [SerializeField] AnimationCurve curveLightChange;
    [SerializeField] float startHour;
    private DateTime currTime;
    private float timeMultiplier;
    private Msg msg;
    private int currDay;
    public static bool isDay;


    private void Start()
    {
        msg = Msg.instance;
        currTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunRiseTime = TimeSpan.FromHours(sunRiseHour);
        sunSetTime = TimeSpan.FromHours(sunSetHour);

        isDay = true;
        currDay = 0;
        msg.DisplayMsg($"Day {currDay}", Color.white);
    }

    private void Update()
    {
        UpdateTime();
        RotateSun();
        UpdateLightSetting();
    }


    private void UpdateTime()
    {
        currTime = currTime.AddSeconds(Time.deltaTime * timeMultiplier);
        txtTime.text = currTime.ToString("hh:mm");
    }

    private void RotateSun()
    {
        float lightRotation;

        if (currTime.TimeOfDay > sunRiseTime && currTime.TimeOfDay < sunSetTime)
        {
            if (!isDay)
            {
                currDay++;
                msg.DisplayMsg($"Day {currDay}", Color.white);
            }
            isDay = true;

            TimeSpan sunRiseSeunSetDuration = TimeDiff(sunRiseTime, sunSetTime);
            TimeSpan timeSinceSunRise = TimeDiff(sunRiseTime, currTime.TimeOfDay);

            double percentage = timeSinceSunRise.TotalMinutes / sunRiseSeunSetDuration.TotalMinutes;
            lightRotation = Mathf.Lerp(0, 180, (float)percentage);

            RenderSettings.skybox = skyBoxDay;

            timeMultiplier = 144;
        }
        else
        {
            isDay = false;

            TimeSpan sunRiseSeunSetDuration = TimeDiff(sunRiseTime, sunSetTime);
            TimeSpan timeSinceSunSet = TimeDiff(sunSetTime, currTime.TimeOfDay);

            double percentage = timeSinceSunSet.TotalMinutes / sunRiseSeunSetDuration.TotalMinutes;
            lightRotation = Mathf.Lerp(180, 360, (float)percentage);

            RenderSettings.skybox = skyBoxNight;

            timeMultiplier = (720) / (currDay + 1);
        }

        dayLight.transform.rotation = Quaternion.AngleAxis(lightRotation, Vector3.right);
    }

    private void UpdateLightSetting()
    {
        float dotProduct = Vector3.Dot(dayLight.transform.forward, Vector3.down);
        dayLight.intensity = Mathf.Lerp(0, mxDayLightInt, curveLightChange.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(mxNightLightInt, 0, curveLightChange.Evaluate(dotProduct));

        RenderSettings.ambientLight = Color.Lerp(colorNight, colorDay, curveLightChange.Evaluate(dotProduct));
    }

    private TimeSpan TimeDiff(TimeSpan from, TimeSpan to)
    {
        TimeSpan diff = to - from;

        if (diff.TotalSeconds < 0)
        {
            diff += TimeSpan.FromHours(24);
        }

        return diff;
    }
}
