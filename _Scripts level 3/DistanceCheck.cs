using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DistanceCheck : MonoBehaviour
{
    [SerializeField] private GameObject cookieGo;
    private LineRenderer _lineRenderer;
    private Transform _startPos;

    private void Start()
    {
        EventManager.Instance.OnCookieSpawned += UpdateCookieGo;
        EventManager.Instance.OnDistanceSent += UpdateDistance;
        _lineRenderer = GetComponent<LineRenderer>();
        if (!cookieGo)
        {
            Debug.Log($"Add Initial cookie to {name}, child of {transform.parent.gameObject.name}");
        }

        _startPos = transform;
        
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _startPos.position);
        _lineRenderer.SetPosition(1, _startPos.position);
    }

    private void Update()
    {
        transform.rotation = cookieGo.transform.rotation;
    }

    private void UpdateDistance(float distance)
    {
        var newPos = _startPos.position + _startPos.up * distance;
        _lineRenderer.SetPosition(1, newPos);
    }

    private void UpdateCookieGo(GameObject cookie)
    {
        cookieGo = cookie;
        _startPos = cookie.transform;
        var position = _startPos.position;
        _lineRenderer.SetPosition(0, position);
        _lineRenderer.SetPosition(1, position);
    }
}