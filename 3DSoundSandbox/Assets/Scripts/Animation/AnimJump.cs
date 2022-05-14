using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimJump : MonoBehaviour
{
    public float jumpHeight;
    public float idleDuration;
    public float jumpDuration;
    private float _timer, _jumpTimer1;
    private Vector3 _jumpApexPosition;
    private Vector3 _jumpIdlePosition;
    public AnimationCurve jumpCurve;
    private float _currentWaitDuration;
    private void Start()
    {
        _jumpApexPosition = transform.position + Vector3.up * jumpHeight;
        _jumpIdlePosition = transform.position;
        _currentWaitDuration = Random.Range(idleDuration*0.75f, idleDuration * 1.25f);

    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _currentWaitDuration)
        {
            _jumpTimer1 += Time.deltaTime;
            transform.position = Vector3.Lerp(_jumpIdlePosition, _jumpApexPosition, jumpCurve.Evaluate( _jumpTimer1/jumpDuration));
            if (!(_jumpTimer1/jumpDuration >= 1)) return;
            _jumpTimer1 = 0;
            _timer = 0;
            _currentWaitDuration = Random.Range(idleDuration*0.75f, idleDuration * 1.25f);
        }
    }
}