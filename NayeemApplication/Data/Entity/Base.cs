using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NayeemApplication.Data.Entity
{
    public class Base
    {
        [Key]
        public int Id { get; set; }

    }



}
