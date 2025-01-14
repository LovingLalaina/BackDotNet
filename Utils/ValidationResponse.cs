using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace back_dotnet.Utils
{
    public static class ValidationResponse
    {
        public static readonly Dictionary<string, List<string>> ErrorMessageList = new Dictionary<string, List<string>> {
            {"isDefined",new List<string>{"est requis","doit être renseigner", "est requise","required","champ existe"}},
            {"isLength",new List<string>{"doit etre superieur ou égal à","Vérifier la longueur de la valeur du champ"}},
            {"isUuid",new List<string>{"être valide","Veuillez inserer un departement valide"}},
            {"isMatch", new List<string>{"user@hairun-technology.com"}},
            };
        private static Dictionary<string, string> MapConstraintPropertyMessage(string messageError)
        {
            var resultErrorMessage = new Dictionary<string, string>();

            foreach (var errorMessage in ErrorMessageList)
            {
                if (errorMessage.Value.Where(e => messageError.Contains(e)).Count() > 0)
                {
                    resultErrorMessage.Add(errorMessage.Key, messageError);
                    break;
                }
            }

            return resultErrorMessage;
        }

        public static List<Object> GetResponseValidation(ModelStateDictionary ModelState)
        {
            var errorValidationResponse = new List<Object>();
            foreach (var Model in ModelState)
            {
                if (Model.Value.Errors.Count > 0)
                {
                    var propertyErrorMessage = new Dictionary<string, string>();
                    foreach (var Error in Model.Value.Errors)
                    {
                        propertyErrorMessage = propertyErrorMessage.Concat(MapConstraintPropertyMessage(Error.ErrorMessage)).ToDictionary(x => x.Key, x => x.Value);
                    }
                    errorValidationResponse.Add(new { property = Model.Key.ToLower(), constraints = propertyErrorMessage });
                }
            }

            return errorValidationResponse;
        }
    }
}