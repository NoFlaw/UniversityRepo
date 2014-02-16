using System.Collections.Generic;
using System.Linq;
using StructureMap;
using University.Data.Repository.Base;

using Models = University.Data.Entities.Models;

namespace University.Business.Services.Student
{    
    public static class StudentService
    {
        //Get an instance of Students
        private static IRepository<Models.Student> StudentRepository
        {
            get { return ObjectFactory.GetInstance<IRepository<Models.Student>>(); }
        }

        /// <summary>
        /// Returns all students within given Student Table
        /// </summary>
        /// <returns></returns>
        public static List<Models.Student> GetAll()
        {
            return StudentRepository.GetAll().ToList();
        }

        public static Models.Student FindById(int id)
        {
            return StudentRepository.GetById(id);
        }
    }
}
