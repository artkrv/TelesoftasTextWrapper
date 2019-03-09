using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Telesoftas.Wrapper;

namespace Telesoftas.UT
{
    [TestClass]
    public class WrapperTests
    {
        private ITextWrapper _wrapper = new TextWrapper();

        [TestMethod]
        public void TestFirstCase()
        {
            IEnumerable<string> wrapped = _wrapper.WrapText(new Wrapper.Models.WrapperModel { Length = 13, Text = "Green metal stick" });
            Assert.AreEqual(2, wrapped.Count());
            Assert.AreEqual("Green metal", wrapped.FirstOrDefault());
            Assert.AreEqual("stick", wrapped.LastOrDefault());
        }

        [TestMethod]
        public void TestSecondCase()
        {
            IEnumerable<string> wrapped = _wrapper.WrapText(new Wrapper.Models.WrapperModel { Length = 7, Text = "Establishment of the church" });
            Assert.AreEqual(4, wrapped.Count());
            Assert.AreEqual("Establi", wrapped.FirstOrDefault());
            Assert.AreEqual("church", wrapped.LastOrDefault());
        }

        [TestMethod]
        public void TestThirdCase()
        {
            IEnumerable<string> wrapped = _wrapper.WrapText(new Wrapper.Models.WrapperModel { Length = 999, Text = "Lorem ipsum\r\ndolor sit amet" });
            Assert.AreEqual(2, wrapped.Count());
            Assert.AreEqual("Lorem ipsum", wrapped.FirstOrDefault());
            Assert.AreEqual("dolor sit amet", wrapped.LastOrDefault());
        }

        [TestMethod]
        public void TestFourthCase()
        {
            IEnumerable<string> wrapped = _wrapper.WrapText(new Wrapper.Models.WrapperModel { Length = 3, Text = "1234\r\n1\r\n1234" });
            Assert.AreEqual(5, wrapped.Count());
            Assert.AreEqual("123", wrapped.FirstOrDefault());
            Assert.AreEqual("4", wrapped.LastOrDefault());
        }
    }
}
