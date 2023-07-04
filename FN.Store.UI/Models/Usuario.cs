using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.Web.UI.Models
{
    [Table(nameof(Usuario))]
    public class Usuario: Entity
    {
        // Campos da Tabela Usuario do SQL Server Object
        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string TipoPessoa { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Senha { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(20)]
        public string CPF { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(20)]
        public string Telefone { get; set; }

        public virtual HashSet<Enderecos> Enderecos { get; set; }

        public static explicit operator int(Usuario v)
        {
            throw new NotImplementedException();
        }

        //[Column(TypeName = "varchar")]
        //[StringLength(10)]
        //public string Cep { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(100)]
        //public string Endereco { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(20)]
        //public string Numero { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(100)]
        //public string Bairro { get; set; }

        //[Column(TypeName = "varchar")]
        //[Required, StringLength(100)]
        //public string Cidade { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(100)]
        //public string Estado { get; set; }

    }
}
