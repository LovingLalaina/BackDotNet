using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using back_dotnet.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace back_dotnet.Models.DTOs.Users
{
    public class CreateUserDto
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
        public Guid Post { get; set; }

        [ModelBinder(Name = "image")]
        [Required(ErrorMessage = "Le fichier est requis")]
        public IFormFile? File { get; set; }

        [ModelBinder(Name = "email")]
        [Required(ErrorMessage = "L'adresse email est requis")]
        [RegularExpression("[0-9a-zA-Z]*@(hairun-technology).(com)", ErrorMessage ="L'adresse email doit être correcte(user@hairun-technology.com)")]
        public string Email { get; set; } = null!;

        [ModelBinder(Name = "password")]
        [Required(ErrorMessage = "Le mot de passe est requis")]
        [Length(8, 255, ErrorMessage = "Le mot de passe doit etre superieur ou égal à 8 et inferieur à 255")]
        public string? Password { get; set; }
    }
}