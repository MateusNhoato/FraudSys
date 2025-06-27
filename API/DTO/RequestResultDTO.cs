namespace FraudSys.DTO
{
    public class RequestResultDTO<T>
    {
        public T Result { get; set; }
        public string[] Mensagens { get; set; }
    }
}
