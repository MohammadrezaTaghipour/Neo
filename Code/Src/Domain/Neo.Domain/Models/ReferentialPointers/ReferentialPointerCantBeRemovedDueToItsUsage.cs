using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.ReferentialPointers;

public class ReferentialPointerCantBeRemovedDueToItsUsage : BusinessException
{
    public ReferentialPointerCantBeRemovedDueToItsUsage() :
        base(ReferentialPointerErrorCode.Referential_pointer_cant_be_removed_due_to_its_usage)
    {

    }

    public enum ReferentialPointerErrorCode
    {
        Referential_pointer_cant_be_removed_due_to_its_usage
    }
}