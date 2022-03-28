using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour
{
    public BirdTrigger[] birdTriggers;

    public enum State
    {
        Idle,
        Lift,
        GotoPoint
    }

    private Vector3 _startPosition;
    private Vector3 _gotoPosition;
    private float _gotoDuration;
    private float _stateTime;

    public AnimationCurve gotoXCurve;
    public AnimationCurve gotoYCurve;
    public AnimationCurve gotoZCurve;
    public AnimationCurve liftCurve;
    public State currentState;
    public float liftHeight;
    public float liftDuration;

    private BirdTrigger _currentBirdTrigger;

    
    //TODO: Fly loop
    // [EventRef]
    // public string flyLoop = "";
    // private FMOD.Studio.EventInstance _flyLoopInstance;
    //
    // [EventRef]
    // public string land = "";
    // private FMOD.Studio.EventInstance _landInstance;
    //
    // [EventRef]
    // public string takeOff = "";
    // private FMOD.Studio.EventInstance _takeOffInstance;
     private Rigidbody _rigidbody;
    private void Start()
    {
        // TryGetComponent(out _rigidbody);
        // _flyLoopInstance = RuntimeManager.CreateInstance(flyLoop);
        // _flyLoopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        // _takeOffInstance = RuntimeManager.CreateInstance(takeOff);
        // _takeOffInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        //
        // _landInstance = RuntimeManager.CreateInstance(land);
        // _landInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        //
        //
        // RuntimeManager.AttachInstanceToGameObject(_landInstance, transform, _rigidbody);
        // RuntimeManager.AttachInstanceToGameObject(_takeOffInstance, transform, _rigidbody);
        // RuntimeManager.AttachInstanceToGameObject(_flyLoopInstance, transform, _rigidbody);


        _currentBirdTrigger = birdTriggers[0];
        _currentBirdTrigger.hasBird = true;
        transform.position = _currentBirdTrigger.transform.position;
        foreach (var birdTrigger in birdTriggers)
        {
            birdTrigger.onTriggerEnter += OnBirdTriggerEnter;
        }
    }

    private void OnBirdTriggerEnter(BirdTrigger trigger, GameObject colliderGameObject)
    {
        if (trigger.hasBird)
            ChangeState(State.Lift);
    }

    void ChangeState(State newState)
    {
        _stateTime = 0;
        _startPosition = transform.position;
        switch (newState)
        {
            case State.Idle:
                _currentBirdTrigger.hasBird = true;
                // _landInstance.start();
                // _flyLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case State.Lift:
                _gotoPosition = _startPosition + Vector3.up * liftHeight;
                _currentBirdTrigger.hasBird = false;
                // _takeOffInstance.start();
                // _flyLoopInstance.start();
                break;
            case State.GotoPoint:
                var newBirdTrigger = birdTriggers[Random.Range(0, birdTriggers.Length)];
                while (newBirdTrigger == _currentBirdTrigger)
                {
                    newBirdTrigger = birdTriggers[Random.Range(0, birdTriggers.Length)];
                }

                _currentBirdTrigger = newBirdTrigger;
                _gotoPosition = _currentBirdTrigger.transform.position;
                _gotoDuration = Vector3.Distance(_startPosition, _gotoPosition) * 0.25f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }


        currentState = newState;
    }

    private void Update()
    {
        _stateTime += Time.deltaTime;
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.GotoPoint:
                Vector3 pos = new Vector3();
                pos.x = Mathf.LerpUnclamped(_startPosition.x, _gotoPosition.x, gotoXCurve.Evaluate(_stateTime / _gotoDuration));
                pos.y = Mathf.LerpUnclamped(_startPosition.y, _gotoPosition.y, gotoYCurve.Evaluate(_stateTime / _gotoDuration));
                pos.z = Mathf.LerpUnclamped(_startPosition.z, _gotoPosition.z, gotoZCurve.Evaluate(_stateTime / _gotoDuration));
                transform.position = pos;
                if (_stateTime / _gotoDuration > 1)
                {
                    ChangeState(State.Idle);
                }

                break;
            case State.Lift:
                transform.position = Vector3.LerpUnclamped(_startPosition, _gotoPosition, liftCurve.Evaluate(_stateTime / liftDuration));
                _rigidbody.position = transform.position;
                // RuntimeManager.AttachInstanceToGameObject(_landInstance, transform, _rigidbody);
                // RuntimeManager.AttachInstanceToGameObject(_takeOffInstance, transform, _rigidbody);
                // RuntimeManager.AttachInstanceToGameObject(_flyLoopInstance, transform, _rigidbody);

                if (_stateTime / liftDuration > 1)
                {
                    ChangeState(State.GotoPoint);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
