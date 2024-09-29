using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchKidComponent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.CaughtKid());
        }
    }
}
