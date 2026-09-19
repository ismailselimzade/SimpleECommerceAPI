namespace SimpleECommerceAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, string key) 
            : base($"{entityName} with id '{key}' was not found")
        {}
    }
}
