using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System.Linq;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {
            if (string.IsNullOrEmpty(controller.HttpContext.Session.GetString("nome")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        public static bool ValidaAcesso(Usuario usr, Controller controller)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                TestaECriaAdmin(bc);

                usr.Senha = Cripto.TxtCripto(usr.Senha);

                IQueryable<Usuario> usrEncont =
                    bc.Usuarios.Where(u => u.Login == usr.Login && u.Senha == usr.Senha);

                List<Usuario> lstUsrEncont = usrEncont.ToList();

                if (lstUsrEncont.Count > 0)
                {
                    controller.HttpContext.Session.SetInt32("id", lstUsrEncont[0].Id);
                    controller.HttpContext.Session.SetString("nome", lstUsrEncont[0].Nome);
                    controller.HttpContext.Session.SetInt32("tipo", lstUsrEncont[0].Tipo);
                    return true;
                }

                else
                    return false;
            }
        }

        public static void TestaECriaAdmin(BibliotecaContext bc)
        {

            IQueryable<Usuario> usrEncont = bc.Usuarios.Where(u => u.Login == "admin");

            if (usrEncont.ToList().Count == 0)
            {
                Usuario usr = new Usuario();

                usr.Nome = "Administrador";
                usr.Login = "admin";
                usr.Senha = Cripto.TxtCripto("123");
                usr.Tipo = Usuario.ADMIN;

                bc.Usuarios.Add(usr);
                bc.SaveChanges();
            }
        }
    }
}