using System.Collections;
using UnityEngine;

public class FlyingMovement : MonoBehaviour
{
    private FlyingDeath _flyingDeath;
    
    [SerializeField] private float maxHeightY = 1.4f;
    [SerializeField] private float moveSpeed = 0.8f;

    private Vector2 _startPos;
    private Vector2 _highPos;
    private Vector2 _lowPos;
    private Vector2 _newPosition;


    private float _startTime;

    private bool _isLerping = true;

    private void Start()
    {
        _startPos = transform.position;
        _highPos = new Vector2(_startPos.x, _startPos.y + maxHeightY);
        _lowPos = new Vector2(_startPos.x, _startPos.y - maxHeightY);
        _newPosition = _highPos;
        _flyingDeath = GetComponent<FlyingDeath>();

        StartCoroutine(LerpPositionCoroutine());
    }

    private IEnumerator LerpPositionCoroutine()
    {
        while (!_flyingDeath.IsDead)
        {
            LerpPosition();
            yield return new WaitForFixedUpdate();
        }
    }

    private void LerpPosition()
    {
        if (_isLerping)
        {
            var lerpTime = Time.deltaTime * moveSpeed;

            var position = transform.position;
            var distanceToNewPosition = Vector2.Distance(position, _newPosition);
            var lerpFactor = Mathf.Clamp01(lerpTime / Vector2.Distance(position, _newPosition));
            
            var endPosition = Vector2.Lerp(position, _newPosition, lerpFactor);
            transform.position = endPosition;
            
            if (distanceToNewPosition < 0.01f)
            {
                _isLerping = false;
            }
        }
        if(_isLerping) return;
        var curPosition = transform.position;
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
