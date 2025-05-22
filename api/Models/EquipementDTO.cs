using System;
using System.Text.Json.Serialization;

namespace PFE_PROJECT.Models
{
    public class EquipementDTO
    {
        public int idEqpt { get; set; }
        public int idType { get; set; }
        public string? typeDesignation { get; set; }
        public int idCat { get; set; }
        public string? categorieDesignation { get; set; }
        public int idMarq { get; set; }
        public string? marqueNom { get; set; }
        public string codeEqp { get; set; } = string.Empty;
        public string design { get; set; } = string.Empty;
        public string? observation { get; set; }
        public int? idGrpIdq { get; set; }
        public string? groupeIdentiqueDesignation { get; set; }
        public string? etat { get; set; }
        public string numserie { get; set; } = "INCONNU";
        public string position_physique { get; set; } = "INCONNU";
        public DateTime? DateMiseService { get; set; }
        [JsonPropertyName("anneeFabrication")]
        public int? AnnéeFabrication { get; set; }
        public DateTime? DateAcquisition { get; set; }
        public decimal? ValeurAcquisition { get; set; }
        public int? idunite { get; set; }
        public string? uniteDesignation { get; set; }
    }

    public class CreateEquipementDTO
    {
        public int idType { get; set; }
        public int idCat { get; set; }
        public int idMarq { get; set; }
        public string design { get; set; } = string.Empty;
        public string? observation { get; set; }
        public int? idGrpIdq { get; set; }
        public string? etat { get; set; }
        public string numserie { get; set; } = "INCONNU";
        public string position_physique { get; set; } = "INCONNU";
        public DateTime? DateMiseService { get; set; }
        [JsonPropertyName("anneeFabrication")]
        public int? AnnéeFabrication { get; set; }
        public DateTime? DateAcquisition { get; set; }
        public decimal? ValeurAcquisition { get; set; }
        public int? idunite { get; set; }
    }

    public class UpdateEquipementDTO
    {
        public int idType { get; set; }
        public int idCat { get; set; }
        public int idMarq { get; set; }
        public string design { get; set; } = string.Empty;
        public string? observation { get; set; }
        public int? idGrpIdq { get; set; }
        public string? etat { get; set; }
        public string numserie { get; set; } = "INCONNU";
        public string position_physique { get; set; } = "INCONNU";
        public DateTime? DateMiseService { get; set; }
        [JsonPropertyName("anneeFabrication")]
        public int? AnnéeFabrication { get; set; }
        public DateTime? DateAcquisition { get; set; }
        public decimal? ValeurAcquisition { get; set; }
        public int? idunite { get; set; }
    }

    public class EquipementFilterDTO
    {
        public int? idCat { get; set; }
        public string? etat { get; set; }
        public int? idMarq { get; set; }
        public int? idType { get; set; }
        public int? idGrpIdq { get; set; }
        public int? idunite { get; set; }
        public string? numserie { get; set; }
        public string? position_physique { get; set; }
        public string? design { get; set; }
        public DateTime? DateMiseService { get; set; }
        [JsonPropertyName("anneeFabrication")]
        public int? AnnéeFabrication { get; set; }
        public DateTime? DateAcquisition { get; set; }
        public decimal? ValeurAcquisition { get; set; }
        public string? searchTerm { get; set; }
        public string? sortBy { get; set; }
        public bool ascending { get; set; } = true;
    }
} 