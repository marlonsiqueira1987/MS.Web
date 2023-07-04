using System;
using System.ComponentModel.DataAnnotations;

namespace MS.Web.UI.Models
{
    public class Entity
    {
        // Campos da Tabela Entity 

        [Key]
        public int Id { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
