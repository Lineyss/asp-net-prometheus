namespace WindowsManager.Models
{
    public class HttpResult<T>
    {
        public T Model { get; }
        public int Status { get; }
        public HttpResult(T Model, int Status)
        {
            this.Model = Model;
            this.Status = Status;
        }
        public HttpResult(int Status)
        {
            this.Status = Status;
        }
    }
}
