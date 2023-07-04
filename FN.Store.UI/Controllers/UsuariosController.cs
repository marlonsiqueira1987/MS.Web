using MS.Web.UI.Data;
using MS.Web.UI.Infra.Helpers;
using MS.Web.UI.Models;
using MS.Web.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using PagedList;

namespace MS.Web.UI.Controllers
{
    /// <summary>
    /// Classe de Controle do Usuário
    /// </summary>
    [Authorize]
    public class UsuariosController : BaseController
    {
        
        private readonly MSWebDataContext _ctx = new MSWebDataContext();

        /// <summary>
        /// Método para mostrar o View (com argumentos) no Index de Usuário
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="currentFilter"></param>
        /// <param name="SortOrder"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ViewResult Index(string Src, string currentFilter, string SortOrder, int? page)
        {
            // Mantem a variavel de ordenação
            ViewBag.CurrentSort = SortOrder;
            var usuarios = from s in _ctx.Usuarios
                            select s;

            // Verifica se a string Src não é null
            if (!String.IsNullOrEmpty(Src))
            {
                page = 1;
            }
            else
            {
                Src = currentFilter;
            }

            // atribui a variável ViewBag.CurrentFilter a string Src
            ViewBag.CurrentFilter = Src;

            // Verifica se a string Src não é null
            if (!String.IsNullOrEmpty(Src))
            {
                // Altera o tamanho da letra para todas minusculas da variável filtro Src
                string Strcompare = Src.ToLower();

                // acrescenta o where na consulta dos dados conforme variavel iltro Src
                usuarios = usuarios.Where(s => s.Nome.Contains(Strcompare)
                               || s.Email.Contains(Strcompare)
                               || s.CPF.Contains(Strcompare));
            }


            if (String.IsNullOrEmpty(SortOrder)) { SortOrder = ""; }

            // Verifica as variáveis passadas para alterar a ordenação
            ViewBag.NameSortParm = SortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.EmailSortParm = SortOrder == "Email" ? "Email_desc" : "Email";

            // Verifica a ordenação
            switch (SortOrder)
            {
                case "name_desc": // Nome Decrescente
                    usuarios = usuarios.OrderByDescending(s => s.Nome);
                    break;
                case "Name": // Nome Crescente
                    usuarios = usuarios.OrderBy(s => s.Nome);
                    break;
                case "Email_desc": // Email Decrescente
                    usuarios = usuarios.OrderByDescending(s => s.Email);
                    break;
                case "Email": // Email Crescente
                    usuarios = usuarios.OrderBy(s => s.Email);
                    break;
                default: // Caso variavel estiver vazia ordena pelo Id
                    usuarios = usuarios.OrderBy(s => s.Id);
                    break;
            }

            //var qualquer = usuarios.ToPagedList(1, 3);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(usuarios.ToPagedList(pageNumber, pageSize)); // retorna a consulta dos dados da tabela para a view Index
        }

        /// <summary>
        /// Verifica o id do Usuário para mostrar as informações na View AddEdit do Usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult AddEdit(int? id)
        {
            Usuario usuario = new Usuario();

            if (id != null)
            {
                usuario = _ctx.Usuarios.Find(id);
            }

            // consulta dos tipos de pessoas
            ViewBag.TiposPessoas = CarregarTipoPessoas();


            return View(usuario);
        }

        /// <summary>
        /// Método para Criar ou Editar um Usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEdit(Usuario usuario)
        {
            ViewBag.TiposPessoas = CarregarTipoPessoas();
            // verifica se é válido
            if (ModelState.IsValid)
            {
                // Se Id for 0 é um novo usuário
                if (usuario.Id == 0)
                {
                    // variavel para fazer a pesquisa se o email informado já existe no sistema
                    var usuarioCompare = from s in _ctx.Usuarios
                                         select s;
                    usuarioCompare = usuarioCompare.Where(s => s.Email.ToLower().Contains(usuario.Email.ToLower()));

                    if (usuario.TipoPessoa == "Selecione...")
                    {
                        this.ShowMessage("Por favor, preencha o Tipo de Pessoa!");
                        return View(usuario);
                        
                    }

                    // caso exista, o alerta é exibido para o usuário e aquelas informações não são cadastradas
                    if (usuarioCompare.Count() > 0)
                    {
                        this.ShowMessage("Já existe um e-mail cadastrado com esse informado. Por Favor, verifique o E-mail!");
                        return View(usuario);
                    }

                    // Caso não exista nenhum email já cadastrado, é inserido o usuário novo e criptografando a sua senha
                    usuario.Senha = usuario.Senha.Encrypt();
                    _ctx.Usuarios.Add(usuario);
                }
                else
                {
                    // atualiza as informações do usuário
                    _ctx.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                }

                try
                {
                    _ctx.SaveChanges(); // salva as alterações - Commit no banco de dados
                }
                catch (Exception ex)
                {
                    // caso houver algum erro na inserção dos dados o alerta é exibido
                    this.ShowMessage("Erro: " + ex.Message);
                    ViewBag.TiposPessoas = CarregarTipoPessoas();
                    return View(usuario);

                }

                // Após as ações o usuário é redirecionado para o Index
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        private static List<TipospessoasList> CarregarTipoPessoas()
        {
            return new List<TipospessoasList>() {
                new TipospessoasList(){ Id = "Selecione...", Name="Selecione..."},
                new TipospessoasList(){ Id = "Física", Name="Física"},
                new TipospessoasList(){ Id = "Jurídica", Name="Jurídica"}
                };
        }

        /// <summary>
        /// Método para excluir um usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelUsur(int id)
        {
            // procura as informações do usuário a ser excluido
            var usuario = _ctx.Usuarios.Find(id);
            
            // Caso não encontrar é apresendado um erro de Usuário não encontrado
            if (usuario == null)
            {
                return HttpNotFound();
            }

            // Ao encontrar compara se o usuário a ser deletado é o mesmo logado no sistema 
            if (User.Identity.Name == usuario.Email)
            {
                // Caso sim, retorna uma mensagem ao usuário que deve logar com outro usuário para excluir este registro
                //this.ShowMessage("Usuário logado no sistema. Não pode excluir! Faça o login em outra conta!");
                //return RedirectToAction("Index");
                return HttpNotFound();
            } 
            else
            {
                _ctx.Usuarios.Remove(usuario); // apaga o usuário
                _ctx.SaveChanges(); // exclui as informações - Commit no banco de dados
                return null;
            }

            
        }


        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
        }




    }

    internal class TipospessoasList
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
    }
}
