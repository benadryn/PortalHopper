using System.Collections;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    
    private PlayerAnimator _player;

    private bool _isFacingRight;


    private void Awake()
    {
        _player = playerTransform.gameObject.GetComponent<PlayerAnimator>();
    }

    public void CallTurn()
    {
        if (_turnCoroutine != null)
        {
            StopCoroutine(_turnCoroutine);
        }
        
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position;
    }

    private IEnumerator FlipYLerp()
    {
        var startRotation = transform.localEulerAngles.y;
        var endRotation = DetermineEndRotation();

        var elapsedTime = 0f;

        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            var yRotation = Mathf.Lerp(startRotation, endRotation, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
        
    }

    private float DetermineEndRotation()
    {       
        _isFacingRight = _player.IsFacingRight;
        
        return _isFacingRight ? 180f : 0f;
    }
}
