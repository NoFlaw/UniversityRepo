// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using University.Data.Entities.Models;
using University.Data.Repository;
using University.Data.Repository.Base;
using University.Data.UnitOfWork;
using University.Data.UnitOfWork.Base;

namespace University.Web.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        //scan.Assembly("University.IoC");
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                        scan.LookForRegistries();
                                        scan.WithDefaultConventions();
                                    });
                       
                            x.For<IUnitOfWorkFactory>().Use<EFUnitOfWorkFactory>();
                            x.For(typeof(IRepository<>)).Use(typeof(EFRepository<>));
                            x.For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
                            x.For<System.Data.Entity.DbContext>().Use(() => new ApplicationDbContext());
                            
                            //x.For<ApplicationUser>().HttpContextScoped();
                            //x.For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
                            ////x.For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
                            //x.For<System.Data.Entity.DbContext>().Use(() => new UniversityContext());
                            //x.For<System.Data.Entity.DbContext>().Use(() => new ApplicationDbContext());
                            //x.For(typeof (IUserStore<ApplicationUser>)).Use(typeof(UserStore<IdentityUser>));
                            //x.For<UniversityContext>().HybridHttpOrThreadLocalScoped().Use<UniversityContext>();

                        });
            return ObjectFactory.Container;
        }
    }
}