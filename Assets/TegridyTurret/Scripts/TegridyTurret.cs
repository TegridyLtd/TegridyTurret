/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using Tegridy.Tools;
using System.Collections;
using System.Collections.Generic;
using Tegridy.Ordinance;

namespace Tegridy.Turret
{
    public class TegridyTurret : MonoBehaviour
    {
        public TegridyOrdinanceManager fireControl;
        [Header("Firing Config")]
        public int targetMode = 0; // 0 = No input || 1 = Rotate to heading || 2 = Rotate to target
        public int fireMode = 0; // 0 = No Fire || 1 = Fire projectile || 2 = fire missile || 3 = Fire Ray || 4 = Fire All

        [Header("Movement")]
        public TurretConfig config;
        public Transform turretBase; //base object that does not move
        public Transform rotate; //rotation axis
        public Transform pivot; //the base of the barrel 
        public Transform targetTracker; //use to lerp towards target in auto mode


        [Header("Projectile Turret")]
        public projectilePod[] projectilePod;
        public Ammo[] projectileAmmo;
        public bool projectileSingleFire; //fire one barrel/pod?
        public int projectileType = 0; //current ammo

        [Header("Missile Turret")]
        public MissilePod[] missilePod;
        public Ammo[] missileAmmo;

        [Header("Audio")]
        public TurretAudio audioClips;
        public float audioTolerance;
        public float fadeSpeed;

        [Header("Info")]
        public float bearing;
        public float inclination;


        [Header("Info")]
        public Transform target;
        public List<int> pods = new List<int>();

        AudioSource audioSFX;
        AudioSource audioBase;
        AudioSource audioPivot;

        private bool changingMissiles;

        public Vector3 targetHeading;
        public bool active;

        public void StartUp()
        {
            //setup the audio
            audioBase = rotate.gameObject.AddComponent<AudioSource>();
            audioPivot = pivot.gameObject.AddComponent<AudioSource>();
            audioSFX = rotate.gameObject.AddComponent<AudioSource>();

            audioBase.loop = true;
            audioPivot.loop = true;
            audioSFX.loop = false;

            audioPivot.clip = audioClips.pivotRotate;
            audioBase.clip = audioClips.baseRotate;

            audioPivot.volume = 0;
            audioBase.volume = 0;

            audioBase.Play();
            audioPivot.Play();

            if (projectilePod.Length > 0)
            {
                pods.Add(1);
                for (int i = 0; i < projectilePod.Length; i++)
                {
                    projectilePod[i].animator = projectilePod[i].barrel.GetComponent<Animator>();
                }
            }

            if (missilePod.Length > 0)
            {
                pods.Add(2);
                for (int pod = 0; pod < missilePod.Length; pod++)
                {
                    missilePod[pod].missiles = new TegridyOrdinanceMissile[missilePod[pod].missileSalvo.Length];

                    List<Transform> reloadSalvos = new List<Transform>();
                    List<int> salvoIds = new List<int>();
                    for (int i = 0; i < missilePod[pod].missileSalvo.Length; i++)
                    {
                        if (missileAmmo[missilePod[pod].ammoType].ammoQuty > 0)
                        {
                            missileAmmo[missilePod[pod].ammoType].ammoQuty--;
                            reloadSalvos.Add(missilePod[pod].missileSalvo[i]);
                            salvoIds.Add(i);
                        }
                        else break;
                    }
                    if (reloadSalvos.Count > 0) StartCoroutine(ReloadMissiles(reloadSalvos, salvoIds, pod, missilePod[pod].ammoType, 0f));
                }
            }

            //set center position
            bearing = (config.minBearing * 0.5f) + (config.maxBearing * 0.5f);
            inclination = (config.minInclination * 0.5f) + (config.maxInclination * 0.5f);


            fireMode = pods[0];
        }
        private void FixedUpdate()
        {
            if(active)
            switch (targetMode)
            {
                case 0:
                    return;
                case 1:
                    RotateToHeading();
                    break;
                case 2:
                    RotateToTarget();
                    break;
            }
            Debug.DrawRay(rotate.position, rotate.TransformDirection(Vector3.forward) * 10, Color.green);
            Debug.DrawRay(pivot.position, pivot.TransformDirection(Vector3.forward) * 10, Color.red);
        }

