using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    GameManager gameManager;
    public void GetItem() {
        Destroy(this.gameObject);
        gameManager.AddScore(100);
    }
}
