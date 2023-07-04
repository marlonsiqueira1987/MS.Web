using Microsoft.VisualStudio.TestTools.UnitTesting;
using MS.Web.UI.Controllers;
using MS.Web.UI.Data;
using MS.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Web.UI.Infra.Helpers;

namespace MS.Web.UI.Controllers.Tests
{
    [TestClass()]
    public class UsuariosControllerTests
    {
        private readonly MSWebDataContext _ctx = new MSWebDataContext();

        [TestMethod()]
        public void IntegracaoBDTabelaUsuarios()
        {
            //Assert.Fail();

            if (_ctx.Usuarios.Count() > 0) 
                { Assert.IsTrue(true, "Existem dados na tabela de Usuários"); }
            else
                { Assert.IsFalse(true, "Não existem dados na tabela de Usuários"); }

            
        }

        [TestMethod()]
        public void VerificaNomePrimeiroRegistro()
        {
            Usuario usuario = new Usuario();

            usuario = _ctx.Usuarios.First();
            if (usuario.Nome != "")
                Assert.IsTrue(true, "O nome do primeiro Usuário é: " + usuario.Nome);
            else
                Assert.IsFalse(true,"Não encontrou nenhum dado na tabela Usuários");
        }

        [TestMethod()]
        public void AddUsuarioInBD()
        {
            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var usuarioCompare = from s in _ctx.Usuarios
                                 select s;

            usuarioCompare = _ctx.Usuarios.Where(s => s.Email.ToLower().Contains("testedotesteunitario@gmail.com"));
            if (usuarioCompare.Count() > 0)
            {
               // Já existe na base de dados....
                Assert.IsTrue(true,"Já existe um e-mail igual a esse na base de dados");
            }
            else if (usuarioCompare.Count() == 0)
            {
                Usuario usuario = new Usuario();

                //Adicionando nomes
                usuario.Nome = "Teste do Teste Unitário";
                usuario.TipoPessoa = "Jurídica";
                usuario.Email = "testedotesteunitario@gmail.com";
                usuario.Senha = "123".Encrypt();
                usuario.CPF = "14.589.654/0001-45";
                usuario.Telefone = "(11)93547-0036";

                _ctx.Usuarios.Add(usuario);
                _ctx.SaveChanges(); // salva as alterações - Commit no banco de dados
                Assert.IsTrue(true,"Usuário adicionado com sucesso!");
            }
            else 
            { 
                Assert.Fail("Falha no método"); 
            }

        }

        [TestMethod()]
        public void VerificaEmailJaCadastrado()
        {
            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var usuarioCompare = from s in _ctx.Usuarios
                                 select s;

            usuarioCompare = _ctx.Usuarios.Where(s => s.Email.ToLower().Contains("sa@gmail.com"));
            if (usuarioCompare.Count() > 0)
            {
                // Já existe na base de dados....
                Assert.IsTrue(true, "Já existe um e-mail igual a esse na base de dados");
            }
            else if (usuarioCompare.Count() == 0)
            {
                Assert.IsFalse(true, "Usuário SA não cadastrado no sistema!");
            }
            else
            {
                Assert.Fail("Falha no método");
            }

        }

        [TestMethod()]
        public void VerificaTipoPessoa()
        {
            // variavel para fazer a pesquisa se o email informado já existe no sistema

            Usuario usuario = new Usuario();
            usuario.TipoPessoa = "";

            if (usuario.TipoPessoa == "")
            {
                // Já existe na base de dados....
                Assert.IsTrue(true, "Tipo de Pessoa não informado!");
            }
            else
            {
                Assert.Fail("Falha no método");
            }

        }

        [TestMethod()]
        public void VerificaTipoPessoaInBD()
        {
            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var usuarioCompare = from s in _ctx.Usuarios
                                 select s;

           foreach(var usuario in usuarioCompare)
           {
                if (usuario.TipoPessoa != "Física" && usuario.TipoPessoa != "Jurídica")
                {
                    Assert.IsFalse(true, "Existem usuário sem Tipo Pessoa informado no Banco de Dados");
                    break;
                }
           }

            // Já existe na base de dados....
            Assert.IsTrue(true, "Tipo de Pessoa informado para todos usuários!");

        }

        [TestMethod()]
        public void DelUsuarioInBD()
        {

            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var usuarioCompare = from s in _ctx.Usuarios
                                 select s;

            usuarioCompare = _ctx.Usuarios.Where(s => s.Email.ToLower().Contains("testedotesteunitario@gmail.com"));

            foreach(var usuario in usuarioCompare)
            {
                _ctx.Usuarios.Remove(usuario); // apaga o usuário
            }

            try
            {
                _ctx.SaveChanges(); // salva as alterações - Commit no banco de dados
                Assert.IsTrue(true, "Usuário removido com sucesso!");
            }
            catch (ArgumentException ae)
            {
                // caso houver algum erro na inserção dos dados o alerta é exibido
                Assert.IsFalse(true, "Erro ao remover usuário. " + ae.Message);
                
            }
        }
    }
}