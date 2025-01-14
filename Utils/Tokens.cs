using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_dotnet.Utils
{
    public class Tokens
    {
        // Clé secrète utilisée pour générer le token JWT
        public const string SECRET_KEY_TOKEN_RESEND_MAIL = "JKgej78dsqvztrhzgfzzrzpmlfkshmgsgkhmk";

        // Clé privée utilisée pour la réinitialisation du mot de passe
        public const string RESET_PASSWORD_PRIVATE_KEY = "hairun2024hairunSI2024";

        // Clé du token pour identifier les utilisateurs
        public const string TOKEN_KEY = "a9Xb2Yz8NkL6vP3qR5HdT7j1WpS4oG0B";

        // Nombre de "salt" pour le hachage avec BCrypt
        public const int BCRYPT_SALT = 10;
    }
}