        #region Control
        private void RotateToHeading()
        {
            bearing = Mathf.Clamp(bearing, config.minBearing, config.maxBearing);

            Vector3 _tempVector = rotate.localEulerAngles;

            float newRot = Mathf.Lerp(_tempVector.y, bearing, Time.deltaTime * config.baseSpeed);

            AudioTools.FadeOnInput(audioBase, newRot, _tempVector.y, audioTolerance, fadeSpeed);

            _tempVector.y = newRot;

            rotate.localEulerAngles = _tempVector;

            inclination = Mathf.Clamp(inclination, config.minInclination, config.maxInclination);
            _tempVector = pivot.localEulerAngles;
            newRot = Mathf.Lerp(_tempVector.x, inclination, Time.deltaTime * config.pivotSpeed);
            AudioTools.FadeOnInput(audioPivot, newRot, _tempVector.x, audioTolerance, fadeSpeed);
            _tempVector.x = newRot;
            pivot.localEulerAngles = _tempVector;
        }
        private void RotateToTarget()
        {
            targetTracker.LookAt(target);
            Vector3 _tempVector = rotate.localEulerAngles;
            _tempVector.y = Mathf.Lerp(_tempVector.y, targetTracker.localEulerAngles.y, Time.deltaTime * config.baseSpeed);
            rotate.localEulerAngles = _tempVector;






            Quaternion newRot = Quaternion.FromToRotation(turretBase.position, target.position);

            //rotate to face target
            bearing = newRot.eulerAngles.y - turretBase.eulerAngles.y;



            RotateToHeading();
        }
        public void Fire()
        {
            Debug.Log("Firing");
            switch (fireMode)
            {
                case 0:
                    //not accepting commands
                    return;
                case 1:
                    FireProjectile();
                    break;
                case 2:
                    FireMissile();
                    break;
                case 3:
                    FireRay();
                    break;
                case 4:
                    FireProjectile();
                    FireMissile();
                    FireRay();
                    break;
            }
        }
        public void ChangeAmmo()
        {
            switch (fireMode)
            {
                case 0:
                    //not accepting commands
                    return;
                case 1:
                    ChangeProjectileAmmo();
                    break;
                case 2:
                    ChangeAllMissiles();
                    break;
            }
        }
        #endregion
        #region Projectiles
        private void FireProjectile()
        {
            if (fireControl != null)
                //Fire the cannons if they are loaded
                for (int i = 0; i < projectilePod.Length; i++)
                {
                    if (!projectilePod[i].reloading && projectileAmmo[projectilePod[i].ammoType].ammoQuty >= 1)
                    {
                        //we have ammunition do some animations if we have them and send off a fire command
                        int thisI = i;
                        if (projectilePod[i].animator != null)
                        {
                            projectilePod[i].animator.Play("Recoil");
                        }
                        StartCoroutine(ReloadProjectile(thisI));
                        projectileAmmo[projectilePod[i].ammoType].ammoQuty--;

                        fireControl.LaunchProjectile(BuildFireCommand(thisI));
                        AudioTools.PlayOneShot(projectilePod[thisI].audioClip.fire, audioSFX);
                        if (projectileSingleFire) break;
                    }
                    else AudioTools.PlayOneShot(projectilePod[i].audioClip.noAmmo, audioSFX);
                }
        }
        public void ChangeProjectileAmmo()
        {
            projectileType++;
            if (projectileType >= projectileAmmo.Length) projectileType = 0;

            for (int i = 0; i < projectilePod.Length; i++)
            {
                int thisI = i;
                SetProjectileAmmo(thisI, projectileType);
                StartCoroutine(ReloadProjectile(thisI));
            }
        }

