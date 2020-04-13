using ExprEvalNetCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExprEvalNetCoreTests
{
    [TestClass]
    public class MathSolverTests
    {
        private MathSolver _solverSvc = new MathSolver();

        [TestMethod]
        public void TestBasicAdd()
        {
            Assert.AreEqual(6, _solverSvc.Solve("1+2+3").Value);
            Assert.AreEqual(6, _solverSvc.Solve("1 + 2 +3").Value);
            Assert.AreEqual(6, _solverSvc.Solve("1                   +2+3").Value);
            Assert.AreEqual(6, _solverSvc.Solve("1+2+3    ").Value);
            Assert.AreEqual(3, _solverSvc.Solve("1+(2)").Value);
            Assert.AreEqual(3, _solverSvc.Solve("1+(+2)").Value);
            Assert.AreEqual(-1, _solverSvc.Solve("1+(-2)").Value);
        }

        [TestMethod]
        public void TestBasicSub()
        {
            Assert.AreEqual(5, _solverSvc.Solve("10-5").Value);
            Assert.AreEqual(-5, _solverSvc.Solve("4 - 9").Value);
            Assert.AreEqual(-1, _solverSvc.Solve("1-2").Value);
            Assert.AreEqual(-1, _solverSvc.Solve("1+(-2)").Value);
            Assert.AreEqual(-3, _solverSvc.Solve("-1-2").Value);
            Assert.AreEqual(3, _solverSvc.Solve("(1*(-1))-(2*(-2))").Value);
            Assert.AreEqual(-7, _solverSvc.Solve("-5-1+2-3").Value);
        }

        [TestMethod]
        public void TestBasicExampleOK()
        {
            var result = _solverSvc.Solve("((1+2)*43)/3.14+2^3");
            Assert.AreEqual(49.0828025477707, result.Value);
            Assert.AreEqual(string.Empty, result.Error);
        }

        [TestMethod]
        public void TestBasicInvalid()
        {
            Assert.AreEqual("Expression error", _solverSvc.Solve("((1+2)").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("((").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("))").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve(")").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("1---2").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("1++++2").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("1++2").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("1/0").Error);
            Assert.AreEqual("Expression error", _solverSvc.Solve("").Error);
        }
    }
}
