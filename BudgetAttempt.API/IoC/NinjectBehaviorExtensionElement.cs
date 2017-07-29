using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace BudgetAttempt.API.IoC
{
    public class NinjectBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(NinjectBehaviorAttribute); }
        }

        protected override object CreateBehavior()
        {
            return new NinjectBehaviorAttribute();
        }
    }
}