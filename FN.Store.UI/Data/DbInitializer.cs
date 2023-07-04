using MS.Web.UI.Infra.Helpers;
using MS.Web.UI.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace MS.Web.UI.Data
{
    internal class DbInitializer : CreateDatabaseIfNotExists<MSWebDataContext>
    {

        /// <summary>
        /// Método para alimentar o banco de dados na primeira vez que abrir o sistema
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(MSWebDataContext context)
        {
            // Adiciona alguns Usuários
            var usuarios = new List<Usuario>() {
                new Usuario() { Nome = "SA", TipoPessoa = "Física", Email = "sa@gmail.com", Senha = "123".Encrypt(), CPF = "125.478.987-85", Telefone = "(35)92561-8549"},
                new Usuario() { Nome = "Julistino da Sé", TipoPessoa = "Física", Email = "julistino.se@gmail.com", Senha = "123".Encrypt(), CPF = "197.036.247-71", Telefone = "(51)99434-8880"},
                new Usuario() { Nome = "Orlando Madruga da Silva", TipoPessoa = "Jurídica", Email = "orlando.silva@gmail.com", Senha = "123".Encrypt(), CPF = "02.587.412/0001-25", Telefone = "(35)91452-2578"},
                new Usuario() { Nome = "Roberto Firmino", TipoPessoa = "Física", Email = "roberto.firmino@gmail.com", Senha = "123".Encrypt(), CPF = "351.810.458-25", Telefone = "(35)92561-4456"},
                new Usuario() { Nome = "Alberto Soares", TipoPessoa = "Física", Email = "soares.alberto@gmail.com", Senha = "123".Encrypt(), CPF = "330.478.963-12", Telefone = "(35)92561-6865"},
                new Usuario() { Nome = "Julio Paiva", TipoPessoa = "Física", Email = "julio.paiva@gmail.com", Senha = "123".Encrypt(), CPF = "330.478.963-12", Telefone = "(35)92561-8520"},
                new Usuario() { Nome = "Sidney da Silva", TipoPessoa = "Jurídica", Email = "sidney.silva@gmail.com", Senha = "123".Encrypt(), CPF = "14.589.654/0001-45", Telefone = "(11)93547-0036"}
            };

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            // Adiciona alguns Usuários
            var enderecos1 = new List<Enderecos>() {
                new Enderecos() { UsuarioId = 1, TipoEndereco = "Residencial", Cep = "13845-376", Logradouro= "Rua Jurunas", Numero= "50", Bairro= "Jardim Igaçaba", Cidade= "Mogi Guaçu", Estado= "SP" },
                new Enderecos() { UsuarioId = 1, TipoEndereco = "Comercial", Cep = "13845-376", Logradouro= "Rua Jurunas", Numero= "50", Bairro= "Jardim Igaçaba", Cidade= "Mogi Guaçu", Estado= "SP" },
                new Enderecos() { UsuarioId = 1, TipoEndereco = "Residencial", Cep = "13845-375", Logradouro= "Rua Tupinambás", Numero= "454", Bairro= "Jardim Igaçaba", Cidade= "Mogi Guaçu", Estado= "SP" },
                new Enderecos() { UsuarioId = 2, TipoEndereco = "Residencial", Cep = "13845-375", Logradouro= "Rua Tupinambás", Numero= "163", Bairro= "Jardim Igaçaba", Cidade= "Mogi Guaçu", Estado= "SP" },
                new Enderecos() { UsuarioId = 3, TipoEndereco = "Comercial", Cep = "13840-156", Logradouro= "Praça Antônio de M. Filho", Numero= "5", Bairro= "Jardim Camargo", Cidade= "Mogi Guaçu", Estado= "SP" },
                new Enderecos() { UsuarioId = 4, TipoEndereco = "Residencial", Cep = "13846-589", Logradouro= "Rua Osvaldo Firmino Vieira", Numero= "1124", Bairro= "Jardim Monte Líbano", Cidade= "Mogi Guaçu", Estado= "SP" },
                new Enderecos() { UsuarioId = 5, TipoEndereco = "Comercial", Cep = "13800-005", Logradouro= "Praça São José", Numero= "456", Bairro= "Centro", Cidade= "Mogi Mirim", Estado= "SP" },
                new Enderecos() { UsuarioId = 6, TipoEndereco = "Residencial", Cep = "13800-005", Logradouro= "Praça São José", Numero= "7894", Bairro= "Centro", Cidade= "Mogi Mirim", Estado= "SP" },
                new Enderecos() { UsuarioId = 7, TipoEndereco = "Comercial", Cep = "13840-123", Logradouro= "Rua Antônio dos Santos", Numero= "06", Bairro= "Jardim Jacira", Cidade= "Mogi Guaçu", Estado= "SP" }
            };

            context.Enderecos.AddRange(enderecos1);

            // Executa as alterações/adições no banco de dados
            context.SaveChanges();

        }


    }
}