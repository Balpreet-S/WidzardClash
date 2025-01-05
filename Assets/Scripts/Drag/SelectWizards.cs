using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWizards : MonoBehaviour
{
    public GameObject Wizards;
    public bool check;
    //public GameObject Wizards { get => wizards;}

    // Start is called before the first frame update
    void Start()
    {
        Wizards.SetActive(false);
    }

    // Update is called once per frame
    public void Update(){
        if(Input.GetMouseButtonDown(0)){
            if(check){
                Deactivate();
            }
            else{
                Activate();
            }
        }
    }

    public void Activate(){
        Wizards.SetActive(true);
        check = true;
    }

    public void Deactivate(){
        Wizards.SetActive(false);
        check = false;
    }
}
