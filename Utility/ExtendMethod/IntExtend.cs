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

        public static string ConvertStatus(this int status)
        {
            if (status == 1)
            {
                return "审批中";
            }
            else if (status == 2)
            {
                return "通过";
            }
            else if (status == 3)
            {
                return "驳回";
            }
            else if (status == 4)
            {
                return "作废";
            }else if(status == 5)
            {
                return "已审批";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
