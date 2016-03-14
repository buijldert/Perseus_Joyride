﻿using UnityEngine;
using System.Collections;

public class AbilityButtons : MonoBehaviour {

    private Cooldowns _cooldownManager;
    private Player _playerScript;
    private Sounds _sounds;
    private CameraShake _cameraShakeScript;

    [SerializeField]private GameObject parentObject;
    [SerializeField]private GameObject LaserbeamParent;
    private AudioSource source;

    private float _laserCooldown = 0;
    private float _medusaCooldown = 0;
    private float _swordCooldown = 0;
    private float _minCooldown = 0;

    delegate void SoundDelegate(AudioClip clip);
    SoundDelegate soundDelegate;

    void Start()
    {
        _cameraShakeScript = GameObject.FindWithTag(Tags.mainCameraTag).GetComponent<CameraShake>();
        soundDelegate = playSound;
        source = GetComponent<AudioSource>();
        _sounds = GameObject.FindWithTag("SoundsObject").GetComponent<Sounds>();
        _playerScript = GetComponentInParent<Player>();
        _cooldownManager = GetComponentInParent<Cooldowns>();
        this.gameObject.tag = Tags.abilityButtonsTag;
    }

    void Update()
    {
        ResetCooldowns();
    }

    public void UseSword(float SwordCD)
    {
        if (_playerScript.UsingSword == false && _swordCooldown <= _minCooldown) 
        {
            _playerScript.UsingSword = true;
            _swordCooldown = SwordCD;
            _cooldownManager.SwordCooldown = _swordCooldown;
        }
    }

    public void UseLasor(float LaserCD)
    {
        if (_laserCooldown <= _minCooldown)
        {
            GameObject laserbeam = ObjectPool.instance.GetObjectForType(ObjectNames.laserBeam, true);
            laserbeam.transform.parent = LaserbeamParent.transform;
            laserbeam.transform.position = LaserbeamParent.transform.position;
            _cameraShakeScript.Shake();
            //source.PlayOneShot(_sounds.FiringMahLazor);
            //soundDelegate = playSound;
            soundDelegate(_sounds.FiringMahLazor);
            StartCoroutine(ActivateTimer(LaserbeamParent, 1.5f));
            _laserCooldown = LaserCD;
            _cooldownManager.LaserCooldown = _laserCooldown;
        }
    }

    void playSound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    IEnumerator ActivateTimer(GameObject obj, float activeTime)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        _playerScript.MoveSpeed = 5f;
        obj.SetActive(false);
    }

    public void UseMedusaHead(float MedusaCD)
    {
        if (_medusaCooldown <= _minCooldown)
        {
            GameObject shockwave = ObjectPool.instance.GetObjectForType(ObjectNames.medusaEffectGameObject, true);
            shockwave.transform.parent = parentObject.transform;
            shockwave.transform.position = parentObject.transform.position;
            //source.PlayOneShot(_sounds.MedusaSound);
            //soundDelegate = playSound;
            soundDelegate(_sounds.MedusaSound);

            _medusaCooldown = MedusaCD;
            _cooldownManager.MedusaCooldown = _medusaCooldown;
            StartCoroutine(ActivateTimer(parentObject, 1f));
        }
    }

    public void ResetCooldowns()
    {
        if(_cooldownManager.MedusaCooldown <= _minCooldown)
        {
            _medusaCooldown = _cooldownManager.MedusaCooldown;
        }
        if (_cooldownManager.LaserCooldown <= _minCooldown) 
        {
            _laserCooldown = _cooldownManager.LaserCooldown;
        }
        if (_cooldownManager.SwordCooldown <= _minCooldown)
        {
            _swordCooldown = _cooldownManager.SwordCooldown;
        }
    }
}
