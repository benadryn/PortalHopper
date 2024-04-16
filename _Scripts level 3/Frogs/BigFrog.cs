using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrog : BaseFrog
{

    [SerializeField] private float frogShowTime = 8f;
    [SerializeField] private float minHiddenTime = 1f;
    [SerializeField] private float maxHiddenTime = 6f;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private int health = 2;
    [SerializeField] private int scoreAmount = 5;


    protected override void Initialize()
    {
        base.Initialize();
        FrogShowTime = frogShowTime;
        OriginalFrogShowTime = frogShowTime;
        TimeToResurfaceMin = minHiddenTime;
        TimeToResurfaceMax = maxHiddenTime;
        Health = health;
        ScoreAmount = scoreAmount;
    }

    private void Update()
    {
        FrogShowHide();

        if (!IsHidden)
        {
            MoveTowardsPlayer(moveSpeed);
        }
    }
    
    
}
