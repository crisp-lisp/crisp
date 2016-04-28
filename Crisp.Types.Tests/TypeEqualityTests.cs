using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

namespace Crisp.Types.Tests
{
    [TestClass]
    public class TypeEqualityTests
    {
        private static Fixture _fixture;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void DifferentNumericAtomsWithSameValueShouldBeEqual()
        {
            var value = _fixture.Create<int>();
            var x = new NumericAtom(value);
            var y = new NumericAtom(value);

            Assert.IsTrue(x.Equals(y));
        }

        [TestMethod]
        public void DifferentNumericAtomsWithDifferentValuesShouldNotBeEqual()
        {
            var x = new NumericAtom(1);
            var y = new NumericAtom(2);

            Assert.IsFalse(x.Equals(y));
        }

        [TestMethod]
        public void DifferentConstantAtomsWithSameValueShouldBeEqual()
        {
            var value = _fixture.Create<string>();
            var x = new ConstantAtom(value);
            var y = new ConstantAtom(value);

            Assert.IsTrue(x.Equals(y));
        }

        [TestMethod]
        public void DifferentConstantAtomsWithDifferentValuesShouldNotBeEqual()
        {
            var x = new ConstantAtom("x");
            var y = new ConstantAtom("y");

            Assert.IsFalse(x.Equals(y));
        }

        [TestMethod]
        public void DifferentStringAtomsWithSameValueShouldBeEqual()
        {
            var value = _fixture.Create<string>();
            var x = new StringAtom(value);
            var y = new StringAtom(value);

            Assert.IsTrue(x.Equals(y));
        }

        [TestMethod]
        public void DifferentStringAtomsWithDifferentValuesShouldNotBeEqual()
        {
            var x = new StringAtom("x");
            var y = new StringAtom("y");

            Assert.IsFalse(x.Equals(y));
        }

        [TestMethod]
        public void DifferentSymbolAtomsWithSameValueShouldBeEqual()
        {
            var value = _fixture.Create<string>();
            var x = new SymbolAtom(value);
            var y = new SymbolAtom(value);

            Assert.IsTrue(x.Equals(y));
        }

        [TestMethod]
        public void DifferentSymbolAtomsWithDifferentValuesShouldNotBeEqual()
        {
            var x = new SymbolAtom("x");
            var y = new SymbolAtom("y");

            Assert.IsFalse(x.Equals(y));
        }

        [TestMethod]
        public void DifferentStringBasedAtomTypesShouldNeverBeEqual()
        {
            var value = _fixture.Create<string>();

            var constant = new ConstantAtom(value);
            var str = new StringAtom(value);
            var symbol = new SymbolAtom(value);

            Assert.IsFalse(constant.Equals(str));
            Assert.IsFalse(constant.Equals(symbol));
            Assert.IsFalse(str.Equals(symbol));
        }
    }
}
