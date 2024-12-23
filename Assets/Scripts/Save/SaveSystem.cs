using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SaveScore(int points)
    {
        ES3.Save("Score", points);
    }

    public static int GetRecord()
    {
        return ES3.Load("Score", 0);
    }

}
