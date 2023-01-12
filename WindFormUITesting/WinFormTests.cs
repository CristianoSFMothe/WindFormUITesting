using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;

namespace WindFormUITesting
{
    [TestClass]
    public class WinFormTests
    {
        static WindowsDriver<WindowsElement> sessionWinForms;

        [ClassInitialize]
        public static void FirstThingsFirst(TestContext testContext)
        {
            AppiumOptions dcWinForms = new AppiumOptions();

            dcWinForms.AddAdditionalCapability("app",
                WindFormUITesting.Properties.Settings.Default.ApplicationPath);
               // @"D:\Documentos\Workspace\Cursos\Udemy\WinAppDriver\WindFormUITesting\DoNotDistrurbMortgageCalculatorFrom1999.exe");

            sessionWinForms = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), dcWinForms);

            //D:\Documentos\Workspace\Cursos\Udemy\WinAppDriver\Application+under+test\DoNotDistrurbMortgageCalculatorFrom1999.exe

            //"D:\Documentos\Workspace\Cursos\Udemy\WinAppDriver\AUT_with_grid_data\DoNotDistrurbMortgageCalculatorFrom1999.exe"
        }
        [TestMethod]
        public void CheckboxTest()
        {
            var check = sessionWinForms.FindElementByName("checkBox1");

            check.Click();

            System.Threading.Thread.Sleep(1000);

            System.Diagnostics.Debug.WriteLine($"Value of checkbox is: {check.Selected}");
        }

        [TestMethod]
        public void RadioTest()
        {
            var radioFirst = sessionWinForms.FindElementByName("First");

            System.Diagnostics.Debug.WriteLine($"**** Value of radio First: {radioFirst.Selected}");

            radioFirst.Click();            

            System.Threading.Thread.Sleep(1000);

            System.Diagnostics.Debug.WriteLine($"**** Value of radio First: {radioFirst.Selected}");

            
        }

        [TestMethod]
        public void ComboTest()
        {
            var combo = sessionWinForms.FindElementByAccessibilityId("comboBox1");

            var open = combo.FindElementByName("Open");

            var listItems = combo.FindElementsByTagName("ListItem");

            Debug.WriteLine($"Before: Number of list items found: {listItems.Count}");

            combo.SendKeys(Keys.Down);

            open.Click();

            listItems = combo.FindElementsByTagName("ListItem");

            Debug.WriteLine($"After: Number of list items found: {listItems.Count}");

            // Verifica o número de elementos no combo
            Assert.AreEqual(6, listItems.Count, "Combo box doen't contain expected number of elements.");

            foreach(var comboKid in listItems)
            {
                if(comboKid.Text == "NJ")
                {
                    // Instancia do WebDriverWait
                    WebDriverWait wdv = new WebDriverWait(sessionWinForms, TimeSpan.FromSeconds(10));
                    wdv.Until(x => comboKid.Displayed);
                    comboKid.Click();
                }
            }

            //sessionWinForms.Quit();
        }

        [TestMethod]
        public void MenuTest()
        {
            var allMenus = sessionWinForms.FindElementsByTagName("MenuItem");

            Debug.WriteLine($"All menu items found by search: {allMenus.Count}");

            foreach(var menu in allMenus)
            {
                Debug.WriteLine($"++++ Menu: {menu.GetAttribute("Name")} - Displayed: {menu.Displayed}");
            }

            foreach (var mainMenuItem in allMenus)
            {
                if(mainMenuItem.GetAttribute("Name").Equals("File"))
                {
                    mainMenuItem.Click();
                    var newMenu = mainMenuItem.FindElementByName("New");

                    WebDriverWait wdv = new WebDriverWait(sessionWinForms, TimeSpan.FromSeconds(10));
                    wdv.Until(x => newMenu.Displayed); 

                    newMenu.Click();

                    System.Threading.Thread.Sleep(400);

                    var lenderMenuThirdLevel = newMenu.FindElementByName("Third");

                    Actions actionForRigthClick = new Actions(sessionWinForms);
                    actionForRigthClick.MoveToElement(lenderMenuThirdLevel);
                    actionForRigthClick.Click();
                    actionForRigthClick.Perform();
                }
            }
        }
    }
}
