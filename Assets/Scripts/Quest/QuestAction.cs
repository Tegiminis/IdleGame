using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAction : MonoBehaviour
{
    public float xp;
    public float xpMultiplier;
    public string fluffText;
    public float damage;
    public float progress;
    public float timeToComplete;

    public void StartAction(Quest quest)
    {
        quest.tickTimer = timeToComplete;
    }

    public void DoAction (Quest quest)
    {
        quest.attachedChar.xp += xp;
        quest.attachedChar.health -= damage;
        quest.progress += progress;
        quest.attachedChar.actionLog.Add(quest.name + ": " + fluffText);
    }
}
