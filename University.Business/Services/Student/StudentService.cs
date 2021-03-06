﻿using System;
using University.Data.Entities.Models;
using System.Collections.Generic;
using StructureMap;
using University.Data.Repository.Base;

namespace University.Business
{    
    public static class StudentService
    {
        //Get an instance of Students
        private static IRepository<Student> StudentRepository
        {
            get { return ObjectFactory.GetInstance<IRepository<Student>>(); }
        }

        /// <summary>
        /// Returns all students within given Student Table
        /// </summary>
        /// <returns></returns>
        public static List<Student> GetAllStudents()
        {
            return StudentRepository.GetAll() as List<Student>;
        }

        /// <summary>
        /// Returns found student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Student FindById(int id)
        {
            return StudentRepository.GetById(id);
        }

        /// <summary>
        /// Returns a boolean result from adding the student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public static bool Add(Student student)
        {
            if (String.IsNullOrEmpty(student.FirstName) || String.IsNullOrEmpty(student.LastName)) 
                return false;
            
            StudentRepository.Add(student);
            
            return true;
        }
        /// <summary>
        /// Attaches current student to objectgraph, must call save changes.
        /// </summary>
        /// <param name="student"></param>
        public static bool UpdateStudent(Student student)
        {
            if (String.IsNullOrEmpty(student.FirstName) || String.IsNullOrEmpty(student.LastName))
                return false;

            StudentRepository.Update(student);

            return true;
        }


        /// <summary>
        /// Returning boolean result from removing the student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public static bool Delete(Student student)
        {
            if (String.IsNullOrEmpty(student.FirstName) || String.IsNullOrEmpty(student.LastName))
                return false;

            StudentRepository.Delete(student);

            return true;
        }
    }
}
