using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnamyManager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameObject deathEffect;
    public enum DIRECTION_TYPE {
        STOP, RIGHT, LEFT
    }
    // 初期値
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    // 自身の物理演算
    Rigidbody2D rigidbody2D;
    float speed;

    private void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = DIRECTION_TYPE.RIGHT;
    }
    private void Update() {

        if (!IsGround()) {
            // 反転させる
            ChangeDirection();
        }
    }

    private void FixedUpdate() {
        switch(direction) {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -3;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        // 入力に応じた速度を入力
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    bool IsGround() {
        Vector3 startVec = transform.position + transform.right * 0.5f * transform.localScale.x;
        Vector3 endVec = startVec - transform.up * 0.5f;
        Debug.DrawLine(startVec, endVec);
        return Physics2D.Linecast(startVec, endVec, blockLayer);
    }

    void ChangeDirection() {
        if (direction == DIRECTION_TYPE.RIGHT) {
            direction = DIRECTION_TYPE.LEFT;
        } else if (direction == DIRECTION_TYPE.LEFT) {
            direction = DIRECTION_TYPE.RIGHT;
        }
    }

    public void DestoryEnemy() {
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
