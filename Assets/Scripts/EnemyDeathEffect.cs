using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    public void OnCompleteAnimation() {
        Destroy(this.gameObject);
    }
}
