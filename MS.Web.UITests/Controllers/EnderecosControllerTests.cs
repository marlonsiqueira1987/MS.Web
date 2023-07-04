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

namespace MS.Web.UITests.Controllers
{
    [TestClass]
    public class EnderecosControllerTests
    {
        private readonly MSWebDataContext _ctx = new MSWebDataContext();

        [TestMethod()]
        public void IntegracaoBDTabelaEndereco()
        {
            //Assert.Fail();

            if (_ctx.Enderecos.Count() > 0)
            { Assert.IsTrue(true, "Existem dados na tabela de Usuários"); }
            else
            { Assert.IsFalse(true, "Não existem dados na tabela de Usuários"); }


        }

        [TestMethod()]
        public void VerificaNomePrimeiroRegistro()
        {
            Enderecos endereco = new Enderecos();

            endereco = _ctx.Enderecos.First();
            if (endereco.Logradouro != "")
                Assert.IsTrue(true, "O nome do primeiro Usuário é: " + endereco.Logradouro);
            else
                Assert.IsFalse(true, "Não encontrou nenhum dado na tabela Endereço");
        }

        [TestMethod()]
        public void AddEnderecoInBD()
        {
            // Pega o primeiro registro do Usuário para inserir o endereço com UsuarioId
            Usuario usuario = new Usuario();

            usuario = _ctx.Usuarios.First();

            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var enderecoCompare = from s in _ctx.Enderecos
                                 select s;

            enderecoCompare = enderecoCompare.Where(s => s.Logradouro.ToLower().Contains("Rua Jurunas")
                                                    && s.UsuarioId.ToString().Contains(usuario.Id.ToString())
                                                    && s.TipoEndereco.ToString().Contains("Residencial")
                                                    );
            if (enderecoCompare.Count() > 0)
            {
                // Já existe na base de dados....
                Assert.IsTrue(true, "Já existe um Endereço igual a esse na base de dados.");
            }
            else if (enderecoCompare.Count() == 0)
            {
                Enderecos endereco = new Enderecos();

                //Adicionando nomes
                endereco.TipoEndereco = "Residencial";
                endereco.Logradouro = "Rua Jurunas";
                endereco.Numero = "1";
                endereco.Complemento = "Para deletar";
                endereco.Cep = "13845-376";
                endereco.Bairro = "Jardim Igaçaba";
                endereco.Cidade = "Mogi Guaçu";
                endereco.Estado = "SP";
                endereco.UsuarioId = usuario.Id;

                _ctx.Enderecos.Add(endereco);
                _ctx.SaveChanges(); // salva as alterações - Commit no banco de dados
                Assert.IsTrue(true, "Endereço adicionado com sucesso!");
            }
            else
            {
                Assert.Fail("Falha no método");
            }

        }

        [TestMethod()]
        public void VerificaEnderecoJaCadastrado()
        {
            // Pega o primeiro registro do Usuário para inserir o endereço com UsuarioId
            Usuario usuario = new Usuario();

            usuario = _ctx.Usuarios.First();

            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var enderecoCompare = from s in _ctx.Enderecos
                                  select s;

            enderecoCompare = enderecoCompare.Where(s => s.Logradouro.ToLower().Contains("Rua Jurunas")
                                                    && s.UsuarioId.ToString().Contains(usuario.Id.ToString())
                                                    && s.TipoEndereco.ToString().Contains("Residencial")
                                                    );
            if (enderecoCompare.Count() > 0)
            {
                // Já existe na base de dados....
                Assert.IsTrue(true, "Já existe um Endereço igual a esse na base de dados.");
            }
            else if (enderecoCompare.Count() == 0)
            {
                Assert.IsTrue(true, "Endereço da Rua Jurunas para o " + usuario.Nome + " não cadastrado no sistema!");
            }
            else
            {
                Assert.Fail("Falha no método");
            }

        }

        [TestMethod()]
        public void VerificaTipoEndereco()
        {
            // variavel para fazer a pesquisa se o email informado já existe no sistema

            Enderecos endereco = new Enderecos();
            endereco.TipoEndereco = "";

            if (endereco.TipoEndereco == "")
            {
                // Já existe na base de dados....
                Assert.IsTrue(true, "Tipo de Endereço não informado!");
            }
            else
            {
                Assert.Fail("Falha no método");
            }

        }

        [TestMethod()]
        public void VerificaTipoEnderecoInBD()
        {
            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var enderecoCompare = from s in _ctx.Enderecos
                                 select s;

            foreach (var endereco in enderecoCompare)
            {
                if (endereco.TipoEndereco != "Residencial" && endereco.TipoEndereco != "Comercial")
                {
                    Assert.IsFalse(true, "Existem Endereço sem Tipo de Endereço informado no Banco de Dados");
                    break;
                }
            }

            // Já existe na base de dados....
            Assert.IsTrue(true, "Tipo de Endereço informado para todos usuários!");

        }

        [TestMethod()]
        public void DelEnderecoInBD()
        {

            // variavel para fazer a pesquisa se o email informado já existe no sistema
            var enderecoCompare = from s in _ctx.Enderecos
                                  select s;

            enderecoCompare = enderecoCompare.Where(s => s.Logradouro.ToLower().Contains("Rua Jurunas")
                                                    && s.TipoEndereco.ToString().Contains("Residencial")
                                                    && s.Numero.ToString().Contains("1")
                                                    && s.Complemento.ToString().Contains("Para deletar")
                                                    && s.Cep.ToString().Contains("13845-376")
                                                    );

            foreach (var endereco in enderecoCompare)
            {
                _ctx.Enderecos.Remove(endereco); // apaga o usuário
            }

            try
            {
                _ctx.SaveChanges(); // salva as alterações - Commit no banco de dados
                Assert.IsTrue(true, "Endereço removido com sucesso!");
            }
            catch (ArgumentException ae)
            {
                // caso houver algum erro na inserção dos dados o alerta é exibido
                Assert.IsFalse(true, "Erro ao remover endereço. " + ae.Message);

            }
        }
    }
}
