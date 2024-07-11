using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]
    internal class BaseTrigger : TriggerAnimator
    {
        public BaseShip Ship;
        public BaseTrigger(Context context) : base(context)
        {
        }

        public override void OnEnter()
        {
            if (Ship != null)
            {
                Ship.Destroy();
            }
        }
    }
}
