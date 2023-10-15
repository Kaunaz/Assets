using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 30;
        timerText.text = "timer: " + timer.ToString();
    }

    public void runtimer(){
        timer -= Time.deltaTime;
        timerText.text = "timer: " + timer.ToString("f2");
    }

    public void resetTimer(){
        timer = 30;
        timerText.text = "timer: " + timer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
