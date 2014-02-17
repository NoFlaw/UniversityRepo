using System.Web.Mvc;
using University.Business;
using University.Data.Entities;
using University.Data.Entities.Models;
using University.Data.UnitOfWork;

namespace University.Web.Controllers
{
    public class StudentController : Controller
    {
        private static readonly UniversityContext Context = new UniversityContext();
    
        // GET: /Student/
        public ActionResult Index()
        {
            var students = StudentService.GetAllStudents(); 
            return View(students);
        }

        // GET: /Student/Details/5
        public ActionResult Details(int id)
        {
            var student = StudentService.FindById(id);
            return student == null ? (ActionResult)HttpNotFound() : View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (!ModelState.IsValid)
                return View(student);
 
            StudentService.Add(student);
            
            UnitOfWork.Save();

            return RedirectToAction("Index");
            
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int id)
        {
            var student = StudentService.FindById(id);

            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            StudentService.UpdateStudent(student);

            UnitOfWork.Save();
            
            return RedirectToAction("Index");
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int id)
        {
            var student = StudentService.FindById(id);

            if (student == null)
                return HttpNotFound();
          
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = StudentService.FindById(id);

            StudentService.Delete(student);

            UnitOfWork.Save();
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Context.Dispose();
           
            base.Dispose(disposing);
        }
    }
}
