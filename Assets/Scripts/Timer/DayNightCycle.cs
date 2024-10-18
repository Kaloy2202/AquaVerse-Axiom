using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;  // Assign the directional light (sun) in the Inspector
    public float secondsInAFullDay = 24f;  // Real seconds for a full in-game day (24 seconds = 1 second = 1 hour)

    private float _currentTimeOfDay = 0f;  // Keeps track of current in-game time
    private float _sunInitialIntensity;    // Store the initial sun intensity

    void Start()
    {
        _sunInitialIntensity = sun.intensity;  // Store the sun's starting intensity
    }

    void Update()
    {
        // Increment the current time of day based on the real-time passed
        _currentTimeOfDay += (Time.deltaTime / secondsInAFullDay) * 24f;

        // Loop back time after a full day (24 hours)
        if (_currentTimeOfDay >= 24f)
        {
            _currentTimeOfDay = 0f;
        }

        // Update the sun’s position and intensity based on the time of day
        UpdateSun();
    }

    void UpdateSun()
    {
        // Calculate the sun's angle. 0 hours = 0 degrees (midnight), 12 hours = 180 degrees (noon), etc.
        float sunAngle = (_currentTimeOfDay / 24f) * 360f;

        // Rotate the sun based on the calculated angle
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunAngle - 90f, 0f, 0f));  // Adjust Y-axis if needed

        // Adjust the sun's intensity depending on time of day (dimmer at night)
        float intensityMultiplier = 1f;

        if (_currentTimeOfDay <= 6f || _currentTimeOfDay >= 18f)
        {
            // If it's early morning or late evening, gradually reduce the sun's intensity
            intensityMultiplier = Mathf.Clamp01(1f - ((6f - Mathf.Abs(_currentTimeOfDay - 12f)) / 6f));
        }

        sun.intensity = _sunInitialIntensity * intensityMultiplier;
    }
}
