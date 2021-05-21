using System.Collections.Generic;
using System.Text;
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
            
            _logger.LogInformation("");
            return View(mappedCompanies);
        }

        private string BuildCompanyReport()
        {
            var sb = new StringBuilder();
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

            sb.Append(@"<div class='card'>
            <div class='card-header header-elements-inline'>
                <h5 class='card-title'>Company Report</h5>
            </div>

            <div class='table-responsive'>
                <table class='table'>
                    <thead>
                        <tr>

                            <th>Name</th>
                            <th>Telephone</th>
                            <th>City</th>
                            <th>Province</th>
                        </tr>
                    </thead>
                    <tbody>
                    ");

            foreach (var item in mappedCompanies)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>",
                   item.CompanyName,
                   item.Telephone,
                   item.City,
                   item.Province);

            }

            sb.AppendFormat(@"<tr>
                                <td>Total Companies:</td>
                                <td>{0}</td>
                            </tr>", mappedCompanies.Count);

            sb.Append(@"</ tbody>
                </ table >
            </ div >
        </ div >");

            return sb.ToString();
        }

       /* public IActionResult ExportCompanyReport()
        {
            var Renderer = new IronPdf.HtmlToPdf();
            //IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            // add a header to very page easily
            Renderer.PrintOptions.FirstPageNumber = 1;
            Renderer.PrintOptions.Header.DrawDividerLine = true;
            Renderer.PrintOptions.Header.CenterText = "{url}";
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.Header.FontSize = 12;
            Renderer.PrintOptions.CustomCssUrl = webHostEnvironment.WebRootPath + "\\css\\bootstrap.css";
            // add a footer too
            Renderer.PrintOptions.Footer.DrawDividerLine = true;
            Renderer.PrintOptions.Footer.FontFamily = "Arial";
            Renderer.PrintOptions.Footer.FontSize = 10;
            Renderer.PrintOptions.Footer.LeftText = "{date} {time}";
            Renderer.PrintOptions.Footer.RightText = "{page} of {total-pages}";
            var file = Renderer.RenderHtmlAsPdf(BuildCompanyReport());
            var contentLength = file.BinaryData.Length;

            return File(file.BinaryData, "application/pdf;");
        }*/

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
