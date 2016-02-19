﻿using UnityEngine;
using System.Collections;

public class AbilityButtons : MonoBehaviour {

    private Cooldowns _cooldownManager;
    private Player _playerScript;
    private Sounds _sounds;

    [SerializeField]private GameObject shockwaveObject;
    [SerializeField]private ParticleSystem particles;
    [SerializeField]private GameObject parentObject;
    [SerializeField]private GameObject Laserbeam;
    private AudioSource source;

    private float _laserCooldown = 0;
    private float _medusaCooldown = 0;
    private float _swordCooldown = 0;
    private float _minCooldown = 0;

    void Start()
    {
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
            source.PlayOneShot(_sounds.FiringMahLazor);
            StartCoroutine(ActivateTimer(Laserbeam, 2f));
            _laserCooldown = LaserCD;
            _cooldownManager.LaserCooldown = _laserCooldown;
        }
    }

    IEnumerator ActivateTimer(GameObject obj, float activeTime)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        obj.SetActive(false);
    }

    public void UseMedusaHead(float MedusaCD)
    {
        if (_medusaCooldown <= _minCooldown)
        {
            //ParticleSystem instantiatedObject = Instantiate(particles, parentObject.transform.position, Quaternion.Euler(0, 90, -90)) as ParticleSystem;
            //instantiatedObject.transform.parent = parentObject.transform;
            GameObject shockwave = Instantiate(shockwaveObject, parentObject.transform.position, Quaternion.identity) as GameObject;
            shockwave.transform.parent = parentObject.transform;
            source.PlayOneShot(_sounds.MedusaSound);

            _medusaCooldown = MedusaCD;
            _cooldownManager.MedusaCooldown = _medusaCooldown;
            StartCoroutine(ActivateTimer(parentObject, 0.75f));
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