#pragma strict


var Obj : GameObject;

function Start () {



}

function OnTriggerEnter (trigger:Collider) { if(trigger.collider.tag=="Player1") Obj.SetActive (false); 


    DontDestroyOnLoad (gameObject);
}