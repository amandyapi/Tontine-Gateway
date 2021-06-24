namespace TontineGateway.Models
{
    public class ResponseModel<T>
    {
        public StatusModel Status { get; set; }
        public T Data { get; set; }
    }
}
