using System.Collections.Generic;
using Telesoftas.Wrapper.Models;

namespace Telesoftas.Wrapper
{
    public interface ITextWrapper
    {
        IEnumerable<string> WrapText(WrapperModel model);
        WrapperModel PrepareWrapperModel(string[] args);
    }
}
