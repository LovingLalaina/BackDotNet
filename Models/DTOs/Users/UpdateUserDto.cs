using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace back_dotnet.Models.DTOs.Users
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Le nom est requis")]
        [ModelBinder(Name = "firstname")]
        [Length(1, 255, ErrorMessage = "Le nom doit comporter au moins un caractère")]
        public string Firstname { get; set; } = null!;

        [Required(ErrorMessage = "Le prénom est requis")]
        [ModelBinder(Name = "lastname")]
        [Length(1, 255, ErrorMessage = "Le nom doit comporter au moins un caractère")]
        public string Lastname { get; set; } = null!;

        [Required(ErrorMessage = "La date de naissance est requise")]
        [ModelBinder(Name = "birth_date")]
        public DateTime BirthDate { get; set; }
        
        [ModelBinder(Name = "post")]
        [Required(ErrorMessage = "Le poste est requis")]
        [RegularExpression("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", ErrorMessage = "Le poste doit être valide")]
        public required Guid Post { get; set; }

        [ModelBinder(Name = "image")]
        public IFormFile? File { get; set; }
    }
}