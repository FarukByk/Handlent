using UnityEngine;
using System.Threading.Tasks;
using System;
public class myMath : MonoBehaviour
{
    //mal sait
    public static async void waitAndStart(float second, Action action)
    {
        await Task.Delay((int)(second * 1000));
        action?.Invoke();
    }

    public static void slowMoEf(float second)
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        waitAndStart(second, () => { Time.timeScale = 1; });
    }

    
}
