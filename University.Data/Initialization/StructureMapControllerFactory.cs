using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace University.Data.Initialization
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if ((requestContext == null) || (controllerType == null))
                    return base.GetControllerInstance(requestContext, (controllerType));

            return (Controller) ObjectFactory.GetInstance(controllerType);

        }
       
    }
}
