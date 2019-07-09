using System.ComponentModel;

namespace CS.Common.Const
{
    public enum DbLogType
    {
        [Description("Other")]
        Other = 0,
        [Description("Login")]
        Login = 1,
        [Description("Exit")]
        Exit = 2,
        [Description("Access")]
        Visit = 3,
        [Description("New")]
        Create = 4,
        [Description("Delete")]
        Delete = 5,
        [Description("Modify")]
        Update = 6,
        [Description("Submit")]
        Submit = 7,
        [Description("Abnormal")]
        Exception = 8,
        [Description("Cancel")]
        Cancel = 9,

    }
}
