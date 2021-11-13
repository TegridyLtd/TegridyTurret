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
namespace Tegridy.Turret
{
    public class TegridyTurretNewInputMulti : MonoBehaviour
    {
        // Start is called before the first frame update
        public TegridyTurret[] turret;
        public float inputSpeed;
        public int targetMode = 1;
        public int fireMode;

        public bool updateAll;
        public int updateTurret;

        PlayerInput input;
        void Start()
        {
            //tell the turret what mode to use.
            for (int i = 0; i < turret.Length; i++)
            {
                turret[i].targetMode = targetMode;
            }

            //get the player input
            input = new PlayerInput();
            input.Enable();
        }

        void Update()
        {
            //capture the input for the turret
            Vector2 _thisVector = input.Turret.RotateTurret.ReadValue<Vector2>();
            _thisVector *= (Time.deltaTime * inputSpeed);

            if (updateAll)
            {
                UpdateRotationAll(_thisVector);
                ChangeProjectileAll();
                FireAll();
            }
            else
            {
                UpdateRotation(updateTurret, _thisVector);
                Fire(updateTurret);
            }



            //do our fire control
            //if (input.Turret.ChangeAmmo.triggered) turret.ChangeProjectileAmmo();
            //if (input.Turret.Fire.triggered) turret.Fire();
        }
        #region Targeting & Movement
        public void UpdateRotationAll(Vector2 ammount)
        {
            for (int i = 0; i < turret.Length; i++)
            {
                UpdateRotation(i, ammount);
            }
        }
        public void UpdateRotation(int thisTurret, Vector2 ammount)
        {
                turret[thisTurret].bearing += ammount.x;
                turret[thisTurret].inclination += ammount.y;
        }
        public void SetRotationAll(float bearing, float inclination)
        {
            for (int i = 0; i < turret.Length; i++)
            {
                SetRotation(i, bearing, inclination);
            }
        }
        public void SetRotation(int thisTurret, float bearing, float inclination)
        {
            turret[thisTurret].bearing = bearing;
            turret[thisTurret].inclination = inclination;
        }
        public void SetTargetModeAll(int mode)
        {
            for (int i = 0; i < turret.Length; i++)
            {
                turret[i].targetMode = mode;
            }
        }
        public void SetTargetMode(int thisTurret, int mode)
        {
            turret[thisTurret].targetMode = mode;
        }
        public void SetFireModeAll(int mode)
        {
            for (int i = 0; i < turret.Length; i++)
            {
                turret[i].fireMode = mode;
            }
        }
        public void SetFireMode(int thisTurret, int mode)
        {
            turret[thisTurret].fireMode = mode;
        }
        public void SetTargetAll(Transform target)
        {
            for (int i = 0; i < turret.Length; i++)
            {
                SetTarget(i, target);
            }
        }
        public void SetTarget(int thisTurret, Transform target)
        {
            turret[thisTurret].target = target;
        }
        public void FireAll()
        {
            for (int i = 0; i < turret.Length; i++)
            {
                Fire(i);
            }
        }
        public void Fire(int thisTurret)
        {
            turret[thisTurret].Fire();
        }
        #endregion


        #region Ammo Changes
        public void ChangeAmmoAll()
        {
            for (int i = 0; i < turret.Length; i++)
            {
                ChangeAmmo(i);
            }
        }
        public void ChangeAmmo(int thisTurret)
        {
            turret[thisTurret].ChangeAmmo();
        }
        public void SetProjectileAll(int ammo) 
        {
            for (int i = 0; i < turret.Length; i++)
            {
                SetProjectile(i, ammo);
            }

        }
        public void SetProjectile(int pod, int ammo)
        {
            turret[pod].SetProjectileAmmo(ammo);
        }
        public void ChangeProjectileAll()
        {
            for (int i = 0; i < turret.Length; i++)
            {
                ChangeProjectile(i);
            }

        }
        public void ChangeProjectile(int pod)
        {
            turret[pod].ChangeProjectileAmmo();
        }
        public void ChangeMissileAll() 
        {
            for (int i = 0; i < turret.Length; i++)
            {
                ChangeMissile(i);
            }

        }
        public void ChangeMissile(int thisTurret) 
        {
            for (int i =0; i< turret[thisTurret].missilePod.Length; i++)
            {
                turret[thisTurret].ChangeMissileAmmo(i);
            }
            
        }
        #endregion
    }
}