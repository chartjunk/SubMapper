using System;
using System.Linq.Expressions;

namespace SubMapper
{
    //public partial class SubMapping<TA, TB, TSubA, TSubB> : BaseMapping<TSubA, TSubB>
    //{
    //    public new SubMapping<TA, TB, TSubA, TSubB> Map<TValue>(
    //        Expression<Func<TSubA, TValue>> getSubAExpr,
    //        Expression<Func<TSubB, TValue>> getSubBExpr)
    //    {
    //        base.Map(getSubAExpr, getSubBExpr);
    //        return this;
    //    }

    //    public new SubMapping<TA, TB, TSubA, TSubB> WithSubMapping<TSubSubA, TSubSubB>(
    //        Expression<Func<TSubA, TSubSubA>> getSubAExpr,
    //        Expression<Func<TSubB, TSubSubB>> getSubBExpr,
    //        Func<SubMappingHandle<TSubA, TSubB, TSubSubA, TSubSubB>, SubMapping<TSubA, TSubB, TSubSubA, TSubSubB>> getSubMapping)
    //        where TSubSubA : new()
    //        where TSubSubB : new()
    //    {
    //        base.WithSubMapping(getSubAExpr, getSubBExpr, getSubMapping);
    //        return this;
    //    }
    //}
}
