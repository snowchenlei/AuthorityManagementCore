namespace Snow.AuthorityManagement.Core
{
    public class Result<T>
    {
        /// <summary>
        /// 0失败,1成功,2特殊操作
        /// </summary>
        public int State { get; set; }

        public Status Status { get; set; }

        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class Result
    {
        public int State { get; set; }
        public Status Status { get; set; }
        public string Message { get; set; }
    }

    public enum Status
    {
        Success = 200,
        Failure = 500,
        Exception = 400,
    }
}