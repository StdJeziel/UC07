using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioService us = new UsuarioService();

        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);

            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario usr)
        {
            if (usr.Id != 0)
                us.Edita(usr);
            
            else
                us.Cria(usr);

            return View("Cadastrado");
        }

        public IActionResult Cadastrado()
        {
            Autenticacao.CheckLogin(this);

            return View();
        }

        public IActionResult Listagem()
        {
            Autenticacao.CheckLogin(this);

            List<Usuario> lstUsr = us.Lista();

            return View(lstUsr);
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);

            return View(us.Busca(id));
        }

        public IActionResult Exclusao(int id)
        {
            us.Exclui(id);

            return RedirectToAction("Listagem", "Usuario");
        }
    }
}