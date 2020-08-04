using System;
using Skimur.Data.Models;

namespace Skimur.Data.Services
{
    public interface ISubCssService
    {
        SubCss GetStylesForSub(Guid subId);

        void UpdateStylesForSub(SubCss styles);
    }
}
