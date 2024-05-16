using DemoPraktika.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DemoPraktika.DataBase;
using System.Linq;

namespace UnitTestPr
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLoginUserRole1()
        {
            string validLogin = "1";
            string validPassword = "t1Os4A";
            AuthWindow authWindow = new AuthWindow();
            bool authenticationResult = authWindow.Authenticate(validLogin, validPassword);
            Assert.IsTrue(authenticationResult, "Аутентификация должна быть успешной");
        }

        [TestMethod]
        public void TestLoginUserRole2()
        {
            string validLogin = "21";
            string validPassword = "Gm87XHb56b";
            AuthWindow authWindow = new AuthWindow();
            bool authenticationResult = authWindow.Authenticate(validLogin, validPassword);
            Assert.IsTrue(authenticationResult, "Аутентификация должна быть успешной");
        }

        [TestMethod]
        public void TestLoginUserRole3()
        {
            string validLogin = "31";
            string validPassword = "fA5j8aHU33";
            AuthWindow authWindow = new AuthWindow();
            bool authenticationResult = authWindow.Authenticate(validLogin, validPassword);
            Assert.IsTrue(authenticationResult, "Аутентификация должна быть успешной");
        }        
        
        [TestMethod]
        public void TestLoginUserRole4()
        {
            string validLogin = "52";
            string validPassword = "5BIgu5";
            AuthWindow authWindow = new AuthWindow();
            bool authenticationResult = authWindow.Authenticate(validLogin, validPassword);
            Assert.IsTrue(authenticationResult, "Аутентификация должна быть успешной");
        }
        [TestMethod]
        public void TestLoginNegative()
        {
            string invalidLogin = "invalidLogin";
            string invalidPassword = "invalidPassword";
            AuthWindow authWindow = new AuthWindow();
            bool authenticationResult = authWindow.Authenticate(invalidLogin, invalidPassword);
            Assert.IsFalse(authenticationResult, "Аутентификация должна быть неудачной");
        }

        [TestMethod]
        public void CheckPassword_TooShort_Returns1()
        {
            using (MerContext _con = new MerContext())
            {
                User testUser = _con.Users.FirstOrDefault(u => u.Id == 21);
                string password = "short";
                RegZhModWindow rw = new RegZhModWindow(testUser);
                int result = rw.CheckPassword(password);
                Assert.AreEqual(1, result);
            }
        }

        [TestMethod]
        public void CheckPassword_NoUpperCase_Returns2()
        {
            using (MerContext _con = new MerContext())
            {
                User testUser = _con.Users.FirstOrDefault(u => u.Id == 21);
                string password = "nouppercase1";
                RegZhModWindow rw = new RegZhModWindow(testUser);
                int result = rw.CheckPassword(password);
                Assert.AreEqual(2, result);
            }
        }

        [TestMethod]
        public void CheckPassword_NoDigit_Returns3()
        {
            using (MerContext _con = new MerContext())
            {
                User testUser = _con.Users.FirstOrDefault(u => u.Id == 21);
                string password = "NoDigitPassword";
                RegZhModWindow rw = new RegZhModWindow(testUser);
                int result = rw.CheckPassword(password);
                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public void CheckPassword_NoSpecialCharacter_Returns4()
        {
            using (MerContext _con = new MerContext())
            {
                User testUser = _con.Users.FirstOrDefault(u => u.Id == 21);
                string password = "NoSpecialCharacter123";
                RegZhModWindow rw = new RegZhModWindow(testUser);
                int result = rw.CheckPassword(password);
                Assert.AreEqual(4, result);
            }
        }

        [TestMethod]
        public void CheckPassword_ValidPassword_Returns5()
        {
            using (MerContext _con = new MerContext())
            {
                User testUser = _con.Users.FirstOrDefault(u => u.Id == 21);
                string password = "ValidPassword123!";
                RegZhModWindow rw = new RegZhModWindow(testUser);
                int result = rw.CheckPassword(password);
                Assert.AreEqual(5, result);
            }
        }

    }
}
