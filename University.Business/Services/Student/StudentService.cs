using System.Collections.Generic;
using System.Linq;
using University.DAL.UnitOfWork;
using Model = University.DAL.Models;
namespace University.Business.Services.Student
{    
    public class StudentService
    {
        private readonly UnitOfWork _unitOfWork;

        public StudentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns all students within given Student Table
        /// </summary>
        /// <returns></returns>
        public List<Model.Student> GetAll()
        {
            return _unitOfWork.StudentRepository.Get().ToList();
        }

        public Model.Student FindById(int? id)
        {
            return _unitOfWork.StudentRepository.Find(id);
        }
    }
}
