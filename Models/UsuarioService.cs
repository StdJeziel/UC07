using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
        public void Cria(Usuario u)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                u.Senha = Cripto.TxtCripto(u.Senha);

                bc.Add(u);
                bc.SaveChanges();
            }
        }

        public List<Usuario> Lista()
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.ToList();
            }
        }

        public Usuario Busca(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }

        public void Edita(Usuario uAlt)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario u = bc.Usuarios.Find(uAlt.Id);
                u.Nome = uAlt.Nome;
                u.Login = uAlt.Login;
                u.Senha = Cripto.TxtCripto(uAlt.Senha);
                u.Tipo = uAlt.Tipo;

                bc.SaveChanges();
            }
        }

        public void Exclui(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Remove(bc.Usuarios.Find(id));
                bc.SaveChanges();
            }
        }
    }
}