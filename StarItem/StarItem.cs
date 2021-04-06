using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class StarItem : MonoBehaviour,IReusable
{
    private int x;
    public int X { get => x; set => x = value; }
    private int y;
    public int Y { get => y; set => y = value; }

    public Explodable _explodable;
    public StarEnum starType;

    public void Init(int x, int y, StarEnum type)
    {
        X = x;
        Y = y;
        starType = type;
        _explodable = GetComponent<Explodable>();
    }

    private void OnMouseDown()
    {
        EventCenter.Broadcast(EventDefine.RemoveStarItem, gameObject);
    }

    public void Move(int newX,int newY)
    {
        X = newX;
        Y = newY;
        Vector2 vec = new Vector2(X - 3, Y - 5) * 0.75f;

        transform.DOMove(vec, 0.3f);
    }

    public void OnSpawn()
    {
       
    }

    public void OnUnSpawn()
    {

    }
}
