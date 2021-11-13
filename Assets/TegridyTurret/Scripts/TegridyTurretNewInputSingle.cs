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
    public class TegridyTurretNewInputSingle : MonoBehaviour
    {
        // Start is called before the first frame update
        public TegridyTurret turret;
        public float inputSpeed;

        PlayerInput input;
        void Start()
        {
            //tell the turret what mode to use.
            turret.targetMode = 1;

            //get the player input
            input = new PlayerInput();
            input.Enable();
            turret.StartUp();
            turret.active = true;
        }

        void Update()
        {
            //capture the input for the turret
            Vector2 _thisVector = input.Turret.RotateTurret.ReadValue<Vector2>();
            turret.bearing += _thisVector.x * (Time.deltaTime * inputSpeed);
            turret.inclination += _thisVector.y * (Time.deltaTime * inputSpeed);
            //do our fire control
            if (input.Turret.ChangeAmmo.triggered) turret.ChangeProjectileAmmo();
            if (input.Turret.Fire.triggered) turret.Fire();
        }
    }
}