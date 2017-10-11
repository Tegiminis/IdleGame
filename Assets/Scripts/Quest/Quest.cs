using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public List<QuestAction> events;
    public List<float> eventsWeight;

    public float resetTime = 360f;
    public float progress = 0.0f;
    public float progressToComplete = 100.0f;
    public bool active = false;
    public Char attachedChar;
    public ParticleSystem particle;
    QuestAction action = null;

    public float tickTimer;
    public float resetTimer;

    public Text txt_prefab;
    Text resetTxt;
    Camera cam;

    // Use this for initialization
    void Start ()
    {
        resetTimer = 0;
        cam = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (attachedChar != null && resetTimer <= 0)
        {
            if (!active)
            {
                action = Roll.ActionRoll(events, eventsWeight);
                action.StartAction(this);
                active = true;
            }

            if (tickTimer <= 0 && active)
            {
                action.DoAction(this);

                if (action.xp > 0)
                    particle.Emit(1);

                active = false;
            }

            tickTimer -= Time.deltaTime;
            
        }

        if (progress >= progressToComplete)
        {
            if (resetTxt == null)
            {
                resetTxt = Instantiate(txt_prefab, GameObject.Find("WorldCanvas").transform, false);
                resetTxt.transform.position = (Vector2)transform.position + new Vector2(0, 0.2f);
            }

            attachedChar.xp += 500;
            resetTimer = resetTime;

            progress = 0;
        }

        if (resetTimer > 0)
        {
            int sec = (int)resetTimer % 60;
            int min = ((int)resetTimer / 60) % 60;
            int hour = (int)resetTimer / 3600;
            if (resetTxt != null) { resetTxt.text = hour.ToString("D2") + ":"+ min.ToString("D2") + ":"+ sec.ToString("D2"); }
        }

        if (resetTimer <= 0 && resetTxt != null) { Destroy(resetTxt.gameObject); }

        resetTimer -= Time.deltaTime;
        resetTimer = Mathf.Max(resetTimer, 0);
    }
}
