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
using System.Collections.Generic;
using System.Collections;
namespace Tegridy.Ordinance
{
    public class TegridyOrdinanceManager : MonoBehaviour
    {
        [Header("Projectiles")]
        public ProjectilePool[] shells;
        public int addDudProjectile;
        public ProjectilePool[] dudProjectiles;
        public int projectileQTY;

        [Header("Missiles")]
        public MissilePool[] missiles;
        public int addDudMissile;
        public MissilePool[] dudMissiles;

        [Header("Logs")]
        public List<FireCommand> projectileLaunchLog = new List<FireCommand>();
        public List<MissileLaunch> missileLaunchLog = new List<MissileLaunch>();
        public List<MissileLaunch> missileImpactLog = new List<MissileLaunch>();
        
        void Awake()
        {
            for (int i = 0; i < shells.Length; i++)
            {
                shells[i].shellPool = new GameObject[projectileQTY];
                shells[i].casingPool = new GameObject[projectileQTY];
                for (int i2 = 0; i2 < shells[i].shellPool.Length; i2++)
                {
                    shells[i].shellPool[i2] = InstantiateZero(shells[i].shell, transform);
                    shells[i].casingPool[i2] = InstantiateZero(shells[i].casing, transform);
                }
            }

            if (addDudProjectile > 0)
            {



            }
        }
        private GameObject InstantiateZero(GameObject spawn, Transform parent)
        {
            if (spawn != null)
            {
                GameObject newObj = Instantiate(spawn);
                newObj.transform.parent = parent;
                newObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                newObj.SetActive(false);
                return newObj;
            }
            else return null;
        }

        #region Projectiles

        public void LaunchProjectile(FireCommand command)
        {
            projectileLaunchLog.Add(command);
            shells[command.group].curShell++;
            if (shells[command.group].curShell == shells[command.group].shellPool.Length) shells[command.group].curShell = 0;

            SpawnProjectile(shells[command.group].shellPool[shells[command.group].curShell], command.launchPos, command.power);
            if(command.eject) StartCoroutine(EjectCasing(command));
        }
        private void SpawnProjectile(GameObject spawn, Transform pos, float force) 
        {
            if (spawn != null && pos != null)
            {
                spawn.transform.SetPositionAndRotation(pos.position, pos.rotation);
                spawn.SetActive(true);
                Rigidbody _rb = spawn.GetComponent<Rigidbody>();
                if (_rb != null) _rb.AddForce(spawn.transform.forward * force, ForceMode.Impulse);
            }
        }
        IEnumerator EjectCasing(FireCommand command)
        {
            yield return new WaitForSeconds(command.ejectDelay);
            SpawnProjectile(shells[command.group].casingPool[shells[command.group].curShell], command.ejectPos, command.ejectPower);
        }
        #endregion
        #region Missiles
        public TegridyOrdinanceMissile[] LoadMissiles(Transform[] salvos, int[] ids, int ammoType)
        {
            TegridyOrdinanceMissile[] newMissiles = new TegridyOrdinanceMissile[salvos.Length];

            for (int i = 0; i < salvos.Length; i++)
            {
                GameObject _thisMissile = Instantiate(missiles[ammoType].shell);
                newMissiles[i] = _thisMissile.GetComponent<TegridyOrdinanceMissile>();
                newMissiles[i].id = ids[i];
                newMissiles[i].config = missiles[ammoType].config;
                newMissiles[i].transform.SetPositionAndRotation(salvos[i].position, salvos[i].rotation);
                newMissiles[i].transform.parent = salvos[i];
                newMissiles[i].StartUp(this);
            }
            return newMissiles;
        }
        #endregion

        #region Rays

        #endregion

        #region AOE

        #endregion

    }
}