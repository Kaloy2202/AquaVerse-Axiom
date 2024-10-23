using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;  // Assign the directional light (sun) in the Inspector
    public AudioManager audioManager; // Reference to AudioManager

    private float _currentTimeOfDay = 0f;  // Keeps track of current in-game time
    private float _sunInitialIntensity;    // Store the initial sun intensity
    private bool isDaytime = true;         // Track if it's currently daytime

    private float secondsInAFullDay;
    private SceneMngrState sceneMngrState;

    void Start()
    {
        _sunInitialIntensity = sun.intensity;  // Store the sun's starting intensity
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        secondsInAFullDay = sceneMngrState.getNumberOfSecondsPerHour() * 24;
        _currentTimeOfDay = sceneMngrState.getStartingtime();
        UpdateSun();
        // Start with playing daytime music
        audioManager.Play("AmbientSound");
    }

    void Update()
    {
        // Increment the current time of day based on real-time passed
        _currentTimeOfDay += (Time.deltaTime / secondsInAFullDay);

        // Loop back time after a full day (24 hours)
        if (_currentTimeOfDay >= 24f)
        {
            _currentTimeOfDay = 0f;
        }

        // Update the sun's position and intensity based on time of day
        UpdateSun();

        // Check if we need to switch music (day to night, or night to day)
        CheckTimeForMusicChange();
    }

    void UpdateSun()
    {
        // Calculate the sun's angle. 0 hours = 0 degrees (midnight), 12 hours = 180 degrees (noon)
        float sunAngle = _currentTimeOfDay * 360f;

        // Rotate the sun based on the calculated angle
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));  // Adjust Y-axis if needed

        // Adjust the sun's intensity depending on the time of day (dimmer at night)
        float intensityMultiplier = 1f;

        if (_currentTimeOfDay <= 6f || _currentTimeOfDay >= 18f)
        {
            // If it's early morning or late evening, gradually reduce the sun's intensity
            intensityMultiplier = Mathf.Clamp01(1f - ((6f - Mathf.Abs(_currentTimeOfDay - 12f)) / 6f));
        }

        sun.intensity = _sunInitialIntensity * intensityMultiplier;
    }

    void CheckTimeForMusicChange()
    {
        // Define "daytime" between 6:00 and 18:00
        if (_currentTimeOfDay >= 6f && _currentTimeOfDay < 18f && !isDaytime)
        {
            isDaytime = true; // Switch to day
            audioManager.Play("DaytimeMusic"); // Play daytime music
        }
        else if ((_currentTimeOfDay < 6f || _currentTimeOfDay >= 18f) && isDaytime)
        {
            isDaytime = false; // Switch to night
            audioManager.Play("NighttimeMusic"); // Play nighttime music
        }
    }
}