        public void SetProjectileAmmo(int ammo)
        {
            for (int i = 0; i < projectilePod.Length; i++)
            {
                int thisI = i;
                SetProjectileAmmo(thisI, ammo);
                StartCoroutine(ReloadProjectile(thisI));
            }
        }
        public void SetProjectileAmmo(int thisBarrel, int ammo)
        {
            projectilePod[thisBarrel].ammoType = ammo;
            AudioTools.PlayOneShot(projectilePod[thisBarrel].audioClip.changeAmmo, audioSFX);
        }
        private IEnumerator ReloadProjectile(int barrel)
        {

            projectilePod[barrel].reloading = true;
            if (projectilePod[barrel].eject)
            {
                AudioTools.PlayOneShot(projectilePod[barrel].audioClip.eject, audioSFX);
                yield return new WaitForSeconds(projectilePod[barrel].ejectDelay);
            }
            AudioTools.PlayOneShot(projectilePod[barrel].audioClip.reloading, audioSFX);
            yield return new WaitForSeconds(projectilePod[barrel].reloadTime);
            projectilePod[barrel].reloading = false;
            AudioTools.PlayOneShot(projectilePod[barrel].audioClip.reloaded, audioSFX);

            if (projectilePod[barrel].animator != null) projectilePod[barrel].animator.Play(0);
        }
        private FireCommand BuildFireCommand(int chamberID)
        {
            FireCommand command = new FireCommand();
            command.playerName = "Test";
            command.group = projectileAmmo[projectilePod[chamberID].ammoType].ammoType;
            command.power = projectilePod[chamberID].power;
            command.launchPos = projectilePod[chamberID].launchPoint;
            command.eject = projectilePod[chamberID].eject;
            command.ejectDelay = projectilePod[chamberID].ejectDelay;
            command.ejectPos = projectilePod[chamberID].ejectPos;
            return command;
        }
        #endregion
        #region Missiles
        private void ChangeAllMissiles()
        {
            for (int i = 0; i < missilePod.Length; i++)
            {
                ChangeMissileAmmo(i);
            }
        } 
        public void ChangeMissileAmmo(int pod)
        {
            if (!changingMissiles)
            {
                int oldAmmo = missilePod[pod].ammoType;
                missilePod[pod].ammoType++;
                if (missilePod[pod].ammoType >= missileAmmo.Length) missilePod[pod].ammoType = 0;

                List<Transform> salvos = new List<Transform>();
                List<int> salvosIDs = new List<int>();
                for (int i = 0; i < missilePod[pod].missileSalvo.Length; i++)
                {
                    if (missileAmmo[missilePod[pod].ammoType].ammoQuty > 0)
                    {
                        if (missilePod[pod].missiles[i] != null)
                        {
                            //remove any unspent rounds and add it back to the players ammo
                            missileAmmo[oldAmmo].ammoQuty++;
                            Destroy(missilePod[pod].missiles[i].gameObject);
                        }
                        missileAmmo[missilePod[pod].ammoType].ammoQuty--;
                        salvos.Add(missilePod[pod].missileSalvo[i]);
                        salvosIDs.Add(i);
                    }
                    else break;
                }
                changingMissiles = true;
                ReloadMissiles(salvos, salvosIDs, pod, missilePod[pod].ammoType, missilePod[pod].reloadTime);
            }
        }
        private void FireMissile()
        {
            if (fireControl != null)
            {
                //For each missile pod on the turret
                for (int pod = 0; pod < missilePod.Length; pod++)
                {
                    if (missilePod[pod].singleShot)
                    {
                        FireSingleMissile(pod);
                    }
                    else
                    {
                        FireAllMissiles(pod);
                    }
                }
            }
        }
        private void FireSingleMissile(int pod)
        {
            Debug.Log("Firing Single");
            //pick a salve to fire
            missilePod[pod].curSalvo++;
            if (missilePod[pod].curSalvo >= missilePod[pod].missileSalvo.Length) missilePod[pod].curSalvo = 0;
            List<Transform> reloadSalvos = new List<Transform>();
            List<int> salvoIds = new List<int>();

            //make sure we fire something
            int count = 0;
            for (int i = missilePod[pod].curSalvo; i < missilePod[pod].missileSalvo.Length; i++)
            {

                if (missilePod[pod].missiles[i] != null)
                {
                    missilePod[pod].missiles[i].LaunchMissile(BuildLaunchCommand(pod, i));
                    missilePod[pod].missiles[i] = null;

                    if (missileAmmo[missilePod[pod].ammoType].ammoQuty > 0)
                    {
                        missileAmmo[missilePod[pod].ammoType].ammoQuty--;
                        reloadSalvos.Add(missilePod[pod].missileSalvo[i]);
                        salvoIds.Add(i);
                        count++;
                    }
                    missilePod[pod].curSalvo = i + 1;

                    if (count == missilePod[pod].singleCount) break;
                }
            }
            if (reloadSalvos.Count > 0) StartCoroutine(ReloadMissiles(reloadSalvos, salvoIds, pod, missilePod[pod].ammoType, missilePod[pod].reloadTime));
        }
        private void FireAllMissiles(int pod)
        {
            Debug.Log("Firing All");
            //fire any load missiles and reload them if we have ammo
            List<Transform> reloadSalvos = new List<Transform>();
            List<int> salvoIds = new List<int>();
            for (int i = 0; i < missilePod[pod].missileSalvo.Length; i++)
            {
                //Make sure the salvo is loaded
                if (missilePod[pod].missiles[i] != null)
                {
                    missilePod[pod].missiles[i].LaunchMissile(BuildLaunchCommand(pod, i));
                    missilePod[pod].missiles[i] = null;

                    if (missileAmmo[missilePod[pod].ammoType].ammoQuty > 0)
                    {
                        missileAmmo[missilePod[pod].ammoType].ammoQuty--;
                        reloadSalvos.Add(missilePod[pod].missileSalvo[i]);
                        salvoIds.Add(i);
                    }
                }
            }
            if (reloadSalvos.Count > 0) StartCoroutine(ReloadMissiles(reloadSalvos, salvoIds, pod, missilePod[pod].ammoType, missilePod[pod].reloadTime));
        }
        IEnumerator ReloadMissiles(List<Transform> salvos, List<int> ids, int podID, int ammoType, float delay)
        {
            yield return new WaitForSeconds(delay);
            TegridyOrdinanceMissile[] theseMissiles = fireControl.LoadMissiles(salvos.ToArray(), ids.ToArray(), ammoType);
            for (int i = 0; i < theseMissiles.Length; i++)
            {
                missilePod[podID].missiles[theseMissiles[i].id] = theseMissiles[i];
            }
            changingMissiles = false; //stops the player changing again and again while the launchers is reloading
        }
        private MissileLaunch BuildLaunchCommand(int podID, int salvoID)
        {
            MissileLaunch command = new MissileLaunch();
            command.playerName = "Test";
            command.target = target;
            command.power = missilePod[podID].power;
            command.launchPos = missilePod[podID].missileSalvo[salvoID];
            command.ejectPos = missilePod[podID].casingEject[salvoID];
            command.eject = missilePod[podID].eject;
            command.ejectDelay = missilePod[podID].ejectDelay;
            fireControl.missileLaunchLog.Add(command);
            return command;
        }
        #endregion
        #region Incomplete
        public void FireRay()
        {

        }
        #endregion
    }
}