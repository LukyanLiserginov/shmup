using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]

    internal class RedShip2 : BaseShip
    {
        private int _fireTimer = 110;
        private Vector3 _b1 = new Vector3(-1,0,-2);
        private Vector3 _b2 = new Vector3(1,0,-2);
        public RedShip2(Context context) : base(context)
        {
            Speed = 0.01f;
            _b1.Normalize();
            _b2.Normalize();
        }
        public override void Update(float timeStep)
        {
            base.Update(timeStep);
            if (_fireTimer > 0)
            {
                _fireTimer--;
            }
            else
            {
                Fire();
                Fire(_b1);
                Fire(_b2);
                _fireTimer = 110;
            }
            Node.Position += Node.Direction * Speed;

            if (Node.Position.Z < -1) Node.Remove();
        }
    }
}
