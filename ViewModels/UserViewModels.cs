using System;
using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.ViewModels
{
    public class UserViewModel
    {
        public int IdUser { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string NumTel { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int IdRole { get; set; }
        public string RoleLibelle { get; set; } = string.Empty;
        public int? IdUnite { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [StringLength(20)]
        public string NumTel { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string MotPasse { get; set; } = string.Empty;

        [Required]
        [Compare("MotPasse")]
        public string ConfirmMotPasse { get; set; } = string.Empty;

        [Required]
        public int IdRole { get; set; }

        public int? IdUnite { get; set; }
    }

    public class UpdateUserViewModel
    {
        [Required]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [StringLength(20)]
        public string NumTel { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 6)]
        public string MotPasse { get; set; } = string.Empty;

        [Compare("MotPasse")]
        public string ConfirmMotPasse { get; set; } = string.Empty;

        [Required]
        public int IdRole { get; set; }

        public int? IdUnite { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string MotPasse { get; set; } = string.Empty;
    }
}