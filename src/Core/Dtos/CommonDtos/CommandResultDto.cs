using System.Collections.Generic;
using System.Linq;

namespace Core.Dtos.CommonDtos
{
    public class CommandResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }

        public static CommandResultDto UnknownError()
        {
            return new CommandResultDto { Success = false, Message = "خطایی در انجام عملیات اتفاق افتاده است. لطفا دوباره تلاش کنید." };
        }

        public static CommandResultDto NotFound()
        {
            return new CommandResultDto { Success = false, Message = "برای این شناسه اطلاعاتی ثبت نشده است." };
        }

        public static CommandResultDto InvalidModelState(List<string> errors = null)
        {
            var message = "مشکلی در اطلاعات ارسالی وجود دارد. لطفا فرم را مجددا بررسی کنید.";
            if(errors!=null && errors.Any())
            {
                message = string.Join("<br/>", errors);
            }
            return new CommandResultDto { Success = false, Message =  message};
        }

        public static CommandResultDto Failed(string message)
        {
            return new CommandResultDto { Success = false, Message = message };
        }

        public static CommandResultDto Successful()
        {
            return new CommandResultDto { Success = true };
        }
    }
}
