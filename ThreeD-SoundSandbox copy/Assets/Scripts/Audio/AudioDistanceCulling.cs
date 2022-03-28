using System;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioDistanceCulling : MonoBehaviour
{
    private StudioEventEmitter _studioEventEmitter;
    private PlayerMovement _playerMovement;

    private int _frameCounter;
    public int audioDistance;
    
    private void Start()
    {
        TryGetComponent(out _studioEventEmitter);
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _frameCounter = Random.Range(0, 100);
    }

    void Update()
    {
        if (_frameCounter == 0)
        {
            if (Vector3.Distance(transform.position, _playerMovement.transform.position) > audioDistance &&
                _studioEventEmitter.IsPlaying())
            {
                _studioEventEmitter.Stop();
            }

            if (Vector3.Distance(transform.position, _playerMovement.transform.position) < audioDistance &&
                !_studioEventEmitter.IsPlaying())
            {
                _studioEventEmitter.Play();
            }
        }
        _frameCounter++;
        if (_frameCounter > 100) _frameCounter = 0;
    }
}
