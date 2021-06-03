using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {
    public GameObject CounterGameObject;
    public Text CounterText;
    public BombController bomb;
    private bool isUserPlaying = false;
    private bool isUserInRoom = false;
    private bool hasToEnd = false;
    private float Counter = 60f;

    private void Start() {
        CounterGameObject.SetActive(false);
        isUserInRoom = false;
        isUserPlaying = false;
        Counter = 60f;
    }

    private void Update() {
        if(isUserInRoom)
        {
            isUserInRoom = false;
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(PreRoomAnimation());
        }
        if(!isUserPlaying) return;

        if(bomb.isBombDeactivated) hasToEnd = true;


        // if(hasToEnd && Counter >= 0f) Playgoodend;
        // else if(hasToEnd && Counter < 0f) PlayBadEnd;

        Counter -= Time.deltaTime;
        CounterText.text = "Time to explode:\n" + (Counter * 10f / 10f) + " seconds";
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") isUserInRoom = true;
    }

    IEnumerator PreRoomAnimation ()
    {
        CounterGameObject.SetActive(true);
        yield return null;
    }
}