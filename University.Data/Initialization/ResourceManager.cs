using System;
using System.Data.Entity;
using StructureMap;
using University.Data.Entities;
using University.Data.UnitOfWork;

namespace University.Data.Initialization
{
    public static class ResourceManager
    {
        public static void Initialize()
        {
            //Tell the concrete factory what EF model to use
            EFUnitOfWorkFactory.SetObjectContext(() => new UniversityContext());

            //Initialization of Database creation
            var context = new UniversityContext();

            //Creates Database & Seeds data
            Database.SetInitializer<UniversityContext>(new UniversityDBInitializer());

            //Checks current context, model, & connection for changes
            //context.Database.Initialize(false);

            context.Dispose();
        }

        public static void Save()
        {
            UnitOfWork.UnitOfWork.Save();
        }

        public static void Dispose()
        {
            UnitOfWork.UnitOfWork.Current.Dispose();
        }

        public static void EndRequest(object sender, EventArgs e)
        {
            //Request cleanup
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}
