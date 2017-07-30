using UnityEngine;

public class DeathByTimer : MonoBehaviour {
    [SerializeField]
    private float deathTimeInSeconds;
    private float currentTimerValue;

	// Use this for initialization
	void Start () {
        currentTimerValue = deathTimeInSeconds;
	}
	
	// Update is called once per frame
	void Update () {
        currentTimerValue -= Time.deltaTime;
        if (currentTimerValue <= 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
	}

    public float DeathTime
    {
        set
        {
            currentTimerValue = value;
            deathTimeInSeconds = value;
        }
    }
}
