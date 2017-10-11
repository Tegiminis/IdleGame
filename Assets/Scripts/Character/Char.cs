using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Char : MonoBehaviour
{
    //Base character class. Contains basic attributes and behaviors

    public float health = 1f;
    public float healthMax = 10f;
    public float healthRegen = 1.0f;
    public bool recovering = false;
    public bool regenerating = true;
    public float xp = 0.0f;
    public int level = 1;

    public CharManager roster;
    SpringJoint2D spring;

    private Vector3 screenPoint;
    private Vector3 offset;
    public bool dragging = false;
    public bool viewing = false;
    public Quest attachedQuest = null;

    public List<string> actionLog;
    public Text txt_prefab;
    bool created = false;
    List<Text> actionLogUI;
    public Camera cam;
    public Text hpTxt;
    public Text xpTxt;
    public Text lvlTxt;
    // Use this for initialization
    void Start ()
    {
        actionLog = new List<string>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Level up (basic)
		if (xp >= 500 * level)
        {
            level++;
            xp = 0;

        }

        //Regen health
        if (regenerating)
        {
            health += healthRegen * Time.deltaTime;
        }

        health = Mathf.Min(health, healthMax);

        if (Input.GetButton("Select") && dragging)
        {
            float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
            transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Info")) { dragging = false; viewing = !viewing; }
        if (Input.GetButtonDown("Select"))
        {
            dragging = true;
            viewing = false;
            if (attachedQuest != null) { attachedQuest.attachedChar = null; }
            attachedQuest = null;
        }
    }

    void OnMouseUp()
    {
        dragging = false;

        Collider2D[] results = new Collider2D[1];
        int arraySize = Physics2D.OverlapCollider(transform.GetComponent<Collider2D>(), new ContactFilter2D(), results);
        results = new Collider2D[arraySize];
        Physics2D.OverlapCollider(transform.GetComponent<Collider2D>(), new ContactFilter2D(), results);

        for (int i = 0; i < results.Length; i++)
        {
            Quest quest = results[i].GetComponent<Quest>();

            if (quest != null)
            {
                transform.position = quest.transform.position;
                quest.attachedChar = this;
                attachedQuest = quest;
            }
        }

        if (attachedQuest == null) { transform.position = roster.rosterPos[0].transform.position; }
    }

    private void OnGUI()
    {
        if (viewing)
        {
            if (!created)
            {
                actionLogUI = new List<Text>();

                hpTxt = Instantiate(txt_prefab, GameObject.Find("ScreenCanvas").transform, false);
                hpTxt.transform.position = (Vector2)hpTxt.transform.position + new Vector2(0, 0.4f);

                xpTxt = Instantiate(txt_prefab, GameObject.Find("ScreenCanvas").transform, false);
                xpTxt.transform.position = (Vector2)xpTxt.transform.position + new Vector2(1.5f, 0.4f);

                lvlTxt = Instantiate(txt_prefab, GameObject.Find("ScreenCanvas").transform, false);
                lvlTxt.transform.position = (Vector2)lvlTxt.transform.position + new Vector2(3.0f, 0.4f);

                for (int i = 0; i < actionLog.Count; i++)
                {
                    Text logTxt = Instantiate(txt_prefab, GameObject.Find("ScreenCanvas").transform, false);
                    logTxt.text = actionLog[i];
                    logTxt.transform.position = (Vector2)logTxt.transform.position + new Vector2(0, i * -0.4f);

                    actionLogUI.Add(logTxt);
                }

                created = true;
            }

            hpTxt.text = "HP: " + (int)health;
            xpTxt.text = "XP: " + (int)xp;
            lvlTxt.text = "LV: " + (int)level;

            if (actionLogUI.Count != actionLog.Count)
            {
                Text logTxt = Instantiate(txt_prefab, GameObject.Find("ScreenCanvas").transform, false);
                logTxt.text = actionLog[actionLog.Count - 1];
                actionLogUI.Add(logTxt);
                logTxt.transform.position = (Vector2)logTxt.transform.position + new Vector2(0, (actionLogUI.Count - 1) * -0.4f);
            }
        }

        if (!viewing)
        {
            created = false;

            if (actionLogUI != null)
            {
                for (int i = actionLogUI.Count - 1; i >= 0; i--)
                {
                    Destroy(actionLogUI[i].gameObject);
                    actionLogUI.RemoveAt(i);
                    actionLog.RemoveAt(i);
                }
            }

            if (hpTxt != null) { Destroy(hpTxt.gameObject); }
            if (xpTxt != null) { Destroy(xpTxt.gameObject); }
            if (lvlTxt != null) { Destroy(lvlTxt.gameObject); }

            actionLogUI = null;
        }
    }
}
