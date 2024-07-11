using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]

    internal class RedShip1 : BaseShip
    {
        private int _fireTimer = 100;
        public RedShip1(Context context) : base(context)
        {
            Speed = 0.01f;
        }
        public override void Update(float timeStep)
        {
            base.Update(timeStep);
            if (_fireTimer > 0) _fireTimer--;
            else
            {
                Fire();
                _fireTimer = 100;
            }
            Node.Position += Node.Direction * Speed;

            if (Node.Position.Z < -1) Node.Remove();
        }
    }
}
