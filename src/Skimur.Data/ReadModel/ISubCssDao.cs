using System;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface ISubCssDao
    {
        SubCss GetStylesForSub(Guid subId);
    }
}
