using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.CompanyViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ISpaniInnerweb.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly ICompanyService _companyService;
        private readonly ICityService _cityService;
        private readonly IProvinceService _provinceService;
        private readonly ILogger<CompanyController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CompanyController(IAddressService addressService, ICompanyService companyService, ILogger<CompanyController> logger,
            IProvinceService provinceService, ICityService cityService, IWebHostEnvironment webHostEnvironment)
        {
            _companyService = companyService;
            this.webHostEnvironment = webHostEnvironment;
            _addressService = addressService;            
            _cityService = cityService;
            _provinceService = provinceService;
            _logger = logger;
        }
        // GET: CompanyController
        public ActionResult Index()
        {
            var companies = _companyService.GetAll();
            var mappedCompanies = new List<ListCompanyViewModel>();


            foreach (var company in companies)
            {
                if (!company.ProvinceId.Equals("0"))
                {
                    mappedCompanies.Add(new ListCompanyViewModel
                    {
                        CompanyId = company.Id,
                        CompanyName = company.CompanyName,
                        Telephone = company.Telephone,
                        City = _cityService.Get(company.CityId).Name,
                        Province = _provinceService.Get(company.ProvinceId).Name

                    });
                }
            }
            
            return View(mappedCompanies);
        }

        public IActionResult ExportCompanyReport()
        {

            var companies = _companyService.GetAll();
            var mappedCompanies = new List<ListCompanyViewModel>();


            foreach (var company in companies)
            {
                if (!company.ProvinceId.Equals("0"))
                {
                    mappedCompanies.Add(new ListCompanyViewModel
                    {
                        CompanyId = company.Id,
                        CompanyName = company.CompanyName,
                        Telephone = company.Telephone,
                        City = _cityService.Get(company.CityId).Name,
                        Province = _provinceService.Get(company.ProvinceId).Name

                    });
                }
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Companies");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Telephone";
                worksheet.Cell(currentRow, 3).Value = "City";
                worksheet.Cell(currentRow, 4).Value = "Province";

                worksheet.Columns().AdjustToContents();

                foreach (var appliedJob in mappedCompanies)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = appliedJob.CompanyName;
                    worksheet.Cell(currentRow, 2).Value = appliedJob.Telephone;
                    worksheet.Cell(currentRow, 3).Value = appliedJob.City;
                    worksheet.Cell(currentRow, 4).Value = appliedJob.Province;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "companies_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }
            }
        }

        // GET: CompanyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            CreateCompanyViewModel createCompanyViewModel = new CreateCompanyViewModel
            {
                Provinces = _addressService.GetAllProvinces(),
                Cities = _addressService.GetAllCities()
            };

            return View(createCompanyViewModel);
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCompanyViewModel createCompanyViewModel)
        {
            CreateCompanyViewModel createCompanyModel = new CreateCompanyViewModel
            {
                Provinces = _addressService.GetAllProvinces(),
                Cities = _addressService.GetAllCities()
            };

            if (ModelState.IsValid)
            {
                TempData["CompanyCreate"] = "Company Created Successfully";
                //Convert street numbers and 
                _companyService.Create(createCompanyViewModel);
                return View(createCompanyModel);
            }

            return View(createCompanyModel);
            
        }

        // GET: CompanyController/Edit/5
        public ActionResult Edit(string id)
        {

            return View(PrepareCompanyEdit(id));
        }

        //Prevent Code duplication for posting edit and displaying edit
        private EditCompanyViewModel PrepareCompanyEdit(string id)
        {
            var cities = _cityService.GetAll();
            var provinces = _provinceService.GetAll();
            var company = _companyService.Get(id);
            var city = _cityService.Get(company.CityId);
            var province = _provinceService.Get(company.ProvinceId);
            var address = _addressService.Get(company.AddressId);
            var editCompanyViewModel = new EditCompanyViewModel();
            editCompanyViewModel.Cities = cities;
            editCompanyViewModel.Provinces = provinces;
            editCompanyViewModel.BuildingNumber = address.BuildingNumber;
            editCompanyViewModel.StreetName = address.StreetName;
            editCompanyViewModel.StreetNumber = address.StreetNumber;
            editCompanyViewModel.Telephone = company.Telephone;
            editCompanyViewModel.CompanyName = company.CompanyName;
            editCompanyViewModel.CityId = company.CityId;
            editCompanyViewModel.ProvinceId = company.ProvinceId;
            editCompanyViewModel.CompanyId = company.Id;
            return editCompanyViewModel;
        }
        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompany(EditCompanyViewModel editCompanyViewModel, string id)
        {
     
                if (ModelState.IsValid)
                {
                    TempData["CompanyEdit"] = "Company Successfully Updated";
                    editCompanyViewModel.CompanyId = id;
                    _companyService.Update(editCompanyViewModel);
                    return View("Edit", PrepareCompanyEdit(id));
                }
                    
                return View("Edit",PrepareCompanyEdit(id));
        }

        // GET: CompanyController/Delete/5

        [HttpGet]
        public ActionResult Delete(string id)
        {
            TempData["CompanyDeleted"] = "Company Is Deleted";
            //_recruiterService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        // POST: CompanyController/Delete/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
