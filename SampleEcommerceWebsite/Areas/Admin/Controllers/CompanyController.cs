using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleEcommerceWebsite.DataAccess.Data;
using SampleEcommerceWebsite.DataAccess.Repository;
using SampleEcommerceWebsite.DataAccess.Repository.IRepository;
using SampleEcommerceWebsite.Models;
using SampleEcommerceWebsite.Models.ViewModels;
using SampleEcommerceWebsite.Utility;


//Adding New Model step 8 - Creating Company Controller
namespace SampleEcommerceWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company
                .GetAll().ToList();
            //Fetching CategoryList
            
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
            
            if (id == null || id == 0)
            {
                //insert when ID is not present
                return View(new Company());
            }
            else
            {
                //Update when ID is present
                Company companyObj = _unitOfWork.Company.Get(u=>u.Id == id);
                return View(companyObj);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {


            //Inserting data to the table if there are no validation errors
            if (ModelState.IsValid)
            {
                if(companyObj.Id == 0) 
                {
                    _unitOfWork.Company.Add(companyObj);

                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully!";
                return RedirectToAction("Index");

            }
            else
            {
                
                return View(companyObj);
            }
            
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletPOST(int? id)
        {
            Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully!";
            return RedirectToAction("Index");
        }
        #region API Calls
        //To implement DataTable
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company
                    .GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u=>u.Id == id);
            if(companyToBeDeleted == null)
            {
                return Json(new { success= false, message="Error while deleting" });
            }
            

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful!" });
        }

        #endregion
    }


}
