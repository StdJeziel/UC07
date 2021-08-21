using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Livro l)
        {
            LivroService livroService = new LivroService();

            if (l.Id == 0)
            {
                livroService.Inserir(l);
            }
            else
            {
                livroService.Atualizar(l);
            }

            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro, int pgAtual = 1)
        {
            Autenticacao.CheckLogin(this);
            FiltrosLivros objFiltro = null;
            if (!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosLivros();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }

            LivroService livroService = new LivroService();

            //Para Paginação
            List<Livro> lstLiv = livroService.ListarTodos(objFiltro).ToList();
            List<Livro> lstLiv10 = new List<Livro>();

            int livTotal = lstLiv.Count, pgTotal, max, min = (pgAtual - 1) * 10;

            if (livTotal <= 10)
            {
                max = livTotal;
                pgTotal = 1;
                ViewData["PgTotal"] = pgTotal;
            }

            else
            {
                max = 10;
                pgTotal = livTotal / 10 + 1;
                ViewData["PgTotal"] = pgTotal++;
            }

            for (int e = min; e < (min + max); e++)
            {
                lstLiv10.Add(lstLiv[e]);

                if ((min + max) >= livTotal)
                    max = livTotal - min;
            }

            return View(lstLiv10);
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            LivroService ls = new LivroService();
            Livro l = ls.ObterPorId(id);
            return View(l);
        }
    }
}