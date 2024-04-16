using UnityEngine;

public class MediumFrog : BaseFrog
{

    [SerializeField] private float frogShowTime = 8f;
    [SerializeField] private float minHiddenTime = 1f;
    [SerializeField] private float maxHiddenTime = 6f;
    [SerializeField] private float moveSpeed = 2.2f;
    [SerializeField] private int scoreAmount = 3;
    [SerializeField] private int health = 1;
    [SerializeField] private float damageToPlayer = 1;

    protected override void Initialize()
    {
        base.Initialize();
        FrogShowTime = frogShowTime;
        OriginalFrogShowTime = frogShowTime;
        TimeToResurfaceMin = minHiddenTime;
        TimeToResurfaceMax = maxHiddenTime;
        Health = health;
        StartHealth = health;
        ScoreAmount = scoreAmount;
        DamageToPlayer = damageToPlayer;
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
