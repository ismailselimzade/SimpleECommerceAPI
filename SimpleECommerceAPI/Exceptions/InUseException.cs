namespace SimpleECommerceAPI.Exceptions
{
    public class InUseException : Exception
    {
        public InUseException(string entityName, string key) 
            : base($"{entityName} with id '{key}' cannot be deleted because it is in use")
        {}
    }
}
