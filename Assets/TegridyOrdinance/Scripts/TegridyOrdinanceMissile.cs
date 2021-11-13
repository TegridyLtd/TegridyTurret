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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tegridy.Ordinance
{
    public class TegridyOrdinanceMissile : MonoBehaviour
    {
        [Header("Missile Config")]
        public GameObject warhead; //game object to be enabled on detonation
        public GameObject exhuast; //game obeject to be enabled while we have fuel

        [HideInInspector] public int id; //used by the turret to find position in array that the misssile belongs to when reloading
        public MissileConfig config;
        private MissileLaunch command;
        private TegridyOrdinanceManager control;

        private bool launched = false;
        private float fuel;

        Rigidbody rb;
        AudioSource audioSource;
        public void StartUp(TegridyOrdinanceManager thisControl)
        {
            control = thisControl;

            rb = gameObject.AddComponent<Rigidbody>();
            audioSource = gameObject.AddComponent<AudioSource>();

            //setup the rigidbody
            rb.mass = config.rbConfig.mass;
            rb.drag = config.rbConfig.drag;
            rb.angularDrag = config.rbConfig.angularDrag;
            rb.useGravity = config.rbConfig.useGravity;
            rb.collisionDetectionMode = config.rbConfig.colMode;
            rb.interpolation = config.rbConfig.interpolMode;

            rb.isKinematic = true;
            launched = false;
        }

        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position, transform.forward * 20, Color.blue);

            if (!launched) return; //Don't do anything till launch
            
            //if we have fuel add some more force a
            if(fuel > 0)
            {
                fuel -= config.fuelBurn * Time.deltaTime;
                //rotate towards our target
                if (command.target != null)
                {
                    exhuast.transform.LookAt(command.target, transform.forward);
                    rb.AddRelativeTorque(exhuast.transform.position, ForceMode.Impulse);
                }

                //add some force to the missile and use up the fuel
                if (config.maxVelocity > rb.velocity.magnitude) rb.AddForce(exhuast.transform.forward * (config.thrustForce * Time.deltaTime), ForceMode.Impulse);
            }
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        public void LaunchMissile(MissileLaunch thisCommand)
        {
            fuel = config.fuel;
            transform.parent = control.transform;
            command = thisCommand;
            rb.isKinematic = false;

            if(config.burnDelay > 0) StartCoroutine(BurnDelay());
            else launched = true;

            rb.AddForce(transform.forward * command.power, ForceMode.Impulse);
        }

        IEnumerator BurnDelay()
        {
            yield return new WaitForSeconds(config.burnDelay);
            launched = true;
        }


        private void Detonate()
        {
            warhead.transform.parent = transform.parent;
            warhead.SetActive(true);
            command.finalPosition = transform.position;
            control.missileImpactLog.Add(command);
            Destroy(this.gameObject);
        }
        IEnumerator DetonateTimer(float delay)
        {
            yield return new WaitForSeconds(delay);
            Detonate();
        }
    }
}