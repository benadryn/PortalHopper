using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Flying Idle", menuName = "Enemy Logic/Idle/Flying Idle")]
public class FlyingIdle : EnemyIdleSOBase
{
    [SerializeField] private float maxHeightY = 1.4f;
    [SerializeField] private float moveSpeed = 0.8f;

    private Vector2 _startPos;
    private Vector2 _highPos;
    private Vector2 _lowPos;
    private Vector2 _newPosition;
    
    private float _startTime;

    private bool _isLerping = true;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _startPos = Enemy.transform.position;
        _highPos = new Vector2(_startPos.x, _startPos.y + maxHeightY);
        _lowPos = new Vector2(_startPos.x, _startPos.y - maxHeightY);
        _newPosition = _highPos;
    }
    
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.Animator.SetBool(IsAttacking, false);

    }
    
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        Enemy.StartCoroutine(LerpPositionCoroutine());
    }
    private IEnumerator LerpPositionCoroutine()
    {
            LerpPosition();
            yield return new WaitForFixedUpdate();
    }
    
    private void LerpPosition()
    {
        if (_isLerping)
        {
            var lerpTime = Time.deltaTime * moveSpeed;

            var position = Enemy.transform.position;
            var distanceToNewPosition = Vector2.Distance(position, _newPosition);
            var lerpFactor = Mathf.Clamp01(lerpTime / Vector2.Distance(position, _newPosition));
            
            var endPosition = Vector2.Lerp(position, _newPosition, lerpFactor);
            Enemy.transform.position = endPosition;
            
            if (distanceToNewPosition < 0.01f)
            {
                _isLerping = false;
            }
        }

        if(_isLerping) return;
        var curPosition = Enemy.transform.position;
        var distanceToHighPos = (_highPos - (Vector2)curPosition).sqrMagnitude;
        var distanceToLowPos = (_lowPos - (Vector2)curPosition).sqrMagnitude;

        var sqrThreshold = 0.1f * 0.1f;
        
        
        if (distanceToHighPos < sqrThreshold)
        {
            _newPosition = _lowPos;
            _isLerping = true;
        }
        else if (distanceToLowPos < sqrThreshold)
        {
            _newPosition = _highPos;
            _isLerping = true;
        }

    }
    
}
