using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    
    class CharacterAvoidance
    {
        public DynamicCharacter character { get; set; }
        public float  maxAcceleration { get; set; }
        public float collisionRadius { get; set; }
        public DynamicCharacter target { get; set; }
        public float maxTimeLookahead { get; set; }

        public MovementOutput getMovement() {
            MovementOutput output = new MovementOutput();
            KinematicData dataCharacter = character.KinematicData;
            KinematicData dataTarget = target.KinematicData;
            UnityEngine.Vector3 futureDeltaPos;
            float futureDistance;
            float timeToClosest;
            UnityEngine.Vector3 deltaPos = dataTarget.position - dataCharacter.position;
            UnityEngine.Vector3 deltaVel = dataTarget.velocity - dataCharacter.velocity;
            float deltaSpeed = deltaVel.magnitude;

            if (deltaSpeed == 0)
                return output;

            timeToClosest = -(Vector3.Dot(deltaPos, deltaVel) / (deltaSpeed * deltaSpeed));

            if (timeToClosest > maxTimeLookahead)
                return output;

            futureDeltaPos = deltaPos + deltaVel * timeToClosest;
            futureDistance = futureDeltaPos.magnitude;

            if (futureDistance > 2 * collisionRadius)
                return new MovementOutput();

            if (futureDistance <= 0 || deltaPos.magnitude < 2 * collisionRadius)
                output.linear = dataCharacter.position - dataTarget.position;
            else
                output.linear = futureDeltaPos * -1;

            output.linear = output.linear.normalized * maxAcceleration;
            return output;
                }

    }
}
