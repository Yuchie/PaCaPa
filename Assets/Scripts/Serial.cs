using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Serial : MonoBehaviour {

    SerialPort stream = new SerialPort("\\\\.\\COM13", 9600);

    public bool free;

    // Use this for initialization
    void Start () {
        stream.Open();
        Debug.Log(stream.IsOpen);
        stream.ReadTimeout = 1;
        free = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!free)
        {
            string reply = null;
            try
            {
                reply = stream.ReadLine();
            }
            catch (System.Exception)
            {

            }

            if (reply != null)
            {
                free = true;
            }
        }

    }

    public bool CheckFree()
    {
        return free;
    }

    public SerialPort ReturnStream()
    {
        return stream;
    }

    public void ChangeFree()
    {
        free = true;
    }

    public void WriteToArduino(string message)
    {
        if (message == "")
        {
            free = true;
        } else
        {
            free = false;
        }
        message += "f";
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    public void CloseStream()
    {
        if (stream.IsOpen)
        {
            stream.Close();
        }
    }
}
