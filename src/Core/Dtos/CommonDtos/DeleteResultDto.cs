namespace Core.Dtos.CommonDtos
{
    public class DeleteResultDto
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static DeleteResultDto NotFound(string id = "")
        {
            return new DeleteResultDto { Id = id, Success = false, Message = "برای این شناسه اطلاعاتی ثبت نشده است." };
        }

        public static DeleteResultDto UnknownError(string id = "")
        {
            return new DeleteResultDto { Id = id, Success = false, Message = "خطایی در حذف پیام اتفاق افتاده است. لطفا دوباره تلاش کنید." };
        }

        public static DeleteResultDto Failed(string message, string id= "")
        {
            return new DeleteResultDto { Id = id, Success = false, Message = message };
        }

        public static DeleteResultDto Successful(string id = "")
        {
            return new DeleteResultDto { Id = id, Success = true };
        }
    }
}
