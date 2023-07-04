using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.Web.UI.Models
{
    [Table(nameof(TipoDeProduto))]
    public class TipoDeProduto:Entity
    {
        // Campos da Tabela Tipo de Produto do SQL Server Object
        [Required, Column(TypeName ="varchar"), StringLength(100)]
        public string Nome { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
