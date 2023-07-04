using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.Web.UI.Models
{
    [Table(nameof(Enderecos))]
    public class Enderecos : Entity
    {
        // Campos da Tabela Endereco do SQL Server Object
        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Logradouro { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string TipoEndereco { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(10)]
        public string Cep { get; set; }    

        [Column(TypeName = "varchar")]
        [StringLength(20)]
        public string Numero { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(20)]
        public string Complemento { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Bairro { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Cidade { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Estado { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario Usuario { get; set; }

    }
}
