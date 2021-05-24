using System.ComponentModel;

namespace EShop.Core.Model.Enums
{
    public enum CustomerStatus
    {
        [Description("Active")]
        Active,
        [Description("Suspended")]
        Suspended,
        [Description("Banned")]
        Banned,
        [Description("Deleted")]
        Deleted,
        [Description("RegisteredButNotConfirmed")]
        RegisteredButNotConfirmed
    }
}