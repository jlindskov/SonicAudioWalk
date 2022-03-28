using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class Helicopter : MonoBehaviour
{
    public enum HelicopterStates
    {
        Idle,
        GotoPatrolPosition,
        GotoHomePosition,
        Patrolling,
        Searching,
        Chasing
    }

    public HelicopterStates currentHelicopterState;
    public LightConeTrigger lightConeTrigger;

    [Header("Idle Settings")] public float idleMovementAmount;
    public float idleMovementSpeed;

    [Header("Goto Patrol Settings")] public AnimationCurve gotoDestinationCurve;
    public Transform patrolStartTarget;
    private float _returnDuration;

    [Header("Go Home Settings")] public Transform homeTarget;
    private float _returnHomeDuration;

    [Header("Search Settings")] public float searchMovementAmount;
    public float searchMovementSpeed;
    private float _searchTime;

    [Header("Chase Settings")] public float chaseSpeed;

    [Header("Patrol Settings")] public Transform[] wayPoints;
    public float patrolSpeed;
    private int _currentWayPoint;
    private int _patrolDir = 1;
    public AnimationCurve patrolMovement;


    private PlayerMovement _playerTarget;
    [FormerlySerializedAs("light")] public Light searchLight;
    public LayerMask lightConeRayLayer;


    private Vector3 _stateStartPosition;
    private float _stateTime;

    private bool _currentActiveState = false;
    private float _playerInSightValue;

    [ParamRef] public string parameter;
    private FMOD.Studio.PARAMETER_DESCRIPTION _parameterDescription;

    private void Awake()
    {
        if (string.IsNullOrEmpty(_parameterDescription.name))
        {
            RuntimeManager.StudioSystem.getParameterDescriptionByName(parameter, out _parameterDescription);
        }

        gameObject.SetActive(_currentActiveState);
    }

    //private void Start()
    //{
    //    lightConeTrigger.onPlayerTriggerEnter += OnPlayerTriggerEnter;
    //    lightConeTrigger.onPlayerTriggerExit += OnPlayerTriggerExit;
    //}

    //private void OnPlayerTriggerEnter(PlayerMovement obj)
    //{
    //    ChangeState(HelicopterStates.Chasing);
    //    _playerTarget = obj;
    //}
//
    //private void OnPlayerTriggerExit(PlayerMovement obj)
    //{
    //    ChangeState(HelicopterStates.Searching);
    //}

    private void Start()
    {
        _playerTarget = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        _stateTime += Time.deltaTime;

        if (searchLight.enabled)
        {
            _playerInSightValue = Mathf.MoveTowards(_playerInSightValue, CanSeePlayer() ? 1 : 0, Time.deltaTime);
        }
        
        switch (currentHelicopterState)
        {
            case HelicopterStates.Idle:
                if(_playerInSightValue > 0.5f) ChangeState(HelicopterStates.Chasing);
                float t = Time.time * idleMovementSpeed;
                transform.position += (new Vector3(Mathf.Sin(t) * idleMovementAmount,
                                          Mathf.Sin(t) * idleMovementAmount, Mathf.Cos(t) * idleMovementAmount)) *
                                      Time.deltaTime;
                break;
            case HelicopterStates.GotoPatrolPosition:
                if(_playerInSightValue > 0.5f) ChangeState(HelicopterStates.Chasing);
                transform.position = Vector3.Lerp(_stateStartPosition, patrolStartTarget.position,
                    gotoDestinationCurve.Evaluate(_stateTime / _returnDuration));
                if (_stateTime / _returnDuration >= 1) ChangeState(HelicopterStates.Patrolling);
                break;
            case HelicopterStates.Searching:
                if(_playerInSightValue > 0.5f) ChangeState(HelicopterStates.Chasing);

                t = Time.time * searchMovementSpeed;
                transform.position +=
                    (new Vector3(Mathf.Sin(t) * searchMovementAmount, 0, Mathf.Cos(t) * searchMovementAmount)) *
                    Time.deltaTime;
                _searchTime += Time.deltaTime;
                if (_searchTime > 4)
                {
                    ChangeState(HelicopterStates.Patrolling);
                }

                break;
            case HelicopterStates.Chasing:
                if(_playerInSightValue < 0.5f) ChangeState(HelicopterStates.Searching);
                
                transform.position = Vector3.MoveTowards(transform.position,
                    _playerTarget.transform.position + Vector3.up * 16, Time.deltaTime * chaseSpeed);
                break;
            case HelicopterStates.Patrolling:
                if(_playerInSightValue > 0.5f) ChangeState(HelicopterStates.Chasing);

                
                transform.position = Vector3.Lerp(_stateStartPosition, wayPoints[_currentWayPoint].position,
                    patrolMovement.Evaluate(_stateTime / patrolSpeed));
                if (_stateTime / patrolSpeed >= 1)
                {
                    _stateStartPosition = transform.position;
                    _stateTime = 0;
                    _currentWayPoint += _patrolDir;
                    if (_currentWayPoint >= wayPoints.Length - 1 || _currentWayPoint <= 0)
                        _patrolDir *= -1;
                }

                break;
            case HelicopterStates.GotoHomePosition:
                transform.position = Vector3.Lerp(_stateStartPosition, homeTarget.position,
                    gotoDestinationCurve.Evaluate(_stateTime / _returnHomeDuration));
                if (_stateTime / _returnHomeDuration >= 1)
                {
                    ChangeState(HelicopterStates.Idle);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    
    bool CanSeePlayer()
    {
        Debug.DrawLine(transform.position, transform.position + searchLight.transform.forward * 50, Color.cyan);
        Physics.SphereCast(transform.position, 5, searchLight.transform.forward,
            out var hit, 50, lightConeRayLayer, QueryTriggerInteraction.Ignore);
        if (hit.transform == null) return false;
        Debug.DrawLine(transform.position, hit.point, Color.yellow);
        var p = hit.transform.GetComponentInParent<PlayerMovement>();
        return p != null;
    }

    void ChangeState(HelicopterStates newState)
    {
        _stateStartPosition = transform.position;
        _stateTime = 0;
        switch (newState)
        {
            case HelicopterStates.Idle:
                TriggerFMODParameter(0);
                gameObject.SetActive(false);
                _currentActiveState = false;
                break;
            case HelicopterStates.GotoPatrolPosition:
                _returnDuration = Vector3.Distance(transform.position, patrolStartTarget.position) / 10;
                TriggerFMODParameter(1);
                break;
            case HelicopterStates.Searching:
                searchLight.color = Color.white;
                _searchTime = 0;
                TriggerFMODParameter(1);
                break;
            case HelicopterStates.Chasing:
                searchLight.color = Color.red;
                TriggerFMODParameter(2);
                break;
            case HelicopterStates.Patrolling:
                _currentWayPoint = GetClosestsWaypointIndex();
                TriggerFMODParameter(1);
                break;
            case HelicopterStates.GotoHomePosition:
                _returnHomeDuration = Vector3.Distance(transform.position, homeTarget.position) / 6;
                TriggerFMODParameter(0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        currentHelicopterState = newState;
    }

    int GetClosestsWaypointIndex()
    {
        float distance = float.MaxValue;
        int returnIndex = 0;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            var point = wayPoints[i];
            float d = Vector3.Distance(transform.position, point.position);
            if (d < distance)
            {
                returnIndex = i;
                distance = d;
            }
        }

        return returnIndex;
    }

    void TriggerFMODParameter(int value)
    {
        print("set "+ _parameterDescription.name +" parameter to " + value);
        FMOD.RESULT result = RuntimeManager.StudioSystem.setParameterByID(_parameterDescription.id, value);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError(string.Format(
                ("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}"),
                parameter, result));
        }
    }

    public void SetHelicopterActive(bool state)
    {
        if (_currentActiveState == state) return;
        _currentActiveState = state;
        
        ChangeState(_currentActiveState ? HelicopterStates.GotoPatrolPosition : HelicopterStates.GotoHomePosition);

        searchLight.gameObject.SetActive(_currentActiveState);
    }
}