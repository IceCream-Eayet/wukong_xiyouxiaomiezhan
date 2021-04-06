using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour,IReusable
{
    public void OnSpawn()
    {
        
    }

    public void OnUnSpawn()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Monster") 
        {
            ObjectPool.Instance.UnSpawn(gameObject);
        }
    }

}
