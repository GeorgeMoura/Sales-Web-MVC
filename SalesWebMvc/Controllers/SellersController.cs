using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService sellerService;
        private readonly DepartmentService departmentService;

        public SellersController(SellerService service, DepartmentService departmentService)
        {
            sellerService = service;
            this.departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await departmentService.FindAllAsync();
            var viewmodel = new SellerFormViewModel() { Departments = departments }; //view models são modelos criados para passar dados de multiplas entidades para uma view
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid) //serve pra testar se o modelo foi validado, no caso de um usuário com javascript desabilitado, a validação dos formulários é feita do lado do servidor, nesse caso é necessário validar no controller se o formulário passou por todas as validações das anotations do model
            {
                var departments = await departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            await sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await sellerService.FindByIdAsync(id.Value); // pega o valor caso nao seja nulo
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = "Can't delete this seller because he/she has sales" });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await sellerService.FindByIdAsync(id.Value); // pega o valor caso nao seja nulo
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            List<Department> departments = await departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Seller seller)
        {
            if (!ModelState.IsValid) //serve pra testar se o modelo foi validado, no caso de um usuário com javascript desabilitado, a validação dos formulários é feita do lado do servidor, nesse caso é necessário validar no controller se o formulário passou por todas as validações das anotations do model
            {
                var departments = await departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                await sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }catch(ApplicationException e) { return RedirectToAction(nameof(Error), new { message = e.Message }); }
            //catch (NotFoundException e) { return RedirectToAction(nameof(Error), new { message = e.Message }); }      O catch de applicationException captura ambos por ser um supertipo
            //catch (DbConcurrencyException e) { return RedirectToAction(nameof(Error), new { message = e.Message }); }
        }


        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel() 
            { 
              Message = message, 
              RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //essa chamada para o RequestId serve para pegar Id interno da requisição no framework
            };

            return View(viewModel);
        }
    }
}
