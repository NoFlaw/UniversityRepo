using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace University.Data.Initialization
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public IContainer Container;

        public StructureMapControllerFactory(IContainer container)
        {
            Container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                //if ((requestContext == null) || (controllerType == null))
                    return Container.GetInstance(controllerType) as Controller;
            }
            catch (StructureMapException)
            {
                System.Diagnostics.Debug.WriteLine(Container.WhatDoIHave());
                throw;
            }
        }

        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
        }
        
    }
}
