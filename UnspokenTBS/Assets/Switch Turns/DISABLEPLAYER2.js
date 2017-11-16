#pragma strict

var Obj : GameObject;

function Start () {

    Obj.SetActive(false);

}

function OnTriggerEnter (trigger:Collider) { if(trigger.collider.tag=="Player1") Obj.SetActive (true); 


    DontDestroyOnLoad (gameObject);
}