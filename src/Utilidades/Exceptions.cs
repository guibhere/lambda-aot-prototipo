namespace Utils
{
    public class AddPessoaException : Exception
    {
        public AddPessoaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
