namespace Utility.ApiResult
{
    public static class ApiResulthelp
    {
        public static ApiResult Success(object data,int count =0)
        {
            return new ApiResult
            {
                Code = 200,
                Msg = "成功",
                Count = count,
                Data = data,
            };
        }
        public static ApiResult Error(string msg)
        {
            return new ApiResult
            {
                Code = 500,
                Data = null,
                Msg = msg,
            };
        }
    }
}
