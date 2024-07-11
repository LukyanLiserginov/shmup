using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]

    internal class BlueShip:BaseShip
    {
        private InputMap _inputMap;
        private Vector3 _curVelocity;
        private float _inertia = 0.95f;
        private int _fireTimer = 0;
        public BlueShip(Context context) : base(context) 
        {
            _inputMap = Context.ResourceCache.GetResource<InputMap>("Input/MoveAndOrbit.inputmap");
            Speed = 0.1f;
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);
            if (_fireTimer > 0) _fireTimer--;

            var left = _inputMap.Evaluate("Left");
            var right = _inputMap.Evaluate("Right");
            var forward = _inputMap.Evaluate("Forward");
            var back = _inputMap.Evaluate("Back");

            var velocity = new Vector3(right - left, 0, forward - back);
            velocity.Normalize();
            _curVelocity = _inertia * _curVelocity + (1-_inertia) * velocity * Speed;
            var newPosition = Node.Position + _curVelocity;
            newPosition.X = MyTools.Clamp(newPosition.X, -6.1f, 6.1f);
            newPosition.Z = MyTools.Clamp(newPosition.Z, -1.5f, 6.6f);
            Node.Position = newPosition;


            if (_inputMap.Evaluate("Use") > 0.5f && _fireTimer < 1) Fire();
        }

        public override void Fire() 
        { 
            base.Fire();
            _fireTimer = 10;
        }

        public override void Destroy()
        {
            MyTools.IsRunning = false;
            base.Destroy();
        }
    }
}
