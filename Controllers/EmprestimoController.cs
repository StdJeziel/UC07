using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Controllers
{

    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);

            LivroService livroService = new LivroService();
            EmprestimoService emprestimoService = new EmprestimoService();
            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();

            cadModel.Livros = livroService.ListarDisponiveis();
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            EmprestimoService emprestimoService = new EmprestimoService();

            if (viewModel.Emprestimo.Id == 0)
            {
                emprestimoService.Inserir(viewModel.Emprestimo);
            }
            else
            {
                emprestimoService.Atualizar(viewModel.Emprestimo);
            }
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro, int pgAtual = 1)
        {
            Autenticacao.CheckLogin(this);

            FiltrosEmprestimos objFiltro = null;
            if (!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }

            EmprestimoService emprestimoService = new EmprestimoService();
            
            //Para Paginação
            List<Emprestimo> lstEmp = emprestimoService.ListarTodos(objFiltro).ToList();
            List<Emprestimo> lstEmp10 = new List<Emprestimo>();

            int empTotal = lstEmp.Count, pgTotal, max, min = (pgAtual - 1) * 10;

            if (empTotal <= 10)
            {
                max = empTotal;
                pgTotal = 1;
                ViewData["PgTotal"] = pgTotal;
            }

            else
            {
                max = 10;
                pgTotal = empTotal / 10 + 1;
                ViewData["PgTotal"] = pgTotal++;
            }

            for (int e = min; e < (min + max); e++)
            {
                lstEmp10.Add(lstEmp[e]);

                if ((min + max) >= empTotal)
                    max = empTotal - min;
            }

            return View(lstEmp10);
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);

            LivroService livroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            cadModel.Emprestimo = e;

            return View(cadModel);
        }
    }
}