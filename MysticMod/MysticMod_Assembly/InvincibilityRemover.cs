using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{

    public class InvincibilityRemover : MonoBehaviour
    {
        private BlockBehaviour BB;
        private bool MakeInvinc = true;
        private MToggle MI;
        private HingeJoint HJ;
        private bool firstframe = false;
        private int fcounter;

        private void Awake()
        {
            Console.Log("Invincibility Remover Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            if (this.BB == null)
			{
                Console.Log("No block behavior, destroying the object");
				UnityEngine.Object.Destroy(this);
			}
            if(gameObject.name=="CircularSaw" || gameObject.name=="CogMediumPowered")
            {
                MakeInvinc = false;
            }
            else
            { 
                MI = BB.AddToggle("Make Invincible", "MVI", MakeInvinc);
                MI.Toggled += (bool value) => { MakeInvinc = value; };

                MI.DisplayInMapper = true;
            }
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (!GetComponent<HingeJoint>())
                    return;
                HJ = GetComponent<HingeJoint>();
            }
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.SimPhysics)
                {
                    if (!firstframe)
                    {
                        if (!HJ)
                        {
                            firstframe = true;
                            return;
                        }
                        fcounter++;

                        if (MakeInvinc)
                            return;
                        else
                        {
                            HJ.breakForce = 90000;
                            HJ.breakTorque = 90000;
                        }
                       
                        if (fcounter == 4)
                            firstframe = true;
                    }
                }
            }
        }
    }

}