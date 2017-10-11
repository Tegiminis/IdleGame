using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    // Rolls on a table of items and weights
    public static QuestAction ActionRoll (List<QuestAction> table, List<float> weights)
    {
        int tableSize = table.Count;
        float weightTotal = 0f;

        float roll;
        float rollSuccess;

        for (int i = tableSize - 1; i >= 0; i--)
        {
            weightTotal += weights[i];
        }

        for (int a = 0; a < tableSize; a++)
        {
            roll = Random.value; // The actual dice roll
            rollSuccess = weights[a] / weightTotal; // Roll greater than this to succeed

            if (roll > rollSuccess)
            {
                weightTotal -= weights[a];
            }

            if (roll < rollSuccess)
            {
                return table[a];
            }
        }

        return null;
    }
}
