namespace Utility.ExtendMethod
{
    public static class IntExtend
    {
        public static string ConvertSex(this int sex)
        {
            if (sex == 1) return "男";
            return "女";
        }

        public static string ConvertYesOrNo(this int parm)
        {
            if (parm == 1) return "是";
            return "否";
        }
    }
}
