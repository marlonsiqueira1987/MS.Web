using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.Web.UI.Models
{
    [Table(nameof(Produto))]
    public class Produto: Entity
    {
        // Campos da Tabela Produtos do SQL Server Object
        [Required, Column(TypeName ="varchar"), StringLength(100)]
        public string Nome { get; set; }

        [Column(TypeName ="money")]
        public decimal Preco { get; set; }

        [Column("Quantidade")]
        public short Qtde { get; set; }

        public int TipoDeProdutoId { get; set; }

        [ForeignKey(nameof(TipoDeProdutoId))]
        public virtual TipoDeProduto TipoDeProduto { get; set; }

    }
}
