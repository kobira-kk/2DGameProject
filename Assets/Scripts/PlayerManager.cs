using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;
    public enum DIRECTION_TYPE {
        STOP, RIGHT, LEFT
    }
    // 初期値
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    // 自身の物理演算
    Rigidbody2D rigidbody2D;
    Animator animator;
    float speed;
    float jumpPower = 400;
    bool isDeath = false;

    // SE
    [SerializeField] AudioClip getItemsSE;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip stampSE;
    AudioSource audioSource;

    private void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update() {
        if (isDeath){
            return;
        }
        // 横方向への移動
        float x = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0) {
            // 停止
            direction = DIRECTION_TYPE.STOP;
        } else if (x > 0) {
            // 右方向
            direction = DIRECTION_TYPE.RIGHT;
        } else if (x < 0) {
            // 左方向
            direction = DIRECTION_TYPE.LEFT;
        }

        // スペース押されたらジャンプ
        if (IsGround()) {
            if (Input.GetKeyDown("space")) {
                Jump();
            } else {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void FixedUpdate() {
        switch(direction) {
            case DIRECTION_TYPE.STOP:
                speed = 0f;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 3f;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -3f;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        // 入力に応じた速度を入力
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    void Jump() {
        audioSource.PlayOneShot(jumpSE);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isJumping", true);

    }

    bool IsGround() {
        // 始点と終点の作成
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer) || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    // トリガーの判定
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Trap") {
            Debug.Log("ゲームオーバー");      
            PlayerDeath();
        }

        if (other.gameObject.tag == "Finish") {
            Debug.Log("ゲームクリア");
            gameManager.GameClear(); 
        }

        if (other.gameObject.tag == "Item") {
            Debug.Log("アイテムゲット");
            audioSource.PlayOneShot(getItemsSE);
            other.gameObject.GetComponent<ItemManager>().GetItem();
        }

        if (other.gameObject.tag == "Enemy") {
            EnamyManager enemy = other.gameObject.GetComponent<EnamyManager>();

            if (this.transform.position.y + 0.2f > enemy.transform.position.y) {
                // 上から踏んだ
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                audioSource.PlayOneShot(stampSE);
                enemy.DestoryEnemy();

            } else {
                // 横からぶつかった
                PlayerDeath();
                
            }
        }
    }

    void PlayerDeath() {
        isDeath = true;
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.Play("PlayerDeath");
        BoxCollider2D boxcollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxcollider2D);
        gameManager.GameOver();

    }
}
