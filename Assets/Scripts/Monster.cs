﻿using System.Collections;
using MusicMaster;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterScale _size;
    [SerializeField] private float _fullness;
	[SerializeField] private float _max;

    [SerializeField] private bool loadSceneOnTransform = false;
    [SerializeField] private string sceneName;
    
    [Space(10)]
    [SerializeField] private GameObject _nextStage;
    [SerializeField] private GameObject _transformationParticle;

	private Material _material;

    public MonsterScale Scale => _size;

	public float PercentageFull => _fullness / _max;

    ///bool playedSong = false;

    public float Fullness
	{
		get => _fullness;

		set
		{
			_fullness = value;
			AdjustColor();
			CheckValue();
		}
	}

    private bool transformationBegan = false;

    private MonsterMotor _motor;
    private SkinnedMeshRenderer _renderer;

	private void Start()
	{
        _motor = GetComponent<MonsterMotor>();

        // not fantastic but...
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();

    }

    private void Update()
    {
        if (PercentageFull >= 1 && !transformationBegan)
        {
            if (!loadSceneOnTransform) StartCoroutine(TransformRoutine());
            else SceneManager.LoadScene(sceneName);
        }
    }


    private IEnumerator TransformRoutine()
    {
        transformationBegan = true;

        var go = Instantiate(_transformationParticle, transform.position, Quaternion.identity, null);
        var particle = go.GetComponent<ParticleSystem>();

        // Make first form go away for a bit
        _motor.enabled = false;
        _renderer.enabled = false;

        // Play fx
        particle.Play();

        yield return new WaitUntil(() => !particle.isPlaying);

        // Instantiate next form
        var nextForm = Instantiate(_nextStage, transform.position, Quaternion.identity, null);

        // do something with material emission
        //var mat = nextForm.GetComponentInChildren<SkinnedMeshRenderer>().material;

        // Destroy original
        Destroy(gameObject);
    }

	public void AdjustColor()
	{
		// Change color over time
		//_material.color += new Color(PercentageFull, 0, 0, 0);
	}

	public void CheckValue()
	{
		// Explode after threshold
	}
}
