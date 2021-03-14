namespace Core.Dtos.CommonDtos
{
    public class ChangeStatusResultDto
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Badge { get; set; }
        public string Text { get; set; }

        public static ChangeStatusResultDto NotPossible(string id)
        {
            return new ChangeStatusResultDto { Id = id, Success = false, Message = "امکان انجام این عملیات وجود ندارد." };
        }

        public static ChangeStatusResultDto NotFound(string id = "")
        {
            return new ChangeStatusResultDto { Id = id, Success = false, Message = "برای این شناسه اطلاعاتی ثبت نشده است." };
        }

        public static ChangeStatusResultDto UnknownError(string id = "")
        {
            return new ChangeStatusResultDto { Id = id, Success = false, Message = "خطایی در حذف پیام اتفاق افتاده است. لطفا دوباره تلاش کنید." };
        }

        public static ChangeStatusResultDto Successful(string id, string badge, string text)
        {
            return new ChangeStatusResultDto
            {
                Id = id,
                Success = true,
                Badge = badge,
                Text = text
            };
        }
    }
}
