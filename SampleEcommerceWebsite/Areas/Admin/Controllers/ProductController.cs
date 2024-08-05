using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleEcommerceWebsite.DataAccess.Data;
using SampleEcommerceWebsite.DataAccess.Repository;
using SampleEcommerceWebsite.DataAccess.Repository.IRepository;
using SampleEcommerceWebsite.Models;
using SampleEcommerceWebsite.Models.ViewModels;
using SampleEcommerceWebsite.Utility;

namespace SampleEcommerceWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product
                .GetAll(includeProperties:"Category").ToList();
            //Fetching CategoryList
            
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            //Enable the below section only if using ViewBag or ViewData
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //    .GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.CategoryId.ToString()
            //    });
            //Using Viewbag to pass information to View. Lasts only during the http request
            //ViewBag.CategoryList = CategoryList;

            //ViewData["CategoryList"] = CategoryList; //using View Data Method to pass data


            //Using View Models
            ProductViewModel productViewModel = new ProductViewModel()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //insert when ID is not present
                return View(productViewModel);
            }
            else
            {
                //Update when ID is present
                productViewModel.Product = _unitOfWork.Product.Get(u=>u.Id == id);
                return View(productViewModel);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {


            //Inserting data to the table if there are no validation errors
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Assigning random Guid and extension as filename
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    //updating image file
                    if(!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productViewModel.Product.ImageUrl = @"\images\product\" +fileName;
                
                }

                if(productViewModel.Product.Id == 0) 
                {
                    _unitOfWork.Product.Add(productViewModel.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(productViewModel.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");

            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                });
                return View(productViewModel);
            }
            
        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id <= 0)
        //    {
        //        return NotFound();
        //    }
        //    //Works better for primary keys
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{

        //    //Updating data to the table if there are no validation errors
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated successfully!";
        //        return RedirectToAction("Index");

        //    }
        //    return View();
        //}

        //Retrieves the id/Product to be deleted

        //Disabling Delete Functionality after enabling DataTablesApi
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id <= 0)
        //    {
        //        return NotFound();
        //    }
        //    //Works better for primary keys
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDb);
        //}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletPOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
        #region API Calls
        //To implement DataTable
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product
                    .GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u=>u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new { success= false, message="Error while deleting" });
            }
            //delete old image
            var oldImagePath =
                Path.Combine(_webHostEnvironment.WebRootPath,
                productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful!" });
        }

        #endregion
    }


}
