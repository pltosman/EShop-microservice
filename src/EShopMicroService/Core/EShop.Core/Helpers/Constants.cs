using System;
namespace EShop.Core.Helpers
{
    public static class StringResources
    {
        public const string UNEXPECTED_ERROR = "UnexpectedError";

        public static string MustBeGuid(string fieldName) => $"{fieldName} must be GUID.";


        public const string COMMAND_CUSTOMERID_NOT_NULL = "Command.Validations.NotNull.CustomerId";

        public const string COMMAND_FIRSTNAME_NOT_NULL = "Command.Validations.NotNull.FirstName";
        public const string COMMAND_LASTNAME_NOT_NULL = "Command.Validations.NotNull.LastName";

        public const string COMMAND_REGISTRATIONSOURCE_NOT_NULL = "Command.Validations.NotNull.RegistrationSource";
        public const string COMMAND_LANGUAGEID_NOT_NULL = "Command.Validations.NotNull.LanguageId";

        public const string COMMAND_CUSTOMER_ALREADY_EXISTS = "Command.Customer.AlreadyExists";

        public const string COMMAND_EMAIL_NOT_NULL = "Command.Validations.NotNull.Email";
        public const string COMMAND_EMAIL_NOT_EMPTY = "Command.Validations.NotEmpty.Email";


        public const string COMMAND_PASSWORD_NOT_EMPTY = "Command.Validations.NotEmpty.Password";
        public const string COMMAND_PASSWORD_NOT_NULL = "Command.Validations.NotNull.Password";

        public const string COMMAND_TOKEN_NOT_NULL = "Command.Validations.NotNull.Token";

        public const string COMMAND_CUSTOMER_NOTFOUND = "Command.Customer.NotFound";

        public const string COMMAND_CUSTOMER_INVALIDCREDENTIALS = "Command.Customer.InvalidCredentials";

    

        public const string COMMAND_CUSTOMER_EMAIL_ALREADY_CONFIRMED = "Command.Customer.Email.AlreadyConfirmed";
        public const string COMMAND_CUSTOMER_EMAIL_CONFIRMATIONCODESENT = "Command.Customer.Email.ConfirmationCodeSent";
        public const string COMMAND_CUSTOMER_EMAIL_TOKENEXPIRED = "Command.Customer.Email.TokenExpired";
        public const string COMMAND_CUSTOMER_EMAIL_INVALIDTOKEN = "Command.Customer.Email.InvalidToken";
        public const string COMMAND_CUSTOMER_EMAIL_CONFIRMATIONSUCCESS = "Command.Customer.Email.ConfirmationSuccess";

        public static string COMMAND_CUSTOMER_RESETPASSWORD_TOKENEXPIRED { get; internal set; }
        public static string COMMAND_CUSTOMER_RESETPASSWORD_SUCCESS { get; internal set; }
    }
}