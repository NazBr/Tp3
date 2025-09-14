using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float cycleDuration = 60f; // Duration of a full day-night cycle in seconds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(1, 0, 0), (360 / cycleDuration) * Time.deltaTime);
        if (this.transform.eulerAngles.x >= 180)
        {
            this.GetComponent<Light>().intensity = 0.1f;
        }
        else
        {
            this.GetComponent<Light>().intensity = 1.1f;
        }
    }
}
