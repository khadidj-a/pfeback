using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PFE_PROJECT.Models
{
    public class Equipement
    {
        [Key]
        public int idEqpt { get; set; }

        public int idType { get; set; }
        [ForeignKey("idType")]
        public TypeEquip? TypeEquip { get; set; }

        public int idCat { get; set; }
        [ForeignKey("idCat")]
        public Categorie? Categorie { get; set; }

        public int idMarq { get; set; }
        [ForeignKey("idMarq")]
        public Marque? Marque { get; set; }

        [Required]
        [StringLength(50)]
        public string codeEqp { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string design { get; set; } = string.Empty;

        [StringLength(500)]
        public string? observation { get; set; }

        public int? idGrpIdq { get; set; }
        [ForeignKey("idGrpIdq")]
        public GroupeIdentique? GroupeIdentique { get; set; }

        [StringLength(50)]
        [RegularExpression("^(operationnel|En panne|pre_reforme|reforme)$", 
            ErrorMessage = "L'état doit être l'un des suivants: operationnel, En panne, pre_reforme, reforme")]
        public string? etat { get; set; }

        [Required]
        [StringLength(100)]
        public string numserie { get; set; } = "INCONNU";

        [Required]
        [StringLength(255)]
        public string position_physique { get; set; } = "INCONNU";

        public DateTime? DateMiseService { get; set; }

        [JsonPropertyName("anneeFabrication")]
        public int? AnnéeFabrication { get; set; }
        
        public DateTime? DateAcquisition { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ValeurAcquisition { get; set; }

        [ForeignKey("Unite")]
        public int? idunite { get; set; }
        public Unite? Unite { get; set; }
    }
}


