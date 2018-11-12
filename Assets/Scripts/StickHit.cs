using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHit : MonoBehaviour {

    public Serial serial;
    public CalcGodStick calcGodStick;

    [Range(0, 90)] public int degree;

    void Start()
    {
        degree = 1;
    }

    void Update()
    {
        int next_degree = calcGodStick.UpdateGodStick();
        if (serial.CheckFree() && (degree != next_degree))
        {
            serial.WriteToArduino(next_degree.ToString());
            degree = next_degree;
        }

        if(!calcGodStick.GetHit() && degree != 0){
            serial.ChangeFree();
            serial.WriteToArduino("0");
            degree = 0;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.tag != "Stage")
        {
            int tag = 0;
            switch(collider.gameObject.tag)
            {
                case "Collider1":
                    tag = 1;
                    break;
                case "Collider2":
                    tag = 2;
                    break;
                case "Collider3":
                    tag = 3;
                    break;
                default:
                    tag = 0;
                    break;
            }

            if(tag == 0)
            {
                // calcGodStick.ChangeTarget(collider.gameObject);
            } else {
                calcGodStick.ChangeStatus(tag);
            }
            calcGodStick.SetControllerHit(true);

            if (!calcGodStick.GetHit()) {
                calcGodStick.SetHit(true);
                calcGodStick.InitAttachPoint();
            }
        }
        
    }

    void OnTriggerStay(Collider collider)
    {

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag != "Stage")
        {
            if (collider.gameObject.tag == "Soft")
            {
                calcGodStick.SetControllerHit(false);
            }
        }
    }
}
