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
using System.Web.UI;

namespace MS.Web.UI.Controllers
{
    /// <summary>
    /// Classe de Controle do Usuário
    /// </summary>
    [Authorize]
    public class EnderecosController : BaseController
    {

        private readonly MSWebDataContext _ctx = new MSWebDataContext();

        /// <summary>
        /// Método para mostrar o View (com argumentos) no Index de Endereço
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
            var enderecos = from s in _ctx.Enderecos
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
                enderecos = enderecos.Where(s => s.Logradouro.Contains(Strcompare));
            }


            if (String.IsNullOrEmpty(SortOrder)) { SortOrder = ""; }

            // Verifica as variáveis passadas para alterar a ordenação
            ViewBag.EnderecoSortParm = SortOrder == "Endereco" ? "endereco_desc" : "Endereco";

            // Verifica a ordenação
            switch (SortOrder)
            {
                case "endereco_desc": // Nome Decrescente
                    enderecos = enderecos.OrderByDescending(s => s.Logradouro);
                    break;
                case "Endereco": // Nome Crescente
                    enderecos = enderecos.OrderBy(s => s.Logradouro);
                    break;
                default: // Caso variavel estiver vazia ordena pelo Id
                    enderecos = enderecos.OrderBy(s => s.Id);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(enderecos.ToPagedList(pageNumber, pageSize)); // retorna a consulta dos dados da tabela para a view Index
        }

        /// <summary>
        /// Verifica o id do Endereço para mostrar as informações na View AddEdit do Endereço
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult AddEdit(int? id)
        {
            Enderecos endereco = new Enderecos();



            int NovoEnd = 0;
            if (id > 9999)
            {
                id = id - 99990;
                NovoEnd = 1;
            }


            if (NovoEnd == 1)
            {
                endereco.UsuarioId = (int)id;
            }
            else
            {
                if (id != null)
                {
                    endereco = _ctx.Enderecos.Find(id);
                }
            }

            // consulta dos tipos de pessoas
            ViewBag.TiposEnderecos = CarregarTipoEnderecos();


            return View(endereco);
        }

        /// <summary>
        /// Método para Criar ou Editar um Usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEdit(Enderecos endereco)
        {
            ViewBag.TiposEnderecos = CarregarTipoEnderecos();
            //ViewBag.NovoEndereco = endereco.UsuarioId;
            // verifica se é válido
            if (ModelState.IsValid)
            {
                // Verifica se o endereço será um novo ou só para edição
                if (endereco.Id > 9999)
                {
                    endereco.Id = 0;
                }

                // Se Id for 0 é um novo usuário
                if (endereco.Id == 0)
                {
                    // variavel para fazer a pesquisa se o email informado já existe no sistema
                    var enderecoCompare = from s in _ctx.Enderecos
                                         select s;
                    enderecoCompare = enderecoCompare.Where(s => s.Logradouro.ToLower().Contains(endereco.Logradouro.ToLower()) 
                                                    && s.UsuarioId.ToString().Contains(endereco.UsuarioId.ToString())
                                                    && s.TipoEndereco.ToString().Contains(endereco.TipoEndereco.ToString())
                                                    );

                    if (endereco.TipoEndereco == "Selecione...")
                    {
                        this.ShowMessage("Por favor, preencha o Tipo Endereço!");
                        return View(endereco);

                    }

                    // caso exista, o alerta é exibido para o usuário e aquelas informações não são cadastradas
                    if (enderecoCompare.Count() > 0)
                    {
                        this.ShowMessage("Já existe um Endereço cadastrado com esse informado. Por Favor, verifique o Endereço!");
                        return View(endereco);
                    }

                    _ctx.Enderecos.Add(endereco);
                }
                else
                {
                    // atualiza as informações do usuário
                    _ctx.Entry(endereco).State = System.Data.Entity.EntityState.Modified;
                }

                try
                {
                    _ctx.SaveChanges(); // salva as alterações - Commit no banco de dados
                }
                catch (Exception ex)
                {
                    // caso houver algum erro na inserção dos dados o alerta é exibido
                    this.ShowMessage("Erro: " + ex.Message);
                    ViewBag.TiposEnderecos = CarregarTipoEnderecos();
                    return View(endereco);

                }

                // Após as ações o usuário é redirecionado para o Index
                return RedirectToAction("../Usuarios/AddEdit/" + endereco.UsuarioId);
            }
            return View(endereco);
        }

        private static List<TiposEnderecosList> CarregarTipoEnderecos()
        {
            return new List<TiposEnderecosList>() {
                new TiposEnderecosList(){ Id = "Selecione...", Name="Selecione..."},
                new TiposEnderecosList(){ Id = "Residencial", Name="Residencial"},
                new TiposEnderecosList(){ Id = "Comercial", Name="Comercial"}
                };
        }

        /// <summary>
        /// Método para excluir um usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelEnd(int id)
        {
            // procura as informações do usuário a ser excluido
            var endereco = _ctx.Enderecos.Find(id);

            _ctx.Enderecos.Remove(endereco); // apaga o endereço
            _ctx.SaveChanges(); // exclui as informações - Commit no banco de dados
            return null;



        }


        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
        }




    }

    internal class TiposEnderecosList
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
    }
}
