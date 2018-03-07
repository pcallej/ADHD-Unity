using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    //Experimental Methods
    public partial class Animal 
    {
        //float acceleration;
        //float DownAcceleration = 1.5f;

        //public virtual void FlyLogic(float time)
        //{
        //    if (!canFly) return;

        //    if (fly)
        //    {


        //        if (MovementAxis.y < -0.1)
        //        {
        //            acceleration = Mathf.Lerp(acceleration, acceleration + DownAcceleration, time);
        //        }
        //        else
        //        {
        //            float a = acceleration - DownAcceleration;
        //            if (a < 0) a = 0;

        //            acceleration = Mathf.Lerp(acceleration, a, time);  //Deacelerate slowly all the acceleration you earned..
        //        }

        //        transform.position = Vector3.Lerp(transform.position, transform.position + Anim.velocity * (acceleration / 2), time);
        //    }
        //    else
        //    {
        //        acceleration = 0;
        //    }

        //    ////This is for changing from fall to Fly
        //    //Anim.applyRootMotion = true;
        //    //transform.position = Vector3.Lerp(animal.transform.position, animal.transform.position + v, time);
        //    //v = Vector3.Lerp(v, Vector3.zero, time * FallRecovery);
        //}

        ///// <summary>
        ///// Underwater Logic
        ///// </summary>
        //public virtual void UnderWaterMovement()
        //{
        //    if (jump) down = false;             //Don't press the two buttons at the same time  UP and Down

        //    _RigidBody.useGravity = false;

        //    transform.rotation = DeltaRotation;     //Store the Rotation before rotating

        //    if (shift)
        //    {
        //        UnderWaterShift = Mathf.Lerp(UnderWaterShift, 2f, underWaterSpeed.lerpPosition * Time.fixedDeltaTime);
        //    }
        //    else
        //    {
        //        UnderWaterShift = Mathf.Lerp(UnderWaterShift, 1f, underWaterSpeed.lerpPosition * Time.fixedDeltaTime);
        //    }

        //    YAxisMovement(upDownSmoothness);


        //    _transform.Rotate(_transform.up, direction * underWaterSpeed.rotation);        //Rotate while going forward

        //    float Up = movementAxis.y;

        //    if (MovementAxis.z < 0) Up = 0;      //Remove Rotations When going backwards

        //    Vector3 forward = (transform.forward * speed) + (transform.up * Up);                     //Calculate the Direction to Move

        //    float limit = 82;

        //    if (forward.magnitude > 1) forward.Normalize();                                         //Remove extra Speed

        //    _transform.position = Vector3.Lerp(transform.position, transform.position + (forward * underWaterSpeed.position * UnderWaterShift), Time.fixedDeltaTime * underWaterSpeed.lerpPosition); //Move it

        //    DeltaRotation = transform.rotation;

        //    if (forward.magnitude > 0.001)
        //    {
        //        float angle = 90 - Vector3.Angle(Vector3.up, forward);

        //        float smooth = Mathf.Max(Mathf.Abs(MovementAxis.y), Mathf.Abs(speed));

        //        transform.Rotate(Mathf.Clamp(-angle, -limit, limit) * smooth, 0, 0, Space.Self);
        //    }


        //    transform.Rotate(0, 0, -30 * Direction, Space.Self);        //Rotation Bank



        //    //To Get Out of the Water---------------------------------
        //    RaycastHit UnderWaterHit;

        //    if (Physics.Raycast(pivots[2].transform.position, -Vector3.up, out UnderWaterHit, scaleFactor * 1, WaterLayer))
        //    {
        //        Debug.DrawRay(pivots[2].transform.position, -Vector3.up * scaleFactor * 1, Color.blue);
        //        if (!down)
        //        {
        //            underwater = false;
        //            anim.applyRootMotion = true;
        //            _RigidBody.useGravity = true;
        //            _RigidBody.drag = 0;
        //            _RigidBody.constraints = Still_Constraints;
        //            movementAxis.y = 0;
        //        }
        //    }
        //}

        //public virtual void UnderWaterMovement()
        //{
        //    if (jump) down = false;  //Don't press the two buttons at the same time  UP and Down

        //    YAxisMovement(upDownSmoothness);

        //    _rigidbody.drag = 100;

        //    int shiftpeed = 1;
        //    if (shift) shiftpeed = 3;

        //    //Forwards Movement
        //    if (movementAxis.z > 0 || movementAxis.y != 0)
        //    {
        //        _transform.position = Vector3.Lerp(_transform.position, _transform.position + _transform.forward * underWaterSpeed.position * shiftpeed * Mathf.Max(movementAxis.z, Mathf.Abs(movementAxis.y)) / 2, Time.fixedDeltaTime);
        //    }
        //    //Rotation left/right

        //    transform.RotateAround(_transform.position, Vector3.up, underWaterSpeed.rotation * movementAxis.x * Time.fixedDeltaTime * 50f);

        //    if ((Vector3.Angle(transform.forward, Vector3.up) > 30 && jump) || (Vector3.Angle(transform.forward, Vector3.up) < 170 && down)) //Limit Up Down Axis
        //    {
        //    transform.RotateAround(_transform.position, transform.right, 2 * -movementAxis.y * Time.fixedDeltaTime * 50);
        //    }



        //    if (!jump && !down)
        //    {
        //        movementAxis.y = Mathf.Lerp(movementAxis.y, 0, Time.fixedDeltaTime * 2);
        //    }

        //    //To Get Out of the Water---------------------------------
        //    RaycastHit UnderWaterHit;

        //    if (Physics.Raycast(pivots[2].transform.position, -Vector3.up, out UnderWaterHit, scaleFactor * 1, WaterLayer))
        //    {
        //        Debug.DrawRay(pivots[2].transform.position, -Vector3.up * scaleFactor * 1, Color.blue);
        //        if (!down)
        //        {
        //            underwater = false;
        //            anim.applyRootMotion = true;

        //            _rigidbody.drag = 0;
        //            _rigidbody.constraints = Still_Constraints;
        //            movementAxis.y = 0;
        //        }
        //    }
        //}
    }
}
