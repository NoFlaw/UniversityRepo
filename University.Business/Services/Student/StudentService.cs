using System.Collections.Generic;
using System.Linq;
using University.DAL.UnitOfWork;

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
        public List<DAL.Models.Student> GetAll()
        {
            return _unitOfWork.StudentRepository.Get().ToList();
        }

        public DAL.Models.Student FindById(int? id)
        {
            return _unitOfWork.StudentRepository.FindById(id);
        }
    }
}
